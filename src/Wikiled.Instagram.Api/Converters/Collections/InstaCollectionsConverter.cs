using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Converters.Collections
{
    internal class InstaCollectionsConverter : IObjectConverter<InstaCollections, InstaCollectionsResponse>
    {
        public InstaCollectionsResponse SourceObject { get; set; }

        public InstaCollections Convert()
        {
            var instaCollectionList = new List<InstaCollectionItem>();
            instaCollectionList.AddRange(
                SourceObject.Items.Select(ConvertersFabric.Instance.GetCollectionConverter)
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
