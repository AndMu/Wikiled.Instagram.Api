using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class
        InstaStoryQuestionInfoConverter : IObjectConverter<InstaStoryQuestionInfo, InstaStoryQuestionInfoResponse>
    {
        public InstaStoryQuestionInfoResponse SourceObject { get; set; }

        public InstaStoryQuestionInfo Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var questionInfo = new InstaStoryQuestionInfo
            {
                BackgroundColor = SourceObject.BackgroundColor,
                LatestQuestionResponseTime = (SourceObject.LatestQuestionResponseTime ??
                    DateTime.UtcNow.ToUnixTime()).FromUnixTimeSeconds(),
                MaxId = SourceObject.MaxId,
                MoreAvailable = SourceObject.MoreAvailable ?? false,
                Question = SourceObject.Question,
                QuestionId = SourceObject.QuestionId,
                QuestionResponseCount = SourceObject.QuestionResponseCount ?? 0,
                QuestionType = SourceObject.QuestionType,
                TextColor = SourceObject.TextColor
            };

            if (SourceObject.Responders?.Count > 0)
            {
                foreach (var responder in SourceObject.Responders)
                {
                    questionInfo.Responders.Add(
                        InstaConvertersFabric.Instance
                            .GetStoryQuestionResponderConverter(responder)
                            .Convert());
                }
            }

            return questionInfo;
        }
    }
}