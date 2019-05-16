using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Media
{
    public class InstaVideoUpload
    {
        public InstaVideoUpload()
        {
        }

        public InstaVideoUpload(InstaVideo video, InstaImage videoThumbnail)
        {
            Video = video;
            VideoThumbnail = videoThumbnail;
        }

        /// <summary>
        ///     User tags => Optional
        /// </summary>
        public List<InstaUserTagVideoUpload> UserTags { get; set; } = new List<InstaUserTagVideoUpload>();

        public InstaVideo Video { get; set; }

        public InstaImage VideoThumbnail { get; set; }
    }
}
