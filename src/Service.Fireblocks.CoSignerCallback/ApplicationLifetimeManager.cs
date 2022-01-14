using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.Service;
using MyJetWallet.Sdk.ServiceBus;
using MyServiceBus.TcpClient;
using Service.Fireblocks.CoSignerCallback.Services;

namespace Service.Fireblocks.CoSignerCallback
{
    public class ApplicationLifetimeManager : ApplicationLifetimeManagerBase
    {
        private readonly ILogger<ApplicationLifetimeManager> _logger;
        private readonly MyNoSqlClientLifeTime _myNoSqlClient;
        private readonly ServiceBusLifeTime _myServiceBusTcpClient;

        public ApplicationLifetimeManager(
            IHostApplicationLifetime appLifetime,
            ILogger<ApplicationLifetimeManager> logger)
            : base(appLifetime)
        {
            _logger = logger;
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called");
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called");
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called");
        }
    }
}