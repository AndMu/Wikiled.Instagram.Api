namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaAnimatedImage
    {
        public string Id { get; set; }

        public bool IsRandom { get; set; }

        public bool IsSticker { get; set; }

        public InstaAnimatedImageMedia Media { get; set; }

        public InstaAnimatedImageUser User { get; set; }
    }
}