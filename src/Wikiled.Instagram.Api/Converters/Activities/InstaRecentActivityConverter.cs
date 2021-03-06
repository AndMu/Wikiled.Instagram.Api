﻿using System.Globalization;
using Wikiled.Instagram.Api.Classes.Models.Feed;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Activities
{
    internal class
        InstaRecentActivityConverter : IObjectConverter<InstaRecentActivityFeed, InstaRecentActivityFeedResponse>
    {
        public InstaRecentActivityFeedResponse SourceObject { get; set; }

        public InstaRecentActivityFeed Convert()
        {
            var activityStory = new InstaRecentActivityFeed
            {
                Pk = SourceObject.Pk,
                Type = SourceObject.Type,
                ProfileId = SourceObject.Args.ProfileId,
                ProfileImage = SourceObject.Args.ProfileImage,
                Text = SourceObject.Args.Text,
                RichText = SourceObject.Args.RichText,
                TimeStamp = InstaDateTimeHelper.UnixTimestampToDateTime(
                    (long)System.Convert.ToDouble(SourceObject.Args.TimeStamp,
                                                  new NumberFormatInfo { NumberDecimalSeparator = "." }))
            };
            if (SourceObject.Args.Links != null)
            {
                foreach (var instaLinkResponse in SourceObject.Args.Links)
                {
                    activityStory.Links.Add(
                        new InstaLink
                        {
                            Start = instaLinkResponse.Start,
                            End = instaLinkResponse.End,
                            Id = instaLinkResponse.Id,
                            Type = instaLinkResponse.Type
                        });
                }
            }

            if (SourceObject.Args.InlineFollow != null)
            {
                activityStory.InlineFollow = new InstaInlineFollow
                {
                    IsFollowing = SourceObject.Args.InlineFollow.IsFollowing,
                    IsOutgoingRequest = SourceObject.Args.InlineFollow.IsOutgoingRequest
                };
                if (SourceObject.Args.InlineFollow.UserInfo != null)
                {
                    activityStory.InlineFollow.User =
                        InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject.Args.InlineFollow.UserInfo)
                            .Convert();
                }
            }

            if (SourceObject.Args.Media != null)
            {
                foreach (var media in SourceObject.Args.Media)
                {
                    activityStory.Medias.Add(
                        new InstaActivityMedia { Id = media.Id, Image = media.Image });
                }
            }

            return activityStory;
        }
    }
}