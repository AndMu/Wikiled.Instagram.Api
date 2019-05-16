using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Media
{
    public class InstaMediaList : List<InstaMedia>, IInstaBaseList
    {
        public string NextMaxId { get; set; }

        public int Pages { get; set; } = 0;

        public int PageSize { get; set; } = 0;
    }
}
