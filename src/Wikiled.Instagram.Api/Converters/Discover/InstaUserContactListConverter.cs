using System;
using Wikiled.Instagram.Api.Classes.Models.Discover;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Discover
{
    internal class InstaUserContactListConverter : IObjectConverter<InstaContactUserList, InstaContactUserListResponse>
    {
        public InstaContactUserListResponse SourceObject { get; set; }

        public InstaContactUserList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var userList = new InstaContactUserList();
            try
            {
                foreach (var item in SourceObject.Items)
                {
                    try
                    {
                        userList.Add(InstaConvertersFabric.Instance.GetSingleUserContactConverter(item.User).Convert());
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            return userList;
        }
    }
}