using System;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaUserTagConverter : IObjectConverter<InstaUserTag, InstaUserTagResponse>
    {
        public InstaUserTagResponse SourceObject { get; set; }

        public InstaUserTag Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var userTag = new InstaUserTag();
            if (SourceObject.Position?.Length == 2)
            {
                userTag.Position = new InstaPosition(SourceObject.Position[0], SourceObject.Position[1]);
            }

            userTag.TimeInVideo = SourceObject.TimeInVideo;
            if (SourceObject.User != null)
            {
                userTag.User = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject.User).Convert();
            }

            return userTag;
        }
    }
}