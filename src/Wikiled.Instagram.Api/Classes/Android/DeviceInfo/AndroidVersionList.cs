using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Android.DeviceInfo
{
    public class InstaAndroidVersionList
    {
        public static InstaAndroidVersionList GetVersionList()
        {
            return new InstaAndroidVersionList();
        }

        public List<InstaAndroidVersion> AndroidVersions()
        {
            return new List<InstaAndroidVersion>
            {
                new InstaAndroidVersion { Codename = "Ice Cream Sandwich", VersionNumber = "4.0", ApiLevel = "14" },
                new InstaAndroidVersion { Codename = "Ice Cream Sandwich", VersionNumber = "4.0.3", ApiLevel = "15" },
                new InstaAndroidVersion { Codename = "Jelly Bean", VersionNumber = "4.1", ApiLevel = "16" },
                new InstaAndroidVersion { Codename = "Jelly Bean", VersionNumber = "4.2", ApiLevel = "17" },
                new InstaAndroidVersion { Codename = "Jelly Bean", VersionNumber = "4.3", ApiLevel = "18" },
                new InstaAndroidVersion { Codename = "KitKat", VersionNumber = "4.4", ApiLevel = "19" },
                new InstaAndroidVersion { Codename = "KitKat", VersionNumber = "5.0", ApiLevel = "21" },
                new InstaAndroidVersion { Codename = "Lollipop", VersionNumber = "5.1", ApiLevel = "22" },
                new InstaAndroidVersion { Codename = "Marshmallow", VersionNumber = "6.0", ApiLevel = "23" },
                new InstaAndroidVersion { Codename = "Nougat", VersionNumber = "7.0", ApiLevel = "24" },
                new InstaAndroidVersion { Codename = "Nougat", VersionNumber = "7.1", ApiLevel = "25" },
                new InstaAndroidVersion { Codename = "Oreo", VersionNumber = "8.0", ApiLevel = "26" },
                new InstaAndroidVersion { Codename = "Oreo", VersionNumber = "8.1", ApiLevel = "27" },
                new InstaAndroidVersion { Codename = "Pie", VersionNumber = "9.0", ApiLevel = "27" }
            };
        }
    }
}