using System;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class InstaBroadcastLikeConverter : IObjectConverter<InstaBroadcastLike, InstaBroadcastLikeResponse>
    {
        public InstaBroadcastLikeResponse SourceObject { get; set; }

        public InstaBroadcastLike Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var broadcastLike = new InstaBroadcastLike
                                {
                                    BurstLikes = SourceObject.BurstLikes,
                                    Likes = SourceObject.Likes,
                                    LikeTime = DateTimeHelper.FromUnixTimeSeconds(SourceObject.LikeTs ?? DateTime.Now.ToUnixTime())
                                };
            return broadcastLike;
        }
    }
}
