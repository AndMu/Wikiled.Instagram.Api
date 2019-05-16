using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaBlockedUsersConverter : IObjectConverter<InstaBlockedUsers, InstaBlockedUsersResponse>
    {
        public InstaBlockedUsersResponse SourceObject { get; set; }

        public InstaBlockedUsers Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var blockedUsers = new InstaBlockedUsers { MaxId = SourceObject.MaxId };

            if (SourceObject.BlockedList != null && SourceObject.BlockedList.Any())
            {
                foreach (var user in SourceObject.BlockedList)
                {
                    blockedUsers.BlockedList.Add(InstaConvertersFabric.Instance.GetBlockedUserInfoConverter(user).Convert());
                }
            }

            return blockedUsers;
        }
    }
}