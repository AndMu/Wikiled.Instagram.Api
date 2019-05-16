using System;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class InstaBroadcastSendCommentConverter : IObjectConverter<InstaBroadcastSendComment, InstaBroadcastSendCommentResponse>
    {
        public InstaBroadcastSendCommentResponse SourceObject { get; set; }

        public InstaBroadcastSendComment Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var broadcastSendComment = new InstaBroadcastSendComment
                                       {
                                           MediaId = SourceObject.MediaId,
                                           ContentType = SourceObject.ContentType,
                                           CreatedAt = DateTimeHelper.FromUnixTimeSeconds(SourceObject.CreatedAt ?? DateTime.Now.ToUnixTime()),
                                           CreatedAtUtc = DateTimeHelper.FromUnixTimeSeconds(SourceObject.CreatedAtUtc ?? DateTime.UtcNow.ToUnixTime()),
                                           Pk = SourceObject.Pk,
                                           Text = SourceObject.Text,
                                           Type = SourceObject.Type
                                       };
            if (SourceObject.User != null)
            {
                broadcastSendComment.User = ConvertersFabric.Instance
                                                            .GetUserShortFriendshipFullConverter(SourceObject.User).Convert();
            }

            return broadcastSendComment;
        }
    }
}
