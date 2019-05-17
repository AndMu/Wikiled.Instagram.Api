using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Media
{
    public class InstaImageUpload
    {
        /// <summary>
        ///     Create an instance of <see cref="InstaImageUpload" />
        /// </summary>
        public InstaImageUpload()
        {
        }

        /// <summary>
        ///     Create an instance of <see cref="InstaImageUpload" />
        /// </summary>
        /// <param name="uri">Image uri</param>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        public InstaImageUpload(string uri, int width = 0, int height = 0)
        {
            Uri = uri;
            Width = width;
            Height = height;
        }

        /// <summary>
        ///     Image height => Optional
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        ///     Image bytes => Optional (if using <see cref="Uri" />)
        /// </summary>
        public byte[] ImageBytes { get; set; }

        /// <summary>
        ///     Image uri => Optional (if using <see cref="ImageBytes" />)
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        ///     User tags => Optional
        /// </summary>
        public List<UserTagUpload> UserTags { get; set; } = new List<UserTagUpload>();

        /// <summary>
        ///     Image width => Optional
        /// </summary>
        public int Width { get; set; }
    }
}