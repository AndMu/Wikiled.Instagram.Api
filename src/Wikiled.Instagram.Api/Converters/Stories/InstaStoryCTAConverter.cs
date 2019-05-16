using System;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryCTAConverter : IObjectConverter<InstaStoryCTA, InstaStoryCTAResponse>
    {
        public InstaStoryCTAResponse SourceObject { get; set; }

        public InstaStoryCTA Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var storyCta = new InstaStoryCTA
                           {
                               AndroidClass = SourceObject.AndroidClass,
                               CallToActionTitle = SourceObject.CallToActionTitle,
                               DeeplinkUri = SourceObject.DeeplinkUri,
                               IgUserId = SourceObject.IgUserId,
                               LeadGenFormId = SourceObject.LeadGenFormId,
                               LinkType = SourceObject.LinkType,
                               Package = SourceObject.Package,
                               RedirectUri = SourceObject.RedirectUri,
                               WebUri = SourceObject.WebUri
                           };

            return storyCta;
        }
    }
}
