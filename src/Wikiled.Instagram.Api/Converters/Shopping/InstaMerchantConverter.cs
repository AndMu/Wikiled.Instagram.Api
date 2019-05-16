using System;

namespace Wikiled.Instagram.Api.Converters.Shopping
{
    internal class InstaMerchantConverter : IObjectConverter<InstaMerchant, InstaMerchantResponse>
    {
        public InstaMerchantResponse SourceObject { get; set; }

        public InstaMerchant Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var merchant = new InstaMerchant
                           {
                               Pk = SourceObject.Pk,
                               ProfilePicture = SourceObject.ProfilePicture,
                               Username = SourceObject.Username
                           };
            return merchant;
        }
    }
}
