﻿using System;

namespace Wikiled.Instagram.Api.Converters.Business
{
    internal class InstaInsightsDataNodeConverter : IObjectConverter<InstaInsightsDataNode, InstaInsightsDataNodeResponse>
    {
        public InstaInsightsDataNodeResponse SourceObject { get; set; }

        public InstaInsightsDataNode Convert()
        {
            var dataNode = new InstaInsightsDataNode
                           {
                               Value = SourceObject.Value ?? 0
                           };
            try
            {
                var truncatedType = SourceObject.Name.Trim().Replace("_", "");

                if (Enum.TryParse(truncatedType, true, out InstaInsightsNameType type))
                {
                    dataNode.NameType = type;
                }
            }
            catch
            {
            }

            return dataNode;
        }
    }
}
