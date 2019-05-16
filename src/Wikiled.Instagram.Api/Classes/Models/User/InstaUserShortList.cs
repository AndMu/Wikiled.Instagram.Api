using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaUserShortList : List<InstaUserShort>, IInstaBaseList
    {
        public string NextMaxId { get; set; }
    }
}
