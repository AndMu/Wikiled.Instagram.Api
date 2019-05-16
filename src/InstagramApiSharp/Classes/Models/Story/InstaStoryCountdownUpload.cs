using System;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryCountdownUpload
    {
        public string DigitCardColor { get; set; } = "#42dcf4";

        public string DigitColor { get; set; } = "#4286f4";

        public string EndBackgroundColor { get; set; } = "#ffffff";

        public DateTime EndTime { get; set; } = DateTime.UtcNow.AddDays(1);

        public bool FollowingEnabled { get; set; } = true;

        public double Height { get; set; } = 0.21962096;

        public bool IsSticker { get; set; } = false;

        public double Rotation { get; set; } = 0.0;

        public string StartBackgroundColor { get; set; } = "#ffffff";

        public string Text { get; set; }

        public string TextColor { get; set; } = "#000000";

        public double Width { get; set; } = 0.7972222;

        public double X { get; set; } = 0.5;

        public double Y { get; set; } = 0.5;

        public double Z { get; set; } = 0;
    }
}
