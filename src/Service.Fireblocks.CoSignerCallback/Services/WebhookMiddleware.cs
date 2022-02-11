using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using Service.Fireblocks.CoSignerCallback.Domain;
using Service.Fireblocks.CoSignerCallback.Domain.Exceptions;
using Service.Fireblocks.CoSignerCallback.Domain.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable InconsistentLogPropertyNaming
// ReSharper disable TemplateIsNotCompileTimeConstantProblem
// ReSharper disable UnusedMember.Global

namespace Service.Fireblocks.CoSignerCallback.Services
{
    public class WebhookMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<WebhookMiddleware> _logger;
        private readonly JwtTokenService _jwtTokenService;


        /// <summary>
        /// Middleware that handles all unhandled exceptions and logs them as errors.
        /// </summary>
        public WebhookMiddleware(
            RequestDelegate next,
            ILogger<WebhookMiddleware> logger,
            JwtTokenService jwtTokenService)
        {
            _next = next;
            _logger = logger;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Invokes the middleware
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            var method = context.Request.Method;
            var query = context.Request.QueryString;
            var body = "--none--";

            if (context.Request.ContentType == "application/grpc")
            {
                _logger.LogInformation($"EXECUTING GRPC METHOD'{path}' | {query} | {method}\n");
                await _next.Invoke(context);

                return;
            }

            using var activity = MyTelemetry.StartActivity("Receive fireblocks cosigner call");

            if (method != "POST")
            {
                _logger.LogInformation($"'{path}' | {query} | {method}\nNO-BODY");
                context.Response.StatusCode = 200;
                _logger.LogWarning("Message from Fireblocks: @{context} method is not POST", body);
                return;
            }

            await using var buffer = new MemoryStream();

            await context.Request.Body.CopyToAsync(buffer);

            buffer.Position = 0L;

            using var reader = new StreamReader(buffer);

            body = await reader.ReadToEndAsync();

            var headers = "Headers: " + string.Join("\n", context.Request.Headers.Select(x => $"{x.Key}: {x.Value}"));
            _logger.LogInformation($"'{path}' | {query} | {method}\n{body}\n{headers}");

            foreach (var header in context.Request.Headers)
            {
                activity.AddTag(header.Key, header.Value);
            }

            activity.AddTag("Body", body);

            try
            {
                var request = _jwtTokenService.GetRequest(body);

                if (request == null || string.IsNullOrEmpty(request.RequestId))
                {
                    _logger.LogError("Can't get request Id: (Impossible to deserialize) {context}", body);
                    context.Response.StatusCode = 400;
                    return;
                }

                if (!_jwtTokenService.Validate(body))
                {
                    _logger.LogError("Can't validate signature from cosigner {context}", body);
                    context.Response.StatusCode = 401;
                    return;
                }

                context.Response.StatusCode = 200;
                var response = new CallbackResponse()
                {
                    Action = "APPROVE",
                    RequestId = request.RequestId,
                    RejectionReason = "",
                };
                var responseStr = _jwtTokenService.Create(response);

                _logger.LogInformation($"Response: '{path}' | {query} | {method}\n{responseStr}\n");

                await context.Response.WriteAsync(responseStr);
            }
            catch (ApiKeysAreNotActivatedException e)
            {
                _logger.LogError(e, "CoSigner Callback Handler keys are not activated! {context}", body);
                context.Response.StatusCode = 500;
                return;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled error {context}", body);
                context.Response.StatusCode = 400;
                return;
            }
        }
    }
}