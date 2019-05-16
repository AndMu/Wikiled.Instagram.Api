using Wikiled.Instagram.Api.Classes.Models.Media;

namespace Wikiled.Instagram.Api.Classes.Models.Highlight
{
    public class InstaHighlightCoverMedia
    {
        public InstaImage CroppedImage { get; set; }

        public float[] CropRect { get; set; }

        public InstaImage Image { get; set; }

        public string MediaId { get; set; }
    }
}