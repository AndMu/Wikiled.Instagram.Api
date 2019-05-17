using System;
using System.Linq;

namespace Wikiled.Instagram.Api.Classes.Android.DeviceInfo
{
    [Serializable]
    public class AndroidVersion
    {
        private static AndroidVersion lastAndriodVersion =
            AndroidVersionList.GetVersionList().AndroidVersions()[
                AndroidVersionList.GetVersionList().AndroidVersions().Count - 2];

        private static readonly Random Rnd = new Random();

        internal AndroidVersion()
        {
        }

        public string ApiLevel { get; set; }

        public string Codename { get; set; }

        public string VersionNumber { get; set; }

        public static AndroidVersion FromString(string versionString)
        {
            var version = new Version(versionString);
            foreach (var androidVersion in AndroidVersionList.GetVersionList().AndroidVersions())
            {
                if (version.CompareTo(new Version(androidVersion.VersionNumber)) == 0 ||
                    version.CompareTo(new Version(androidVersion.VersionNumber)) > 0 &&
                    androidVersion != AndroidVersionList.GetVersionList().AndroidVersions().Last() &&
                    version.CompareTo(
                        new Version(
                            AndroidVersionList.GetVersionList().AndroidVersions()[
                                    AndroidVersionList.GetVersionList().AndroidVersions().IndexOf(androidVersion) + 1]
                                .VersionNumber)) <
                    0)
                {
                    return androidVersion;
                }
            }

            return null;
        }

        public static AndroidVersion GetAndroidVersion(string apiLevel)
        {
            if (string.IsNullOrEmpty(apiLevel))
            {
                return null;
            }

            return AndroidVersionList.GetVersionList()
                .AndroidVersions()
                .FirstOrDefault(api => api.ApiLevel == apiLevel);
        }

        public static AndroidVersion GetRandomAndriodVersion()
        {
            AndroidVersion androidVersion;
            do
            {
                var randomDeviceIndex = Rnd.Next(0, AndroidVersionList.GetVersionList().AndroidVersions().Count);
                androidVersion = AndroidVersionList.GetVersionList().AndroidVersions().ElementAt(randomDeviceIndex);

            } while (lastAndriodVersion != null && androidVersion.ApiLevel == lastAndriodVersion.ApiLevel);

            lastAndriodVersion = androidVersion;
            return androidVersion;
        }
    }
}