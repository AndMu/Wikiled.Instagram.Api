using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryCtaConverter : IObjectConverter<InstaStoryCta, InstaStoryCtaResponse>
    {
        public InstaStoryCtaResponse SourceObject { get; set; }

        public InstaStoryCta Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var storyCta = new InstaStoryCta
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