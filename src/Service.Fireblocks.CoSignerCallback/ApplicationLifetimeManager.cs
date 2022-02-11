using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.Service;
using MyJetWallet.Sdk.ServiceBus;
using MyNoSqlServer.Abstractions;
using MyServiceBus.TcpClient;
using Service.Fireblocks.CoSignerCallback.Domain;
using Service.Fireblocks.CoSignerCallback.Services;
using Service.Fireblocks.Signer.NoSql;

namespace Service.Fireblocks.CoSignerCallback
{
    public class ApplicationLifetimeManager : ApplicationLifetimeManagerBase
    {
        private readonly ILogger<ApplicationLifetimeManager> _logger;
        private readonly IMyNoSqlServerDataWriter<CoSignerApiKeysNoSql> _myNoSqlServerData;
        private readonly SymmetricEncryptionService _symmetricEncryptionService;
        //private readonly MyNoSqlClientLifeTime _myNoSqlClient;
        private readonly KeyActivator _keyActivator;

        public ApplicationLifetimeManager(
            IHostApplicationLifetime appLifetime,
            ILogger<ApplicationLifetimeManager> logger,
            IMyNoSqlServerDataWriter<CoSignerApiKeysNoSql> myNoSqlServerData,
            SymmetricEncryptionService symmetricEncryptionService,
            //MyNoSqlClientLifeTime myNoSqlClient,
            KeyActivator keyActivator)
            : base(appLifetime)
        {
            _logger = logger;
            _myNoSqlServerData = myNoSqlServerData;
            _symmetricEncryptionService = symmetricEncryptionService;
            //_myNoSqlClient = myNoSqlClient;
            _keyActivator = keyActivator;
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");
            //_myNoSqlClient.Start();
            var key = _myNoSqlServerData.GetAsync(CoSignerApiKeysNoSql.GeneratePartitionKey(), CoSignerApiKeysNoSql.GenerateRowKey()).Result;

            if (key != null)
            {
                try
                {
                    var coSignerPubKey = _symmetricEncryptionService.Decrypt(key.CoSignerPubKey);
                    var privateKey = _symmetricEncryptionService.Decrypt(key.PrivateKey);
                    _keyActivator.Activate(coSignerPubKey, privateKey);
                }
                catch (System.Exception e)
                {
                    _logger.LogError(e, "PLS >:( SET UP KEYS FOR COSIGNER CALLBACK HANDLER!");
                }
            } else
            {
                _logger.LogError("PLS >:( SET UP KEYS FOR COSIGNER CALLBACK HANDLER!");
            }
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
            //_myNoSqlClient.Stop();
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }
    }
}