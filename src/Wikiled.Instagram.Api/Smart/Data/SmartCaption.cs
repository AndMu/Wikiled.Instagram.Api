using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Common.Extensions;

namespace Wikiled.Instagram.Api.Smart.Data
{
    public class SmartCaption
    {
        private Dictionary<string, HashTagData> tags = new Dictionary<string, HashTagData>(StringComparer.OrdinalIgnoreCase);

        public SmartCaption(string original)
        {
            Original = original;
        }

        public string Original { get; }

        public string WithoutTags { get; set; }

        public IEnumerable<HashTagData> Tags => tags.Values;

        public int TotalTags => tags.Count;

        public string Generate()
        {
            var tagsText = Tags.Select(item => item.Tag).AccumulateItems(" ");
            if (WithoutTags.Length == 0)
            {
                return tagsText;
            }

            return $"{WithoutTags} {tagsText}";
        }

        public void AddTags(IEnumerable<HashTagData> tags)
        {
            foreach (var tag in tags)
            {
                AddTag(tag);
            }
        }

        public void AddTag(HashTagData tag)
        {
            if (!tags.ContainsKey(tag.Tag))
            {
                tags.Add(tag.Tag, tag);
            }
        }
    }
}
