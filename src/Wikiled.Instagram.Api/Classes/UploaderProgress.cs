using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes
{
    public class UploaderProgress
    {
        public string Caption { get; internal set; }

        public string Name { get; internal set; } = "Uploading single file";

        //public long FileSize { get; internal set; }
        //public long UploadedBytes { get; internal set; }
        public string UploadId { get; internal set; }

        public InstaUploadState UploadState { get; internal set; }
    }
}