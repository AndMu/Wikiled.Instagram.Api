using System;
using Wikiled.Text.Analysis.Emojis;

namespace Wikiled.Instagram.Api.Smart.Data
{
    public class HashTagData
    {
        private static readonly EmojyCleanup cleanup = new EmojyCleanup();

        private HashTagData(string tag, string text)
        {
            Tag = tag?.ToLower() ?? throw new ArgumentNullException(nameof(tag));
            Text = text?.ToLower() ?? throw new ArgumentNullException(nameof(text));
        }

        public string Tag { get; }

        public string Text { get; }

        public int? MediaCount { get; set; }

        public int? Relevance { get; set; }

        public int? Rank { get; set; }

        public static HashTagData FromTag(string tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            if (!tag.StartsWith("#"))
            {
                throw new ArgumentOutOfRangeException(nameof(tag));
            }

            var result = cleanup.Extract(tag);
            var text = result.Cleaned;
            return new HashTagData(tag, text.Substring(1));
        }

        public static HashTagData FromText(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (text.StartsWith("#"))
            {
                throw new ArgumentOutOfRangeException(nameof(text));
            }

            return new HashTagData("#" + text, text);
        }

        public override string ToString()
        {
            return Tag;
        }
    }
}
