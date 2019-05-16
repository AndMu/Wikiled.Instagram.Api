using System;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class
        InstaBroadcastCommentConverter : IObjectConverter<InstaBroadcastComment, InstaBroadcastCommentResponse>
    {
        public InstaBroadcastCommentResponse SourceObject { get; set; }

        public InstaBroadcastComment Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var broadcastComment = new InstaBroadcastComment
            {
                MediaId = SourceObject.MediaId,
                ContentType = SourceObject.ContentType,
                CreatedAt = (SourceObject.CreatedAt ?? DateTime.Now.ToUnixTime()).FromUnixTimeSeconds(),
                CreatedAtUtc = (SourceObject.CreatedAtUtc ?? DateTime.UtcNow.ToUnixTime()).FromUnixTimeSeconds(),
                Pk = SourceObject.Pk,
                Text = SourceObject.Text,
                Type = SourceObject.Type,
                BitFlags = SourceObject.BitFlags,
                DidReportAsSpam = SourceObject.DidReportAsSpam,
                InlineComposerDisplayCondition = SourceObject.InlineComposerDisplayCondition,
                UserId = SourceObject.UserId
            };
            if (SourceObject.User != null)
            {
                broadcastComment.User = InstaConvertersFabric.Instance
                    .GetUserShortFriendshipFullConverter(SourceObject.User)
                    .Convert();
            }

            return broadcastComment;
        }
    }
}