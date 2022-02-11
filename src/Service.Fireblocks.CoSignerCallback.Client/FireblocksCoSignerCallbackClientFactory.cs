using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.Fireblocks.CoSignerCallback.Grpc;

namespace Service.Fireblocks.CoSignerCallback.Client
{
    [UsedImplicitly]
    public class FireblocksCoSignerCallbackClientFactory : MyGrpcClientFactory
    {
        public FireblocksCoSignerCallbackClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IEncryptionService GetEncryptionService() => CreateGrpcService<IEncryptionService>();
    }
}
