namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaMediaStoryUpload
    {
        public double Height { get; set; } = 0.5;

        public bool IsSticker { get; set; } = false;

        /// <summary>
        ///     Get it from <see cref="InstaMedia.Pk" />
        /// </summary>
        public long MediaPk { get; set; }

        public double Rotation { get; set; } = 0.0;

        public double Width { get; set; } = 0.5;

        public double X { get; set; } = 0.5;

        public double Y { get; set; } = 0.499812593703148;
    }
}
