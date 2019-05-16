using System;

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
                                Time = DateTimeHelper.FromUnixTimeSeconds(SourceObject.Ts),
                                User = ConvertersFabric.Instance.GetUserShortFriendshipConverter(SourceObject.User).Convert()
                            };

            return voterItem;
        }
    }
}
