using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Media
{
    internal class InstaCaptionConverter : IObjectConverter<InstaCaption, InstaCaptionResponse>
    {
        public InstaCaptionResponse SourceObject { get; set; }

        public InstaCaption Convert()
        {
            var caption = new InstaCaption
            {
                Pk = SourceObject.Pk,
                CreatedAt = InstaDateTimeHelper.UnixTimestampToDateTime(SourceObject.CreatedAtUnixLike),
                CreatedAtUtc = InstaDateTimeHelper.UnixTimestampToDateTime(SourceObject.CreatedAtUtcUnixLike),
                MediaId = SourceObject.MediaId,
                Text = SourceObject.Text,
                User = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject.User).Convert(),
                UserId = SourceObject.UserId
            };
            return caption;
        }
    }
}