using Microsoft.Extensions.Logging;
using System;
using System.Text;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart
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
            var result = new SmartCaption(caption);
            if (caption == null)
            {
                return result;
            }

            int? begin = null;
            var builder = new StringBuilder();
            for (int i = 0; i < caption.Length; i++)
            {
                var current = caption[i];
                if (current == '#')
                {
                    if (begin != null)
                    {
                        result.AddTag(HashTagData.FromTag(caption.Substring(begin.Value, i - begin.Value)));
                    }

                    begin = i;
                    builder.Append(' ');
                }
                else if (!char.IsLetterOrDigit(current) && begin != null)
                {
                    result.AddTag(HashTagData.FromTag(caption.Substring(begin.Value, i - begin.Value)));
                    begin = null;
                }

                if (begin == null)
                {
                    builder.Append(current);
                }
            }

            if (begin != null)
            {
                result.AddTag(HashTagData.FromTag(caption.Substring(begin.Value, caption.Length - begin.Value)));
            }

            result.WithoutTags = builder.ToString().Trim();
            return result;
        }

    }
}
