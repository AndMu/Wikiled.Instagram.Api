using System;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment;

namespace Wikiled.Instagram.Api.Converters.Other
{
    internal class
        InstaBlockedCommentersConverter : IObjectConverter<InstaUserShortList, InstaBlockedCommentersResponse>
    {
        public InstaBlockedCommentersResponse SourceObject { get; set; }

        public InstaUserShortList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var users = new InstaUserShortList();

            if (SourceObject.BlockedCommenters?.Count > 0)
            {
                foreach (var user in SourceObject.BlockedCommenters)
                {
                    users.Add(InstaConvertersFabric.Instance.GetUserShortConverter(user).Convert());
                }
            }

            return users;
        }
    }
}