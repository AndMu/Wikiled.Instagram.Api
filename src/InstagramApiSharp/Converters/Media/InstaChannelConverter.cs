using System;

namespace Wikiled.Instagram.Api.Converters.Media
{
    internal class InstaChannelConverter : IObjectConverter<InstaChannel, InstaChannelResponse>
    {
        public InstaChannelResponse SourceObject { get; set; }

        public InstaChannel Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var channel = new InstaChannel
                          {
                              ChannelId = SourceObject.ChannelId,
                              ChannelType = SourceObject.ChannelType,
                              Context = SourceObject.Context,
                              Header = SourceObject.Header,
                              Title = SourceObject.Title
                          };
            if (SourceObject.Media != null)
            {
                channel.Media = ConvertersFabric.Instance.GetSingleMediaConverter(SourceObject.Media).Convert();
            }

            return channel;
        }
    }
}
