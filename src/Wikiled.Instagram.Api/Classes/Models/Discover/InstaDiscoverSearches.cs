using System;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Discover
{
    public class InstaDiscoverSearches
    {
        public DateTime ClientTime { get; set; }

        public int Position { get; set; }

        public InstaUser User { get; set; }
    }
}