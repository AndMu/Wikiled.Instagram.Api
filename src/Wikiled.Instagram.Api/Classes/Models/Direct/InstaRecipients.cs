using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaRecipients
    {
        public long ExpiresIn { get; set; }

        public bool Filtered { get; set; }

        public string RankToken { get; set; }

        public string RequestId { get; set; }

        public List<InstaRankedRecipientThread> Threads { get; set; } = new List<InstaRankedRecipientThread>();

        public List<UserShortDescription> Users { get; set; } = new List<UserShortDescription>();
    }
}