using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Shopping;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping;

namespace Wikiled.Instagram.Api.Converters.Shopping
{
    internal class
        InstaProductMediaListConverter : IObjectConverter<InstaProductMediaList, InstaProductMediaListResponse>
    {
        public InstaProductMediaListResponse SourceObject { get; set; }

        public InstaProductMediaList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var productMedia = new InstaProductMediaList
            {
                AutoLoadMoreEnabled = SourceObject.AutoLoadMoreEnabled,
                MoreAvailable = SourceObject.MoreAvailable,
                NextMaxId = SourceObject.NextMaxId,
                ResultsCount = SourceObject.ResultsCount,
                TotalCount = SourceObject.TotalCount
            };
            if (SourceObject.Medias != null && SourceObject.Medias.Any())
            {
                foreach (var media in SourceObject.Medias)
                {
                    productMedia.Medias.Add(InstaConvertersFabric.Instance.GetSingleMediaConverter(media).Convert());
                }
            }

            return productMedia;
        }
    }
}