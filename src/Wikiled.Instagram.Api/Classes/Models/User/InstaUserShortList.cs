using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaUserShortList : List<InstaUserShort>, IInstaBaseList
    {
        public string NextMaxId { get; set; }
    }
}