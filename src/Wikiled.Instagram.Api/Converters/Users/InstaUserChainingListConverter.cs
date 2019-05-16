using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class
        InstaUserChainingListConverter : IObjectConverter<InstaUserChainingList, InstaUserChainingContainerResponse>
    {
        public InstaUserChainingContainerResponse SourceObject { get; set; }

        public InstaUserChainingList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var users = new InstaUserChainingList { Status = SourceObject.Status, IsBackup = SourceObject.IsBackup };
            if (SourceObject.Users != null && SourceObject.Users.Any())
            {
                foreach (var u in SourceObject.Users)
                {
                    try
                    {
                        users.Add(InstaConvertersFabric.Instance.GetSingleUserChainingConverter(u).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            return users;
        }
    }
}