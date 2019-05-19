using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Wikiled.Common.Extensions;
using Wikiled.Instagram.Api.Hashtags.Data;

namespace Wikiled.Instagram.Api.Hashtags
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

            var captionTags = caption.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            log.LogInformation("Adding [{0}] caption tags", captionTags.Length);
            var words = new List<string>();
            foreach (var tag in captionTags)
            {
                if (tag.StartsWith("#"))
                {
                    var tagWithoutImages = Regex.Replace(tag, @"[^\u0020-\u007E]", string.Empty);
                    result.AddTag(tagWithoutImages);
                }
                else
                {
                    words.Add(tag);
                }
            }

            result.WithoutTags = words.AccumulateItems(" ");
            return result;
        }

    }
}
