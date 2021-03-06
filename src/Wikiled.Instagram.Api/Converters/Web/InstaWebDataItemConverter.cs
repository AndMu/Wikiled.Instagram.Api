﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Web;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Web;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Web
{
    internal class InstaWebDataItemConverter : IObjectConverter<InstaWebDataItem, InstaWebDataItemResponse>
    {
        public InstaWebDataItemResponse SourceObject { get; set; }

        public InstaWebDataItem Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var data = new InstaWebDataItem { Text = SourceObject.Text };

            if (SourceObject.Timestamp != null)
            {
                data.Time = SourceObject.Timestamp.Value.FromUnixTimeSeconds();
            }
            else
            {
                data.Time = DateTime.MinValue;
            }

            return data;
        }
    }
}