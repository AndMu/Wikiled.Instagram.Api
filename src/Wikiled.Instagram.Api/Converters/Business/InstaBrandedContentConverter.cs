using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Business;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Business;

namespace Wikiled.Instagram.Api.Converters.Business
{
    internal class InstaBrandedContentConverter : IObjectConverter<InstaBrandedContent, InstaBrandedContentResponse>
    {
        public InstaBrandedContentResponse SourceObject { get; set; }

        public InstaBrandedContent Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var brandedContent = new InstaBrandedContent { RequireApproval = SourceObject.RequireApproval };
            if (SourceObject.WhitelistedUsers != null && SourceObject.WhitelistedUsers.Any())
            {
                foreach (var item in SourceObject.WhitelistedUsers)
                {
                    try
                    {
                        brandedContent.WhitelistedUsers.Add(InstaConvertersFabric.Instance.GetUserShortConverter(item)
                                                                .Convert());
                    }
                    catch
                    {
                    }
                }
            }

            return brandedContent;
        }
    }
}