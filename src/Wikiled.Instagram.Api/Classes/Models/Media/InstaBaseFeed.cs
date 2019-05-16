using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.Models.Media
{
    public class InstaBaseFeed : IInstaBaseList
    {
        public InstaMediaList Medias { get; set; } = new InstaMediaList();

        public string NextMaxId { get; set; }
    }
}