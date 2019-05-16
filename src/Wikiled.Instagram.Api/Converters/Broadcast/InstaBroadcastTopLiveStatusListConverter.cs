using System;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class InstaBroadcastTopLiveStatusListConverter : IObjectConverter<InstaBroadcastTopLiveStatusList,
        InstaBroadcastTopLiveStatusResponse>
    {
        public InstaBroadcastTopLiveStatusResponse SourceObject { get; set; }

        public InstaBroadcastTopLiveStatusList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var broadcastStatusItems = new InstaBroadcastTopLiveStatusList();
            try
            {
                if (SourceObject.BroadcastStatusItems?.Count > 0)
                {
                    foreach (var statusItem in SourceObject.BroadcastStatusItems)
                    {
                        broadcastStatusItems.Add(InstaConvertersFabric.Instance.GetBroadcastStatusItemConverter(statusItem)
                                                     .Convert());
                    }
                }
            }
            catch
            {
            }

            return broadcastStatusItems;
        }
    }
}