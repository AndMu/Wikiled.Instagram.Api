using System;

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

            var blockedUsers = new InstaBlockedUsers
                               {
                                   MaxId = SourceObject.MaxId
                               };

            if (SourceObject.BlockedList != null && SourceObject.BlockedList.Any())
            {
                foreach (var user in SourceObject.BlockedList)
                {
                    blockedUsers.BlockedList.Add(ConvertersFabric.Instance.GetBlockedUserInfoConverter(user).Convert());
                }
            }

            return blockedUsers;
        }
    }
}
