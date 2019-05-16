using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class
        InstaStoryPollVotersListConverter : IObjectConverter<InstaStoryPollVotersList, InstaStoryPollVotersListResponse>
    {
        public InstaStoryPollVotersListResponse SourceObject { get; set; }

        public InstaStoryPollVotersList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var votersList = new InstaStoryPollVotersList
            {
                LatestPollVoteTime = (SourceObject.LatestPollVoteTime ?? 0).FromUnixTimeSeconds(),
                MaxId = SourceObject.MaxId,
                MoreAvailable = SourceObject.MoreAvailable,
                PollId = SourceObject.PollId
            };

            if (SourceObject.Voters?.Count > 0)
            {
                foreach (var voter in SourceObject.Voters)
                {
                    votersList.Voters.Add(InstaConvertersFabric.Instance.GetStoryPollVoterItemConverter(voter).Convert());
                }
            }

            return votersList;
        }
    }
}