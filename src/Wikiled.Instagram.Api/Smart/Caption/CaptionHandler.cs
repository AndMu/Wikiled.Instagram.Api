using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart.Caption
{
    public class CaptionHandler : ICaptionHandler
    {
        private readonly ILogger<CaptionHandler> log;

        public CaptionHandler(ILogger<CaptionHandler> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public SmartCaption Extract(string caption)
        {
            if (caption == null)
            {
                return new SmartCaption(caption, new HashTagData[]{});
            }

            var table = new Dictionary<string, HashTagData>(StringComparer.OrdinalIgnoreCase);
            int? begin = null;
            for (int i = 0; i < caption.Length; i++)
            {
                var current = caption[i];
                if (current == '#')
                {
                    if (begin != null)
                    {
                        var tag = HashTagData.FromTag(caption.Substring(begin.Value, i - begin.Value));
                        table[tag.Tag] = tag;
                    }

                    begin = i;
                }
                else if (!char.IsLetterOrDigit(current) && current != '_' && begin != null)
                {
                    var tag = HashTagData.FromTag(caption.Substring(begin.Value, i - begin.Value));
                    table[tag.Tag] = tag;
                    begin = null;
                }
            }

            if (begin != null)
            {
                var tag = HashTagData.FromTag(caption.Substring(begin.Value, caption.Length - begin.Value));
                table[tag.Tag] = tag;
            }

            return new SmartCaption(caption, table.Values.ToArray());
        }
    }
}
