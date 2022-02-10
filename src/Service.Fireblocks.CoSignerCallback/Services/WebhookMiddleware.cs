using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using Service.Fireblocks.CoSignerCallback.Models;
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


        /// <summary>
        /// Middleware that handles all unhandled exceptions and logs them as errors.
        /// </summary>
        public WebhookMiddleware(
            RequestDelegate next,
            ILogger<WebhookMiddleware> logger)
        {
            _next = next;
            _logger = logger;

        }

        /// <summary>
        /// Invokes the middleware
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            using var activity = MyTelemetry.StartActivity("Receive fireblocks cosigner call");

            var path = context.Request.Path;
            var method = context.Request.Method;

            var body = "--none--";
            var query = context.Request.QueryString;
            var signature = "";
            //var signature = context.Request.Headers["Fireblocks-Signature"].FirstOrDefault();
            byte[] bodyArray;

            if (method != "POST")
            {
                _logger.LogInformation($"'{path}' | {query} | {method}\nNO-BODY\n{signature}");
                context.Response.StatusCode = 200;
                _logger.LogWarning("Message from Fireblocks: @{context} method is not POST", body);
                return;
            }

            await using var buffer = new MemoryStream();

            await context.Request.Body.CopyToAsync(buffer);

            buffer.Position = 0L;

            using var reader = new StreamReader(buffer);

            body = await reader.ReadToEndAsync();
            //bodyArray = buffer.GetBuffer();

            var headers = "Headers: " + string.Join("\n", context.Request.Headers.Select(x => $"{x.Key}: {x.Value}"));
            _logger.LogInformation($"'{path}' | {query} | {method}\n{body}\n{signature}\n{headers}");

            //Fireblocks - Signature = Base64(RSA512(WEBHOOK_PRIVATE_KEY, SHA512(eventBody)))

            //if (!CryptoProvider.VerifySignature(bodyArray, Convert.FromBase64String(signature)))
            //{
            //    //context.Response.StatusCode = 401;
            //    var bAStr = Convert.ToBase64String(bodyArray);
            //    _logger.LogWarning("Message from Fireblocks: {context} webhook can't be verified", new { 
            //        Body = body,
            //        Signature = signature, 
            //        BodyBase64 = bAStr });

            //    //return;
            //} else
            //{
            //    var bAStr = Convert.ToBase64String(bodyArray);
            //    _logger.LogInformation("Body Array: {context} webhook is verified", new { Signature = signature, Body = bAStr });
            //}

            foreach (var header in context.Request.Headers)
            {
                activity.AddTag(header.Key, header.Value);
            }

            activity.AddTag("Body", body);

            try
            {
                var request = Newtonsoft.Json.JsonConvert.DeserializeObject<CallbackRequestWithId>(body);

                if (request == null || string.IsNullOrEmpty(request.RequestId))
                {
                    _logger.LogError("Can't get request Id: (Impossible to deserialize) {context}", body);
                    context.Response.StatusCode = 400;
                    return;
                }

                context.Response.StatusCode = 200;
                var response = new CallbackResponse()
                {
                    Action = "APPROVE",
                    RequestId = request.RequestId,
                    RejectionReason = "",
                };
                var responseStr = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(responseStr);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Can't get request Id: (Impossible to deserialize) {context}", body);
                context.Response.StatusCode = 400;
            }
        }
    }
}