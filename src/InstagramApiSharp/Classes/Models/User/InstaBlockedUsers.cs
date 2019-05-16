using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaBlockedUsers : InstaDefault
    {
        public List<InstaBlockedUserInfo> BlockedList { get; set; } = new List<InstaBlockedUserInfo>();

        public string MaxId { get; set; }

        public int? PageSize { get; set; }
    }
}
