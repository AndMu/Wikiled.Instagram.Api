using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaRankedRecipientThread
    {
        public bool Canonical { get; set; }

        public bool Named { get; set; }

        public bool Pending { get; set; }

        public string ThreadId { get; set; }

        public string ThreadTitle { get; set; }

        public string ThreadType { get; set; }

        public List<InstaUserShort> Users { get; set; } = new List<InstaUserShort>();

        public long ViewerId { get; set; }
    }
}