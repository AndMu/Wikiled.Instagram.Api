namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaPrimaryCountryInfoConverter : IObjectConverter<InstaPrimaryCountryInfo, InstaPrimaryCountryInfoResponse>
    {
        public InstaPrimaryCountryInfoResponse SourceObject { get; set; }

        public InstaPrimaryCountryInfo Convert()
        {
            return new InstaPrimaryCountryInfo
                   {
                       CountryName = SourceObject.CountryName,
                       HasCountry = SourceObject.HasCountry ?? false,
                       IsVisible = SourceObject.IsVisible ?? false
                   };
        }
    }
}
