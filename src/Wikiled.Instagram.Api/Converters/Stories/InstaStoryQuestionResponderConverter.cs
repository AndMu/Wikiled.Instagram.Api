using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class
        InstaStoryQuestionResponderConverter : IObjectConverter<InstaStoryQuestionResponder,
            InstaStoryQuestionResponderResponse>
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
                Time = (SourceObject.Ts ?? DateTime.UtcNow.ToUnixTime()).FromUnixTimeSeconds()
            };

            if (SourceObject.User != null)
            {
                responder.User = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject.User).Convert();
            }

            return responder;
        }
    }
}