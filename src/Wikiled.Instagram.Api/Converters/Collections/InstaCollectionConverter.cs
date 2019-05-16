using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Collection;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Collection;

namespace Wikiled.Instagram.Api.Converters.Collections
{
    internal class InstaCollectionConverter : IObjectConverter<InstaCollectionItem, InstaCollectionItemResponse>
    {
        public InstaCollectionItemResponse SourceObject { get; set; }

        public InstaCollectionItem Convert()
        {
            var instaMediaList = new InstaMediaList();

            if (SourceObject.Media != null)
            {
                instaMediaList.AddRange(
                    SourceObject.Media.Medias
                        .Select(InstaConvertersFabric.Instance.GetSingleMediaConverter)
                        .Select(converter => converter.Convert()));
            }

            return new InstaCollectionItem
            {
                CollectionId = SourceObject.CollectionId,
                CollectionName = SourceObject.CollectionName,
                HasRelatedMedia = SourceObject.HasRelatedMedia,
                Media = instaMediaList,
                CoverMedia = SourceObject.CoverMedia != null
                    ? InstaConvertersFabric.Instance.GetCoverMediaConverter(SourceObject.CoverMedia).Convert()
                    : null,
                NextMaxId = SourceObject.NextMaxId
            };
        }
    }
}