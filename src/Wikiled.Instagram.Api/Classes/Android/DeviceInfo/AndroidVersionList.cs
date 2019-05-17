using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Android.DeviceInfo
{
    public class AndroidVersionList
    {
        public static AndroidVersionList GetVersionList()
        {
            return new AndroidVersionList();
        }

        public List<AndroidVersion> AndroidVersions()
        {
            return new List<AndroidVersion>
            {
                new AndroidVersion { Codename = "Ice Cream Sandwich", VersionNumber = "4.0", ApiLevel = "14" },
                new AndroidVersion { Codename = "Ice Cream Sandwich", VersionNumber = "4.0.3", ApiLevel = "15" },
                new AndroidVersion { Codename = "Jelly Bean", VersionNumber = "4.1", ApiLevel = "16" },
                new AndroidVersion { Codename = "Jelly Bean", VersionNumber = "4.2", ApiLevel = "17" },
                new AndroidVersion { Codename = "Jelly Bean", VersionNumber = "4.3", ApiLevel = "18" },
                new AndroidVersion { Codename = "KitKat", VersionNumber = "4.4", ApiLevel = "19" },
                new AndroidVersion { Codename = "KitKat", VersionNumber = "5.0", ApiLevel = "21" },
                new AndroidVersion { Codename = "Lollipop", VersionNumber = "5.1", ApiLevel = "22" },
                new AndroidVersion { Codename = "Marshmallow", VersionNumber = "6.0", ApiLevel = "23" },
                new AndroidVersion { Codename = "Nougat", VersionNumber = "7.0", ApiLevel = "24" },
                new AndroidVersion { Codename = "Nougat", VersionNumber = "7.1", ApiLevel = "25" },
                new AndroidVersion { Codename = "Oreo", VersionNumber = "8.0", ApiLevel = "26" },
                new AndroidVersion { Codename = "Oreo", VersionNumber = "8.1", ApiLevel = "27" },
                new AndroidVersion { Codename = "Pie", VersionNumber = "9.0", ApiLevel = "27" }
            };
        }
    }
}