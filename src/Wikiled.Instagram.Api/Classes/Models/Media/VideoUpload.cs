using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Media
{
    public class VideoUpload
    {
        public VideoUpload()
        {
        }

        public VideoUpload(InstaVideo video, InstaImage videoThumbnail)
        {
            Video = video;
            VideoThumbnail = videoThumbnail;
        }

        /// <summary>
        ///     User tags => Optional
        /// </summary>
        public List<UserTagVideoUpload> UserTags { get; set; } = new List<UserTagVideoUpload>();

        public InstaVideo Video { get; set; }

        public InstaImage VideoThumbnail { get; set; }
    }
}