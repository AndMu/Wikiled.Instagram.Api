using System;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaUserPresence
    {
        public bool IsActive { get; set; }

        public DateTime LastActivity { get; set; }

        public long Pk { get; set; }
    }
}
