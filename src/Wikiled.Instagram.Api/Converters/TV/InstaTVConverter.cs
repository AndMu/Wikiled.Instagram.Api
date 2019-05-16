using System;

namespace Wikiled.Instagram.Api.Converters.TV
{
    internal class InstaTVConverter : IObjectConverter<InstaTV, InstaTVResponse>
    {
        public InstaTVResponse SourceObject { get; set; }

        public InstaTV Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var tv = new InstaTV
                     {
                         Status = SourceObject.Status
                     };
            if (SourceObject.MyChannel != null)
            {
                try
                {
                    tv.MyChannel = ConvertersFabric.Instance.GetTVSelfChannelConverter(SourceObject.MyChannel).Convert();
                }
                catch
                {
                }
            }

            if (SourceObject.Channels != null && SourceObject.Channels.Any())
            {
                foreach (var channel in SourceObject.Channels)
                {
                    try
                    {
                        tv.Channels.Add(ConvertersFabric.Instance.GetTVChannelConverter(channel).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            return tv;
        }
    }
}
