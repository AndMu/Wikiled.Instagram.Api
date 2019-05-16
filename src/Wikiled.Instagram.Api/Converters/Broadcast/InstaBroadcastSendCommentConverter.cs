using System;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class
        InstaBroadcastSendCommentConverter : IObjectConverter<InstaBroadcastSendComment,
            InstaBroadcastSendCommentResponse>
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
                CreatedAt = (SourceObject.CreatedAt ?? DateTime.Now.ToUnixTime()).FromUnixTimeSeconds(),
                CreatedAtUtc = (SourceObject.CreatedAtUtc ?? DateTime.UtcNow.ToUnixTime()).FromUnixTimeSeconds(),
                Pk = SourceObject.Pk,
                Text = SourceObject.Text,
                Type = SourceObject.Type
            };
            if (SourceObject.User != null)
            {
                broadcastSendComment.User = InstaConvertersFabric.Instance
                    .GetUserShortFriendshipFullConverter(SourceObject.User)
                    .Convert();
            }

            return broadcastSendComment;
        }
    }
}