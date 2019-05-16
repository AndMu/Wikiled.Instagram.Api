﻿using System;

namespace Wikiled.Instagram.Api.Classes.Android.DeviceInfo
{
    [Serializable]
    public class AndroidVersion
    {
        private static AndroidVersion LastAndriodVersion = AndroidVersionList.GetVersionList().AndroidVersions()[AndroidVersionList.GetVersionList().AndroidVersions().Count - 2];

        private static readonly Random Rnd = new Random();

        internal AndroidVersion()
        {
        }

        public string APILevel { get; set; }

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
                            AndroidVersionList.GetVersionList().AndroidVersions()[AndroidVersionList.GetVersionList().AndroidVersions().IndexOf(androidVersion) + 1]
                                              .VersionNumber)) < 0)
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

            return AndroidVersionList.GetVersionList().AndroidVersions().FirstOrDefault(api => api.APILevel == apiLevel);
        }

        public static AndroidVersion GetRandomAndriodVersion()
        {
            TryLabel:
            var randomDeviceIndex = Rnd.Next(0, AndroidVersionList.GetVersionList().AndroidVersions().Count);
            var androidVersion = AndroidVersionList.GetVersionList().AndroidVersions().ElementAt(randomDeviceIndex);
            if (LastAndriodVersion != null)
            {
                if (androidVersion.APILevel == LastAndriodVersion.APILevel)
                {
                    goto TryLabel;
                }
            }

            LastAndriodVersion = androidVersion;
            return androidVersion;
        }
    }
}
