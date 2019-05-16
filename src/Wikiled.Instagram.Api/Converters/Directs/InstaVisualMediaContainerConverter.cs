using System;
using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Directs
{
    internal class
        InstaVisualMediaContainerConverter : IObjectConverter<InstaVisualMediaContainer,
            InstaVisualMediaContainerResponse>
    {
        public InstaVisualMediaContainerResponse SourceObject { get; set; }

        public InstaVisualMediaContainer Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var visualMedia = new InstaVisualMediaContainer { SeenCount = SourceObject.SeenCount ?? 0 };

            if (SourceObject.UrlExpireAtSecs != null)
            {
                visualMedia.UrlExpireAt = SourceObject.UrlExpireAtSecs.Value.FromUnixTimeSeconds();
            }

            if (SourceObject.ReplayExpiringAtUs != null)
            {
                visualMedia.ReplayExpiringAtUs =
                    DateTime.MinValue /*SourceObject.ReplayExpiringAtUs.Value.FromUnixTimeSeconds()*/;
            }

            if (SourceObject.Media != null)
            {
                visualMedia.Media = InstaConvertersFabric.Instance.GetVisualMediaConverter(SourceObject.Media).Convert();
            }

            if (!string.IsNullOrEmpty(SourceObject.ViewMode))
            {
                visualMedia.ViewMode = (InstaViewMode)Enum.Parse(typeof(InstaViewMode), SourceObject.ViewMode, true);
            }

            if (SourceObject.SeenUserIds?.Count > 0)
            {
                foreach (var user in SourceObject.SeenUserIds)
                {
                    visualMedia.SeenUserIds.Add(user);
                }
            }

            return visualMedia;
        }
    }
}