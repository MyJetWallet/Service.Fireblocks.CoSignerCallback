using MyNoSqlServer.Abstractions;
using Service.Fireblocks.CoSignerCallback.Grpc;
using Service.Fireblocks.CoSignerCallback.Grpc.Models.Encryption;
using Service.Fireblocks.Signer.NoSql;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Service.Fireblocks.CoSignerCallback.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly SymmetricEncryptionService _symmetricEncryptionService;
        private readonly IMyNoSqlServerDataWriter<FireblocksApiKeysNoSql> _myNoSqlServerDataReader;

        public EncryptionService(SymmetricEncryptionService symmetricEncryptionService,
            IMyNoSqlServerDataWriter<FireblocksApiKeysNoSql> myNoSqlServerDataReader)
        {
            _symmetricEncryptionService = symmetricEncryptionService;
            _myNoSqlServerDataReader = myNoSqlServerDataReader;
        }

        public Task<EncryptionResponse> EncryptAsync(EncryptionRequest request)
        {
            var result = _symmetricEncryptionService.Encrypt(request.Data.Trim());

            return Task.FromResult(new EncryptionResponse
            {
                EncryptedData = Convert.ToBase64String(Encoding.UTF8.GetBytes(result.Trim()))
            });
        }

        public async Task<SetApiKeyResponse> SetApiKeysAsync(SetApiKeyRequest request)
        {
            var apiKey = _symmetricEncryptionService.Encrypt(request.ApiKey);
            var privateKey = request.PrivateKey
                .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                .Replace("-----END RSA PRIVATE KEY-----", "");
            var privateKeyEnc = _symmetricEncryptionService.Encrypt(privateKey);

            await _myNoSqlServerDataReader.InsertOrReplaceAsync(FireblocksApiKeysNoSql.Create(apiKey, privateKeyEnc));

            //_keyActivator.ActivateKeys(request.ApiKey, privateKey);

            return new SetApiKeyResponse { };
        }
    }
}
