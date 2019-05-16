using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class InstaBroadcastListConverter : IObjectConverter<InstaBroadcastList, List<InstaBroadcastResponse>>
    {
        public List<InstaBroadcastResponse> SourceObject { get; set; }

        public InstaBroadcastList Convert()
        {
            //if (SourceObject == null) throw new ArgumentNullException($"Source object");
            var broadcastList = new InstaBroadcastList();
            if (SourceObject?.Count > 0)
            {
                foreach (var broadcast in SourceObject)
                {
                    broadcastList.Add(InstaConvertersFabric.Instance.GetBroadcastConverter(broadcast).Convert());
                }
            }

            return broadcastList;
        }
    }
}