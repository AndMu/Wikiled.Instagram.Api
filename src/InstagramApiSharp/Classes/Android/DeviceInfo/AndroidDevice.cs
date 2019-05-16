using System;

namespace Wikiled.Instagram.Api.Classes.Android.DeviceInfo
{
    [Serializable]
    public class AndroidDevice
    {
        public Guid AdId { get; set; } = Guid.NewGuid();

        public string AndroidBoardName { get; set; }

        public string AndroidBootloader { get; set; }

        public AndroidVersion AndroidVer { get; set; } = AndroidVersion.GetRandomAndriodVersion();

        public string DeviceBrand { get; set; }

        public Guid DeviceGuid { get; set; }

        public string DeviceId { get; set; }

        public string DeviceModel { get; set; }

        public string DeviceModelBoot { get; set; }

        public string DeviceModelIdentifier { get; set; }

        public string Dpi { get; set; } = "480dpi";

        public string FirmwareBrand { get; set; }

        public string FirmwareFingerprint { get; set; }

        public string FirmwareTags { get; set; }

        public string FirmwareType { get; set; }

        public Guid GoogleAdId { get; set; } = Guid.NewGuid();

        public string HardwareManufacturer { get; set; }

        public string HardwareModel { get; set; }

        public Guid PhoneGuid { get; set; }

        public Guid RankToken { get; set; } = Guid.NewGuid();

        public string Resolution { get; set; } = "1080x1812";
    }
}
