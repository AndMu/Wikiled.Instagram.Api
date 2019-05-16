using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStorySliderVoterInfoItemConverter : IObjectConverter<InstaStorySliderVoterInfoItem,
        InstaStorySliderVoterInfoItemResponse>
    {
        public InstaStorySliderVoterInfoItemResponse SourceObject { get; set; }

        public InstaStorySliderVoterInfoItem Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var voterInfoItem = new InstaStorySliderVoterInfoItem
            {
                LatestSliderVoteTime =
                    (SourceObject.LatestSliderVoteTime ?? DateTime.Now.ToUnixTime()).FromUnixTimeSeconds(),
                MaxId = SourceObject.MaxId,
                MoreAvailable = SourceObject.MoreAvailable,
                SliderId = SourceObject.SliderId
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