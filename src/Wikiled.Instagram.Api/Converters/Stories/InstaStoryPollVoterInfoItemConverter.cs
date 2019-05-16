using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class
        InstaStoryPollVoterInfoItemConverter : IObjectConverter<InstaStoryPollVoterInfoItem,
            InstaStoryPollVoterInfoItemResponse>
    {
        public InstaStoryPollVoterInfoItemResponse SourceObject { get; set; }

        public InstaStoryPollVoterInfoItem Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var voterInfoItem = new InstaStoryPollVoterInfoItem
            {
                LatestPollVoteTime =
                    (SourceObject.LatestPollVoteTime ?? DateTime.Now.ToUnixTime()).FromUnixTimeSeconds(),
                MaxId = SourceObject.MaxId,
                MoreAvailable = SourceObject.MoreAvailable,
                PollId = SourceObject.PollId
            };

            if (SourceObject.Voters?.Count > 0)
            {
                foreach (var voter in SourceObject.Voters)
                {
                    voterInfoItem.Voters.Add(InstaConvertersFabric.Instance.GetStoryPollVoterItemConverter(voter).Convert());
                }
            }

            return voterInfoItem;
        }
    }
}