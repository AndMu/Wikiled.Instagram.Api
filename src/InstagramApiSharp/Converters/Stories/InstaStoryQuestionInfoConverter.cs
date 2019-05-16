using System;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryQuestionInfoConverter : IObjectConverter<InstaStoryQuestionInfo, InstaStoryQuestionInfoResponse>
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
                                   LatestQuestionResponseTime = DateTimeHelper.FromUnixTimeSeconds(
                                       SourceObject.LatestQuestionResponseTime ??
                                       DateTime.UtcNow.ToUnixTime()),
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
                        ConvertersFabric.Instance
                                        .GetStoryQuestionResponderConverter(responder).Convert());
                }
            }

            return questionInfo;
        }
    }
}
