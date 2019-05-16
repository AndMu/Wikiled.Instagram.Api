namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryLocationUpload
    {
        public double Height { get; set; } = 0.08751394;

        public bool IsSticker { get; set; } = false;

        /// <summary>
        ///     Location id (get it from <seealso cref="ILocationProcessor.SearchLocationAsync" /> )
        /// </summary>
        public string LocationId { get; set; }

        public double Rotation { get; set; } = 0.0;

        public double Width { get; set; } = 0.7416667;

        public double X { get; set; } = 0.5;

        public double Y { get; set; } = 0.5;

        public double Z { get; set; } = 0;
    }
}
