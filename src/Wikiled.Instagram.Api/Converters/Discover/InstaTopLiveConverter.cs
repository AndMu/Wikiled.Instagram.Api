using System;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;

namespace Wikiled.Instagram.Api.Converters.Discover
{
    internal class InstaTopLiveConverter : IObjectConverter<InstaTopLive, InstaTopLiveResponse>
    {
        public InstaTopLiveResponse SourceObject { get; set; }

        public InstaTopLive Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var storyTray = new InstaTopLive { RankedPosition = SourceObject.RankedPosition };
            foreach (var owner in SourceObject.BroadcastOwners)
            {
                var userOwner = InstaConvertersFabric.Instance.GetUserShortFriendshipFullConverter(owner).Convert();
                storyTray.BroadcastOwners.Add(userOwner);
            }

            return storyTray;
        }
    }
}