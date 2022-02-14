using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Service.Fireblocks.CoSignerCallback.Domain;
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
        private readonly IMyNoSqlServerDataWriter<CoSignerApiKeysNoSql> _myNoSqlServerDataReader;
        private readonly KeyActivator _keyActivator;
        private readonly ILogger<EncryptionService> _logger;

        public EncryptionService(SymmetricEncryptionService symmetricEncryptionService,
            IMyNoSqlServerDataWriter<CoSignerApiKeysNoSql> myNoSqlServerDataReader,
            KeyActivator keyActivator,
            ILogger<EncryptionService> logger)
        {
            _symmetricEncryptionService = symmetricEncryptionService;
            _myNoSqlServerDataReader = myNoSqlServerDataReader;
            _keyActivator = keyActivator;
            _logger = logger;
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
            _logger.LogInformation("Setting keys");

            var coSignerKey = request.CoSignerPubKey
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "");

            var coSignerKeyEnc = _symmetricEncryptionService.Encrypt(coSignerKey); 
            var privateKey = request.PrivateKey
                .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                .Replace("-----END RSA PRIVATE KEY-----", "");

            var privateKeyEnc = _symmetricEncryptionService.Encrypt(privateKey);

            await _myNoSqlServerDataReader.InsertOrReplaceAsync(CoSignerApiKeysNoSql.Create(coSignerKeyEnc, privateKeyEnc));

            _keyActivator.Activate(coSignerKey, privateKey);

            _logger.LogInformation("Keys are set");

            return new SetApiKeyResponse { };
        }
    }
}
