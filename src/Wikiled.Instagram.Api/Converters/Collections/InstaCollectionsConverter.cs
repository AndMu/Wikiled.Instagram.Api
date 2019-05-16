using System.Collections.Generic;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Collection;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Collection;

namespace Wikiled.Instagram.Api.Converters.Collections
{
    internal class InstaCollectionsConverter : IObjectConverter<InstaCollections, InstaCollectionsResponse>
    {
        public InstaCollectionsResponse SourceObject { get; set; }

        public InstaCollections Convert()
        {
            var instaCollectionList = new List<InstaCollectionItem>();
            instaCollectionList.AddRange(
                SourceObject.Items.Select(InstaConvertersFabric.Instance.GetCollectionConverter)
                    .Select(converter => converter.Convert()));

            return new InstaCollections
            {
                Items = instaCollectionList,
                MoreCollectionsAvailable = SourceObject.MoreAvailable,
                NextMaxId = SourceObject.NextMaxId
            };
        }
    }
}