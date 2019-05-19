using System;
using System.Text.RegularExpressions;

namespace Wikiled.Instagram.Api.Hashtags.Data
{
    public class HashTagData
    {
        private HashTagData(string tag, string text)
        {
            Tag = tag?.ToLower() ?? throw new ArgumentNullException(nameof(tag));
            Text = text?.ToLower() ?? throw new ArgumentNullException(nameof(text));
        }

        public string Tag { get; }

        public string Text { get; }

        public static HashTagData FromTag(string tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            if (!tag.StartsWith("#"))
            {
                throw new ArgumentOutOfRangeException(nameof(tag));
            }

            var text = Regex.Replace(tag, @"[^\u0020-\u007E]", string.Empty);
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
    }
}
