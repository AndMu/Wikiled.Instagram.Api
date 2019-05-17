using System;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logic;

namespace Wikiled.Instagram.Api.Serialization
{
    public class EncryptedSerializer : ISerializer
    {
        private readonly ISerializer inner;

        private readonly IInstaApi api;

        public EncryptedSerializer(ISerializer inner, IInstaApi api)
        {
            this.inner = inner ?? throw new ArgumentNullException(nameof(inner));
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public string Serialize<T>(T instance)
        {
            return Encrypt.EncryptString(inner.Serialize(instance), api.GetStateData().UserSession.Password);
        }

        public T DeSerialize<T>(string text)
        {
            return inner.DeSerialize<T>(Encrypt.DecryptString(text, api.GetStateData().UserSession.Password));
        }
    }
}
