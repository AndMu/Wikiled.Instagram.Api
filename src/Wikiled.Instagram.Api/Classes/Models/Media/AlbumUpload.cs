namespace Wikiled.Instagram.Api.Classes.Models.Media
{
    /// <summary>
    ///     Only one of these property property!
    /// </summary>
    public class AlbumUpload
    {
        /// <summary>
        ///     If you set <see cref="ImageToUpload" />, don't set <see cref="VideoToUpload" />
        /// </summary>
        public ImageUpload ImageToUpload { get; set; }

        /// <summary>
        ///     If you set <see cref="VideoToUpload" />, don't set <see cref="ImageToUpload" />
        /// </summary>
        public VideoUpload VideoToUpload { get; set; }

        internal bool IsBoth => ImageToUpload != null && VideoToUpload != null;

        internal bool IsImage => ImageToUpload != null;

        internal bool IsVideo => VideoToUpload != null;
    }
}