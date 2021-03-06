﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct;

namespace Wikiled.Instagram.Api.Converters.Directs
{
    internal class InstaDirectBroadcastConverter : IObjectConverter<InstaDirectBroadcast, InstaDirectBroadcastResponse>
    {
        public InstaDirectBroadcastResponse SourceObject { get; set; }

        public InstaDirectBroadcast Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var broadcast = new InstaDirectBroadcast
            {
                Broadcast = SourceObject.Broadcast,
                Text = SourceObject.Text,
                IsLinked = SourceObject.IsLinked ?? false,
                Message = SourceObject.Message,
                Title = SourceObject.Title
            };

            return broadcast;
        }
    }
}