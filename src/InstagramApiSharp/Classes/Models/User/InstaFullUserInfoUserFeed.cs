using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaFullUserInfoUserFeed
    {
        public bool AutoLoadMoreEnabled { get; set; }

        public List<InstaMedia> Items { get; set; } = new List<InstaMedia>();

        public bool MoreAvailable { get; set; }

        public string NextMaxId { get; set; }

        public string NextMinId { get; set; }

        public int NumResults { get; set; }
    }
}
