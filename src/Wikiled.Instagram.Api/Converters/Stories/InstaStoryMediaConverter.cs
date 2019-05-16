using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryMediaConverter : IObjectConverter<InstaStoryMedia, InstaStoryMediaResponse>
    {
        public InstaStoryMediaResponse SourceObject { get; set; }

        public InstaStoryMedia Convert()
        {
            var instaStoryMedia = new InstaStoryMedia
            {
                Media = InstaConvertersFabric.Instance.GetStoryItemConverter(SourceObject.Media).Convert()
            };

            return instaStoryMedia;
        }
    }
}