using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.TV;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.TV;

namespace Wikiled.Instagram.Api.Converters.TV
{
    internal class InstaTvConverter : IObjectConverter<InstaTv, InstaTvResponse>
    {
        public InstaTvResponse SourceObject { get; set; }

        public InstaTv Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var tv = new InstaTv { Status = SourceObject.Status };
            if (SourceObject.MyChannel != null)
            {
                try
                {
                    tv.MyChannel = InstaConvertersFabric.Instance.GetTvSelfChannelConverter(SourceObject.MyChannel)
                        .Convert();
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
                        tv.Channels.Add(InstaConvertersFabric.Instance.GetTvChannelConverter(channel).Convert());
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