using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.TV;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.TV;

namespace Wikiled.Instagram.Api.Converters.TV
{
    internal class InstaTvChannelConverter : IObjectConverter<InstaTvChannel, InstaTvChannelResponse>
    {
        public InstaTvChannelResponse SourceObject { get; set; }

        public InstaTvChannel Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var channel = new InstaTvChannel
            {
                HasMoreAvailable = SourceObject.HasMoreAvailable,
                Id = SourceObject.Id,
                MaxId = SourceObject.MaxId,
                Title = SourceObject.Title,
                Type = SourceObject.Type
            };

            if (SourceObject.Items != null && SourceObject.Items.Any())
            {
                foreach (var item in SourceObject.Items)
                {
                    try
                    {
                        channel.Items.Add(InstaConvertersFabric.Instance.GetSingleMediaConverter(item).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            if (SourceObject.UserDetail != null)
            {
                try
                {
                    channel.UserDetail =
                        InstaConvertersFabric.Instance.GetTvUserConverter(SourceObject.UserDetail).Convert();
                }
                catch
                {
                }
            }

            return channel;
        }
    }
}