using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaBrandedContent
    {
        public bool RequireApproval { get; set; }

        public List<InstaUserShort> WhitelistedUsers { get; set; } = new List<InstaUserShort>();
    }
}