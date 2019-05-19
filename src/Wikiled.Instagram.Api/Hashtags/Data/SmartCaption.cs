using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Common.Extensions;

namespace Wikiled.Instagram.Api.Hashtags.Data
{
    public class SmartCaption
    {
        private HashSet<string> tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public SmartCaption(string original)
        {
            Original = original;
        }

        public string Original { get; }

        public string WithoutTags { get; set; }

        public IEnumerable<string> Tags => tags;

        public string Generate()
        {
            var tagsText = Tags.Select(item => "#" + item.ToLower()).AccumulateItems(" ");
            if (WithoutTags.Length == 0)
            {
                return tagsText;
            }

            return $"{WithoutTags} {tagsText}";
        }

        public void AddTags(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
            {
                AddTag(tag);
            }
        }

        public void AddTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }

            tags.Add(tag.StartsWith("#") ? tag.Substring(1) : tag);
        }
    }
}
