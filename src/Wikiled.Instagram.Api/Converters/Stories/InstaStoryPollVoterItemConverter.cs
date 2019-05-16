using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryPollVoterItemConverter : IObjectConverter<InstaStoryVoterItem, InstaStoryVoterItemResponse>
    {
        public InstaStoryVoterItemResponse SourceObject { get; set; }

        public InstaStoryVoterItem Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var voterItem = new InstaStoryVoterItem
            {
                Vote = SourceObject.Vote ?? 0,
                Time = SourceObject.Ts.FromUnixTimeSeconds(),
                User = InstaConvertersFabric.Instance.GetUserShortFriendshipConverter(SourceObject.User).Convert()
            };

            return voterItem;
        }
    }
}