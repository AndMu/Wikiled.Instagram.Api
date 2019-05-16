using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class
        InstaStoryCountdownListConverter : IObjectConverter<InstaStoryCountdownList, InstaStoryCountdownListResponse>
    {
        public InstaStoryCountdownListResponse SourceObject { get; set; }

        public InstaStoryCountdownList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var storyCountdownList = new InstaStoryCountdownList
            {
                MoreAvailable = SourceObject.MoreAvailable ?? false, MaxId = SourceObject.MaxId
            };

            if (SourceObject.Items?.Count > 0)
            {
                foreach (var countdown in SourceObject.Items)
                {
                    storyCountdownList.Items.Add(InstaConvertersFabric.Instance
                                                     .GetStoryCountdownStickerItemConverter(countdown)
                                                     .Convert());
                }
            }

            return storyCountdownList;
        }
    }
}