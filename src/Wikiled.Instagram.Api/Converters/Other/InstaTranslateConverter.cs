﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Other;

namespace Wikiled.Instagram.Api.Converters.Other
{
    internal class InstaTranslateConverter : IObjectConverter<InstaTranslate, InstaTranslateResponse>
    {
        public InstaTranslateResponse SourceObject { get; set; }

        public InstaTranslate Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var translate = new InstaTranslate { Id = SourceObject.Id, Translation = SourceObject.Translation };
            return translate;
        }
    }
}