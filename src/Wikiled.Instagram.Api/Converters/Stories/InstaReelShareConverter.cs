using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaReelShareConverter : IObjectConverter<InstaReelShare, InstaReelShareResponse>
    {
        public InstaReelShareResponse SourceObject { get; set; }

        public InstaReelShare Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var reelShare = new InstaReelShare
            {
                IsReelPersisted = SourceObject.IsReelPersisted ?? false,
                ReelOwnerId = SourceObject.ReelOwnerId,
                ReelType = SourceObject.ReelType,
                Text = SourceObject.Text,
                Type = SourceObject.Type
            };
            try
            {
                reelShare.Media = InstaConvertersFabric.Instance.GetStoryItemConverter(SourceObject.Media).Convert();
            }
            catch
            {
            }

            return reelShare;
        }
    }
}