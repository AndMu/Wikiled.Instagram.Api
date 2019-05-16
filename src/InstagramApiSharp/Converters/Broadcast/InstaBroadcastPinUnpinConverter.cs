using System;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class InstaBroadcastPinUnpinConverter : IObjectConverter<InstaBroadcastPinUnpin, InstaBroadcastPinUnpinResponse>
    {
        public InstaBroadcastPinUnpinResponse SourceObject { get; set; }

        public InstaBroadcastPinUnpin Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var broadcastPinUnpin = new InstaBroadcastPinUnpin
                                    {
                                        CommentId = SourceObject.CommentId
                                    };

            return broadcastPinUnpin;
        }
    }
}
