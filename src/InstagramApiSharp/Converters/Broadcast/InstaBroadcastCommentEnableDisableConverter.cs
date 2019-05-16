using System;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class InstaBroadcastCommentEnableDisableConverter : IObjectConverter<InstaBroadcastCommentEnableDisable, InstaBroadcastCommentEnableDisableResponse>
    {
        public InstaBroadcastCommentEnableDisableResponse SourceObject { get; set; }

        public InstaBroadcastCommentEnableDisable Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var enDis = new InstaBroadcastCommentEnableDisable
                        {
                            CommentMuted = SourceObject.CommentMuted
                        };
            return enDis;
        }
    }
}
