using System;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryQuestionResponderConverter : IObjectConverter<InstaStoryQuestionResponder, InstaStoryQuestionResponderResponse>
    {
        public InstaStoryQuestionResponderResponse SourceObject { get; set; }

        public InstaStoryQuestionResponder Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var responder = new InstaStoryQuestionResponder
                            {
                                HasSharedResponse = SourceObject.HasSharedResponse ?? false,
                                Id = SourceObject.Id,
                                ResponseText = SourceObject.Response,
                                Time = DateTimeHelper.FromUnixTimeSeconds(SourceObject.Ts ?? DateTime.UtcNow.ToUnixTime())
                            };

            if (SourceObject.User != null)
            {
                responder.User = ConvertersFabric.Instance.GetUserShortConverter(SourceObject.User).Convert();
            }

            return responder;
        }
    }
}
