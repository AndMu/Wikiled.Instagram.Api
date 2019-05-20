using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Common.Extensions;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart.Caption
{
    public class SmartCaption
    {
        private readonly Dictionary<string, (HashTagData, bool)> tagsTable =
            new Dictionary<string, (HashTagData, bool)>(StringComparer.OrdinalIgnoreCase);

        public SmartCaption(string original, HashTagData[] originalTags)
        {
            Original = original;
            foreach (var originalTag in originalTags)
            {
                tagsTable.Add(originalTag.Tag, (originalTag, true));
            }
        }

        public string Original { get; }

        public IEnumerable<HashTagData> Tags => tagsTable.Values.Select(item => item.Item1);

        public int TotalTags => tagsTable.Count;

        public string Generate()
        {
            var tagsText = tagsTable.Where(item => !item.Value.Item2)
                .Select(item => item.Value.Item1.Tag).AccumulateItems(" ");

            return string.IsNullOrEmpty(tagsText) ? Original : $"{Original} {tagsText}";
        }

        public void AddTag(HashTagData tag)
        {
            if (!tagsTable.ContainsKey(tag.Tag))
            {
                tagsTable.Add(tag.Tag, (tag, false));
            }
        }
    }
}
