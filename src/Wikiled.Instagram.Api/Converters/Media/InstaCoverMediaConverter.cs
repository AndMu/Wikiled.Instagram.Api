using System.Collections.Generic;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;

namespace Wikiled.Instagram.Api.Converters.Media
{
    internal class InstaCoverMediaConverter : IObjectConverter<InstaCoverMedia, InstaCoverMediaResponse>
    {
        public InstaCoverMediaResponse SourceObject { get; set; }

        public InstaCoverMedia Convert()
        {
            var instaImageList = new List<InstaImage>();

            if (SourceObject.ImageVersions != null)
            {
                instaImageList.AddRange(
                    SourceObject.ImageVersions.Candidates
                        .Select(InstaConvertersFabric.Instance.GetImageConverter)
                        .Select(converter => converter.Convert()));
            }

            return new InstaCoverMedia
            {
                Id = SourceObject.Id,
                ImageVersions = instaImageList,
                MediaType = SourceObject.MediaType,
                OriginalHeight = SourceObject.OriginalHeight,
                OriginalWidth = SourceObject.OriginalWidth
            };
        }
    }
}