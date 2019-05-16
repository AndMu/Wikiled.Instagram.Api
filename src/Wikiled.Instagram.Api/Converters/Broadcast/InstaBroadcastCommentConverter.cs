using System;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class InstaBroadcastCommentConverter : IObjectConverter<InstaBroadcastComment, InstaBroadcastCommentResponse>
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
                                       CreatedAt = DateTimeHelper.FromUnixTimeSeconds(SourceObject.CreatedAt ?? DateTime.Now.ToUnixTime()),
                                       CreatedAtUtc = DateTimeHelper.FromUnixTimeSeconds(SourceObject.CreatedAtUtc ?? DateTime.UtcNow.ToUnixTime()),
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
                broadcastComment.User = ConvertersFabric.Instance
                                                        .GetUserShortFriendshipFullConverter(SourceObject.User).Convert();
            }

            return broadcastComment;
        }
    }
}
