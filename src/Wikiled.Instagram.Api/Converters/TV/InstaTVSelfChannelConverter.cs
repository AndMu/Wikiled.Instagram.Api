using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.TV;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.TV;

namespace Wikiled.Instagram.Api.Converters.TV
{
    internal class InstaTvSelfChannelConverter : IObjectConverter<InstaTvSelfChannel, InstaTvSelfChannelResponse>
    {
        public InstaTvSelfChannelResponse SourceObject { get; set; }

        public InstaTvSelfChannel Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var channel = new InstaTvSelfChannel
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

            if (SourceObject.User != null)
            {
                try
                {
                    channel.User = InstaConvertersFabric.Instance.GetTvUserConverter(SourceObject.User).Convert();
                }
                catch
                {
                }
            }

            return channel;
        }
    }
}