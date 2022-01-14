using Autofac;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.ServiceBus;
using Service.AssetsDictionary.Client;
using Service.Blockchain.Wallets.Client;
using Service.Blockchain.Wallets.MyNoSql.Addresses;
using Service.Blockchain.Wallets.MyNoSql.AssetsMappings;
using Service.Fireblocks.Api.Client;

namespace Service.Fireblocks.CoSignerCallback.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
        }
    }
}