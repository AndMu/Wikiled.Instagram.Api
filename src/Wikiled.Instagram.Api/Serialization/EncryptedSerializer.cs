using System;
using Wikiled.Common.Utilities.Auth;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logic;

namespace Wikiled.Instagram.Api.Serialization
{
    public class EncryptedSerializer : ISerializer
    {
        private readonly ISerializer inner;

        private readonly IInstaApi api;

        private readonly IEncryptor encryptor;

        public EncryptedSerializer(ISerializer inner, IInstaApi api, IEncryptor encryptor)
        {
            this.inner = inner ?? throw new ArgumentNullException(nameof(inner));
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.encryptor = encryptor ?? throw new ArgumentNullException(nameof(encryptor));
        }

        public string Serialize<T>(T instance)
        {
            return encryptor.EncryptString(inner.Serialize(instance), api.GetStateData().UserSession.Password);
        }

        public T DeSerialize<T>(string text)
        {
            return inner.DeSerialize<T>(encryptor.DecryptString(text, api.GetStateData().UserSession.Password));
        }
    }
}
