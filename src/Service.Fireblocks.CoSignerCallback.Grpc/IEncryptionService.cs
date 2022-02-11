﻿using System.ServiceModel;
using System.Threading.Tasks;
using Service.Fireblocks.CoSignerCallback.Grpc.Models.Encryption;

namespace Service.Fireblocks.CoSignerCallback.Grpc
{
    [ServiceContract]
    public interface IEncryptionService
    {
        [OperationContract]
        Task<EncryptionResponse> EncryptAsync(EncryptionRequest request);

        [OperationContract]
        Task<SetApiKeyResponse> SetApiKeysAsync(SetApiKeyRequest request);
    }
}