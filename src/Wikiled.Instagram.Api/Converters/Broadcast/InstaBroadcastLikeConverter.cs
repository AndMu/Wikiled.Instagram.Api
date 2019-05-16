using System;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;
using Wikiled.Instagram.Api.Helpers;

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
                LikeTime = (SourceObject.LikeTs ?? DateTime.Now.ToUnixTime()).FromUnixTimeSeconds()
            };
            return broadcastLike;
        }
    }
}