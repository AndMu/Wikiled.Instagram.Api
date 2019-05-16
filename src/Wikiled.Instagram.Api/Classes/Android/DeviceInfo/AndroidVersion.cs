using System;
using System.Linq;

namespace Wikiled.Instagram.Api.Classes.Android.DeviceInfo
{
    [Serializable]
    public class InstaAndroidVersion
    {
        private static InstaAndroidVersion lastAndriodVersion =
            InstaAndroidVersionList.GetVersionList().AndroidVersions()[
                InstaAndroidVersionList.GetVersionList().AndroidVersions().Count - 2];

        private static readonly Random Rnd = new Random();

        internal InstaAndroidVersion()
        {
        }

        public string ApiLevel { get; set; }

        public string Codename { get; set; }

        public string VersionNumber { get; set; }

        public static InstaAndroidVersion FromString(string versionString)
        {
            var version = new Version(versionString);
            foreach (var androidVersion in InstaAndroidVersionList.GetVersionList().AndroidVersions())
            {
                if (version.CompareTo(new Version(androidVersion.VersionNumber)) == 0 ||
                    version.CompareTo(new Version(androidVersion.VersionNumber)) > 0 &&
                    androidVersion != InstaAndroidVersionList.GetVersionList().AndroidVersions().Last() &&
                    version.CompareTo(
                        new Version(
                            InstaAndroidVersionList.GetVersionList().AndroidVersions()[
                                    InstaAndroidVersionList.GetVersionList().AndroidVersions().IndexOf(androidVersion) + 1]
                                .VersionNumber)) <
                    0)
                {
                    return androidVersion;
                }
            }

            return null;
        }

        public static InstaAndroidVersion GetAndroidVersion(string apiLevel)
        {
            if (string.IsNullOrEmpty(apiLevel))
            {
                return null;
            }

            return InstaAndroidVersionList.GetVersionList()
                .AndroidVersions()
                .FirstOrDefault(api => api.ApiLevel == apiLevel);
        }

        public static InstaAndroidVersion GetRandomAndriodVersion()
        {
            TryLabel:
            var randomDeviceIndex = Rnd.Next(0, InstaAndroidVersionList.GetVersionList().AndroidVersions().Count);
            var androidVersion = InstaAndroidVersionList.GetVersionList().AndroidVersions().ElementAt(randomDeviceIndex);
            if (lastAndriodVersion != null)
            {
                if (androidVersion.ApiLevel == lastAndriodVersion.ApiLevel)
                {
                    goto TryLabel;
                }
            }

            lastAndriodVersion = androidVersion;
            return androidVersion;
        }
    }
}