﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class
        InstaStoryTalliesItemConverter : IObjectConverter<InstaStoryTalliesItem, InstaStoryTalliesItemResponse>
    {
        public InstaStoryTalliesItemResponse SourceObject { get; set; }

        public InstaStoryTalliesItem Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var tallies = new InstaStoryTalliesItem
            {
                Count = SourceObject.Count, FontSize = SourceObject.FontSize, Text = SourceObject.Text
            };
            return tallies;
        }
    }
}