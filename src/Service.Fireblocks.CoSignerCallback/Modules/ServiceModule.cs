using Autofac;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.NoSql;
using Service.Fireblocks.CoSignerCallback.Services;
using Service.Fireblocks.Signer.NoSql;

namespace Service.Fireblocks.CoSignerCallback.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //var logger = Program.LogFactory.CreateLogger<LoggerMiddleware>();
            var encryptionService = new SymmetricEncryptionService(Program.EnvSettings.GetEncryptionKey());
            builder.RegisterInstance(encryptionService);

            builder.RegisterMyNoSqlWriter<FireblocksApiKeysNoSql>(() => Program.Settings.MyNoSqlWriterUrl, FireblocksApiKeysNoSql.TableName);
        }
    }
}