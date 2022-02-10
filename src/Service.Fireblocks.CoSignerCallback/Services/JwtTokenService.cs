using System.IO;
using System.Text;
using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using JwtUtils;
using Service.Fireblocks.CoSignerCallback.Models;

namespace Service.Fireblocks.CoSignerCallback.Services
{
    public sealed class JwtTokenService
    {
        private readonly string _privateKey;
        private readonly string _coSignerPubKey;

        public JwtTokenService(string privateKey, string coSignerPubKey)
        {
            _privateKey = privateKey;
            _coSignerPubKey = coSignerPubKey;
        }

        public void ReadJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            //tokenS.v
        }

        public string Create(CallbackResponse callbackResponse)
        {
            var token = JWT.RS256.Create(callbackResponse, _privateKey);

            return token;
        }

        public bool Validate(string token)
        {
            return Validate(token, _coSignerPubKey);
        }

        public bool Validate(string token, string pubKey)
        {
            var isValid = JWT.RS256.ValidateSignature(token, pubKey);

            return isValid;
        }
    }
}
