using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaUserChainingList : List<InstaUserChaining>
    {
        public bool IsBackup { get; set; }

        internal string Status { get; set; }
    }
}