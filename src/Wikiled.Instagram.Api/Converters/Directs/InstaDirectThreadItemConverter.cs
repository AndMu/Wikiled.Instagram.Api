using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Directs
{
    internal class InstaDirectThreadItemConverter : IObjectConverter<InstaDirectInboxItem, InstaDirectInboxItemResponse>
    {
        public InstaDirectInboxItemResponse SourceObject { get; set; }

        public InstaDirectInboxItem Convert()
        {
            var threadItem = new InstaDirectInboxItem
            {
                ClientContext = SourceObject.ClientContext, ItemId = SourceObject.ItemId
            };

            threadItem.TimeStamp = InstaDateTimeHelper.UnixTimestampMilisecondsToDateTime(SourceObject.TimeStamp);
            threadItem.UserId = SourceObject.UserId;

            var truncatedItemType = SourceObject.ItemType.Trim().Replace("_", "");
            if (Enum.TryParse(truncatedItemType, true, out InstaDirectThreadItemType type))
            {
                threadItem.ItemType = type;
            }

            if (threadItem.ItemType == InstaDirectThreadItemType.Link && SourceObject.Link != null)
            {
                threadItem.Text = SourceObject.Link.Text;
                try
                {
                    threadItem.LinkMedia = new InstaWebLink { Text = SourceObject.Link.Text };
                    if (SourceObject.Link.LinkContext != null)
                    {
                        threadItem.LinkMedia.LinkContext = new InstaWebLinkContext();

                        if (!string.IsNullOrEmpty(SourceObject.Link.LinkContext.LinkImageUrl))
                        {
                            threadItem.LinkMedia.LinkContext.LinkImageUrl = SourceObject.Link.LinkContext.LinkImageUrl;
                        }

                        if (!string.IsNullOrEmpty(SourceObject.Link.LinkContext.LinkSummary))
                        {
                            threadItem.LinkMedia.LinkContext.LinkSummary = SourceObject.Link.LinkContext.LinkSummary;
                        }

                        if (!string.IsNullOrEmpty(SourceObject.Link.LinkContext.LinkTitle))
                        {
                            threadItem.LinkMedia.LinkContext.LinkTitle = SourceObject.Link.LinkContext.LinkTitle;
                        }

                        if (!string.IsNullOrEmpty(SourceObject.Link.LinkContext.LinkUrl))
                        {
                            threadItem.LinkMedia.LinkContext.LinkUrl = SourceObject.Link.LinkContext.LinkUrl;
                        }
                    }
                }
                catch
                {
                }
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.Like)
            {
                threadItem.Text = SourceObject.Like;
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.Media && SourceObject.Media != null)
            {
                var converter = InstaConvertersFabric.Instance.GetInboxMediaConverter(SourceObject.Media);
                threadItem.Media = converter.Convert();
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.MediaShare && SourceObject.MediaShare != null)
            {
                var converter = InstaConvertersFabric.Instance.GetSingleMediaConverter(SourceObject.MediaShare);
                threadItem.MediaShare = converter.Convert();
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.StoryShare && SourceObject.StoryShare != null)
            {
                threadItem.StoryShare = new InstaStoryShare
                {
                    IsReelPersisted = SourceObject.StoryShare.IsReelPersisted,
                    ReelType = SourceObject.StoryShare.ReelType,
                    Text = SourceObject.StoryShare.Text,
                    IsLinked = SourceObject.StoryShare.IsLinked,
                    Message = SourceObject.StoryShare.Message,
                    Title = SourceObject.StoryShare.Title
                };
                if (SourceObject.StoryShare.Media != null)
                {
                    var converter = InstaConvertersFabric.Instance.GetSingleMediaConverter(SourceObject.StoryShare.Media);
                    threadItem.StoryShare.Media = converter.Convert();
                }
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.Text)
            {
                threadItem.Text = SourceObject.Text;
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.RavenMedia &&
                SourceObject.RavenMedia != null)
            {
                var converter = InstaConvertersFabric.Instance.GetVisualMediaConverter(SourceObject.RavenMedia);
                threadItem.RavenMedia = converter.Convert();
                threadItem.RavenSeenUserIds = SourceObject.RavenSeenUserIds;
                if (!string.IsNullOrEmpty(SourceObject.RavenViewMode))
                {
                    threadItem.RavenViewMode =
                        (InstaViewMode)Enum.Parse(typeof(InstaViewMode), SourceObject.RavenViewMode, true);
                }

                threadItem.RavenReplayChainCount = SourceObject.RavenReplayChainCount ?? 0;
                threadItem.RavenSeenCount = SourceObject.RavenSeenCount;
                if (SourceObject.RavenExpiringMediaActionSummary != null)
                {
                    var ravenType = SourceObject.RavenExpiringMediaActionSummary.Type.ToLower() == "raven_delivered"
                        ? InstaRavenType.Delivered
                        : InstaRavenType.Opened;
                    threadItem.RavenExpiringMediaActionSummary = new InstaRavenMediaActionSummary
                    {
                        Count = SourceObject.RavenExpiringMediaActionSummary.Count, Type = ravenType
                    };
                    if (!string.IsNullOrEmpty(SourceObject.RavenExpiringMediaActionSummary.TimeStamp))
                    {
                        threadItem.RavenExpiringMediaActionSummary.ExpireTime =
                            InstaDateTimeHelper.UnixTimestampMilisecondsToDateTime(
                                SourceObject.RavenExpiringMediaActionSummary.TimeStamp);
                    }
                }
            }

            // VisualMedia is updated RavenMedia for v61 and newer
            else if (threadItem.ItemType == InstaDirectThreadItemType.RavenMedia &&
                SourceObject.VisualMedia != null)
            {
                threadItem.VisualMedia = InstaConvertersFabric.Instance
                    .GetVisualMediaContainerConverter(SourceObject.VisualMedia)
                    .Convert();
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.ActionLog && SourceObject.ActionLogMedia != null)
            {
                threadItem.ActionLog = new InstaActionLog { Description = SourceObject.ActionLogMedia.Description };
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.Profile && SourceObject.ProfileMedia != null)
            {
                var converter = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject.ProfileMedia);
                threadItem.ProfileMedia = converter.Convert();
                if (SourceObject.ProfileMediasPreview != null && SourceObject.ProfileMediasPreview.Any())
                {
                    try
                    {
                        var previewMedias = new List<InstaMedia>();
                        foreach (var item in SourceObject.ProfileMediasPreview)
                        {
                            previewMedias.Add(InstaConvertersFabric.Instance.GetSingleMediaConverter(item).Convert());
                        }

                        threadItem.ProfileMediasPreview = previewMedias;
                    }
                    catch
                    {
                    }
                }
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.Placeholder && SourceObject.Placeholder != null)
            {
                threadItem.Placeholder = new InstaPlaceholder
                {
                    IsLinked = SourceObject.Placeholder.IsLinked, Message = SourceObject.Placeholder.Message
                };
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.Location && SourceObject.LocationMedia != null)
            {
                try
                {
                    threadItem.LocationMedia = new Classes.Models.Location.Location();
                    if (!string.IsNullOrEmpty(SourceObject.LocationMedia.Address))
                    {
                        threadItem.LocationMedia.Address = SourceObject.LocationMedia.Address;
                    }

                    if (!string.IsNullOrEmpty(SourceObject.LocationMedia.City))
                    {
                        threadItem.LocationMedia.City = SourceObject.LocationMedia.City;
                    }

                    if (!string.IsNullOrEmpty(SourceObject.LocationMedia.ExternalId))
                    {
                        threadItem.LocationMedia.ExternalId = SourceObject.LocationMedia.ExternalId;
                    }

                    if (!string.IsNullOrEmpty(SourceObject.LocationMedia.ExternalIdSource))
                    {
                        threadItem.LocationMedia.ExternalSource = SourceObject.LocationMedia.ExternalIdSource;
                    }

                    if (!string.IsNullOrEmpty(SourceObject.LocationMedia.ShortName))
                    {
                        threadItem.LocationMedia.ShortName = SourceObject.LocationMedia.ShortName;
                    }

                    if (!string.IsNullOrEmpty(SourceObject.LocationMedia.Name))
                    {
                        threadItem.LocationMedia.Name = SourceObject.LocationMedia.Name;
                    }

                    threadItem.LocationMedia.FacebookPlacesId = SourceObject.LocationMedia.FacebookPlacesId;
                    threadItem.LocationMedia.Lat = SourceObject.LocationMedia.Lat;
                    threadItem.LocationMedia.Lng = SourceObject.LocationMedia.Lng;
                }
                catch
                {
                }
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.FelixShare &&
                SourceObject.FelixShareMedia != null &&
                SourceObject.FelixShareMedia.Video != null)
            {
                try
                {
                    threadItem.FelixShareMedia = InstaConvertersFabric.Instance
                        .GetSingleMediaConverter(SourceObject.FelixShareMedia.Video)
                        .Convert();
                }
                catch
                {
                }
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.ReelShare && SourceObject.ReelShareMedia != null)
            {
                try
                {
                    threadItem.ReelShareMedia = InstaConvertersFabric.Instance
                        .GetReelShareConverter(SourceObject.ReelShareMedia)
                        .Convert();
                }
                catch
                {
                }
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.VoiceMedia && SourceObject.VoiceMedia != null)
            {
                try
                {
                    threadItem.VoiceMedia = InstaConvertersFabric.Instance.GetVoiceMediaConverter(SourceObject.VoiceMedia)
                        .Convert();
                }
                catch
                {
                }
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.AnimatedMedia &&
                SourceObject.AnimatedMedia != null)
            {
                try
                {
                    threadItem.AnimatedMedia = InstaConvertersFabric.Instance
                        .GetAnimatedImageConverter(SourceObject.AnimatedMedia)
                        .Convert();
                }
                catch
                {
                }
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.Hashtag && SourceObject.HashtagMedia != null)
            {
                try
                {
                    threadItem.HashtagMedia = InstaConvertersFabric.Instance
                        .GetDirectHashtagConverter(SourceObject.HashtagMedia)
                        .Convert();
                }
                catch
                {
                }
            }
            else if (threadItem.ItemType == InstaDirectThreadItemType.LiveViewerInvite &&
                SourceObject.LiveViewerInvite != null)
            {
                try
                {
                    threadItem.LiveViewerInvite = InstaConvertersFabric.Instance
                        .GetDirectBroadcastConverter(SourceObject.LiveViewerInvite)
                        .Convert();
                }
                catch
                {
                }
            }

            return threadItem;
        }
    }
}