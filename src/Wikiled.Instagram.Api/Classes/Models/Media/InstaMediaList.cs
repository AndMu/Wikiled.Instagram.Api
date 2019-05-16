using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.Models.Media
{
    public class InstaMediaList : List<InstaMedia>, IInstaBaseList
    {
        public int Pages { get; set; } = 0;

        public int PageSize { get; set; } = 0;

        public string NextMaxId { get; set; }
    }
}