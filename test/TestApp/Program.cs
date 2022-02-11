using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Service.Fireblocks.CoSignerCallback.Client;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(10);
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            Console.Write("Press enter to start");
            Console.ReadLine();

            var factory = new FireblocksCoSignerCallbackClientFactory("http://localhost:89");
            var encryptionService = factory.GetEncryptionService();

            var coSignerPublicKey = await File.ReadAllTextAsync(@"D:\fireblocks uat\cosigner\cosigner-pub.pem");
            var privateKey = await File.ReadAllTextAsync(@"D:\fireblocks uat\cosigner\private-new.pem");

            await encryptionService.SetApiKeysAsync(new Service.Fireblocks.CoSignerCallback.Grpc.Models.Encryption.SetApiKeyRequest
            {
                CoSignerPubKey = coSignerPublicKey,
                PrivateKey = privateKey
            });

            Console.WriteLine("End");
        }
    }
}