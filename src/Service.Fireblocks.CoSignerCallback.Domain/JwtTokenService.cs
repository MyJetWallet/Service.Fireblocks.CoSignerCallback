using JwtUtils;
using Service.Fireblocks.CoSignerCallback.Domain.Exceptions;
using Service.Fireblocks.CoSignerCallback.Domain.Models;

namespace Service.Fireblocks.CoSignerCallback.Domain
{
    public sealed class JwtTokenService
    {
        private readonly KeyActivator _keyActivator;

        public JwtTokenService(KeyActivator keyActivator)
        {
            _keyActivator = keyActivator;
        }

        public string Create(CallbackResponse callbackResponse)
        {
            CheckThatKeysAreActivated();

            var token = JWT.RS256.Create(callbackResponse, _keyActivator.PrivateKey);

            return token;
        }

        public CallbackRequestWithId GetRequest(string token)
        {
            var request = JWT.Read(token);

            if (request.TryGetValue("requestId", out var requestId))
                return new CallbackRequestWithId
                {
                    RequestId = requestId.ToString()
                };

            return null;
        }

        public bool Validate(string token)
        {
            if (!_keyActivator.IsActivated)
                throw new ApiKeysAreNotActivatedException("Api keys are not activated");

            return Validate(token, _keyActivator.CoSignerPubKey);
        }

        public bool Validate(string token, string pubKey)
        {
            var isValid = JWT.RS256.ValidateSignature(token, pubKey);

            return isValid;
        }

        private void CheckThatKeysAreActivated()
        {
            if (!_keyActivator.IsActivated)
                throw new ApiKeysAreNotActivatedException("Api keys are not activated");
        }
    }
}
