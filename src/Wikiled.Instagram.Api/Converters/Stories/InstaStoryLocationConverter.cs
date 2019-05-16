using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryLocationConverter : IObjectConverter<InstaStoryLocation, InstaStoryLocationResponse>
    {
        public InstaStoryLocationResponse SourceObject { get; set; }

        public InstaStoryLocation Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var storyLocation = new InstaStoryLocation
            {
                Height = SourceObject.Height,
                IsHidden = SourceObject.IsHidden,
                IsPinned = SourceObject.IsPinned,
                Rotation = SourceObject.Rotation,
                Width = SourceObject.Width,
                X = SourceObject.X,
                Y = SourceObject.Y,
                Z = SourceObject.Z,
                Location = InstaConvertersFabric.Instance.GetPlaceShortConverter(SourceObject.Location).Convert()
            };

            return storyLocation;
        }
    }
}