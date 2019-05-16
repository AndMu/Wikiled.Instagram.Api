using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryTrayConverter : IObjectConverter<InstaStoryTray, InstaStoryTrayResponse>
    {
        public InstaStoryTrayResponse SourceObject { get; set; }

        public InstaStoryTray Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var storyTray = new InstaStoryTray
            {
                Id = SourceObject.Id,
                IsPortrait = SourceObject.IsPortrait,
                TopLive = InstaConvertersFabric.Instance.GetTopLiveConverter(SourceObject.TopLive).Convert()
            };

            if (SourceObject.Tray != null)
            {
                foreach (var item in SourceObject.Tray)
                {
                    var story = InstaConvertersFabric.Instance.GetStoryConverter(item).Convert();
                    storyTray.Tray.Add(story);
                }
            }

            return storyTray;
        }
    }
}