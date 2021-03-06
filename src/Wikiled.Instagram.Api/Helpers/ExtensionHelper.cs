﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Logic.Versions;

namespace Wikiled.Instagram.Api.Helpers
{
    internal static class InstaExtensionHelper
    {
        private static readonly Random Rnd = new Random();

        public static ImageUpload ConvertToImageUpload(this InstaImage instaImage,
                                                            UserTagUpload[] userTags = null)
        {
            return new ImageUpload
            {
                Height = instaImage.Height,
                ImageBytes = instaImage.ImageBytes,
                Uri = instaImage.Uri,
                Width = instaImage.Width,
                UserTags = userTags?.ToList()
            };
        }

        public static JObject ConvertToJson(this InstaStoryPollUpload poll)
        {
            var jArray = new JArray
            {
                new JObject { { "text", poll.Answer1 }, { "count", 0 }, { "font_size", poll.Answer1FontSize } },
                new JObject { { "text", poll.Answer2 }, { "count", 0 }, { "font_size", poll.Answer2FontSize } }
            };

            return new JObject
            {
                { "x", poll.X },
                { "y", poll.Y },
                { "z", poll.Z },
                { "width", poll.Width },
                { "height", poll.Height },
                { "rotation", poll.Rotation },
                { "question", poll.Question },
                { "viewer_vote", 0 },
                { "viewer_can_vote", true },
                { "tallies", jArray },
                { "is_shared_result", false },
                { "finished", false },
                { "is_sticker", poll.IsSticker }
            };
        }

        public static JObject ConvertToJson(this InstaStoryLocationUpload location)
        {
            return new JObject
            {
                { "x", location.X },
                { "y", location.Y },
                { "z", location.Z },
                { "width", location.Width },
                { "height", location.Height },
                { "rotation", location.Rotation },
                { "location_id", location.LocationId },
                { "is_sticker", location.IsSticker }
            };
        }

        public static JObject ConvertToJson(this InstaStoryHashtagUpload hashtag)
        {
            return new JObject
            {
                { "x", hashtag.X },
                { "y", hashtag.Y },
                { "z", hashtag.Z },
                { "width", hashtag.Width },
                { "height", hashtag.Height },
                { "rotation", hashtag.Rotation },
                { "tag_name", hashtag.TagName },
                { "is_sticker", hashtag.IsSticker }
            };
        }

        public static JObject ConvertToJson(this InstaStorySliderUpload slider)
        {
            return new JObject
            {
                { "x", slider.X },
                { "y", slider.Y },
                { "z", slider.Z },
                { "width", slider.Width },
                { "height", slider.Height },
                { "rotation", slider.Rotation },
                { "question", slider.Question },
                { "viewer_can_vote", true },
                { "viewer_vote", -1.0 },
                { "slider_vote_average", 0.0 },
                { "background_color", slider.BackgroundColor },
                { "emoji", $"{slider.Emoji}" },
                { "text_color", slider.TextColor },
                { "is_sticker", slider.IsSticker }
            };
        }

        public static JObject ConvertToJson(this InstaMediaStoryUpload mediaStory)
        {
            return new JObject
            {
                { "x", mediaStory.X },
                { "y", mediaStory.Y },
                { "width", mediaStory.Width },
                { "height", mediaStory.Height },
                { "rotation", mediaStory.Rotation },
                { "media_id", mediaStory.MediaPk },
                { "is_sticker", mediaStory.IsSticker }
            };
        }

        public static JObject ConvertToJson(this InstaStoryMentionUpload storyMention)
        {
            return new JObject
            {
                { "x", storyMention.X },
                { "y", storyMention.Y },
                { "z", storyMention.Z },
                { "width", storyMention.Width },
                { "height", storyMention.Height },
                { "rotation", storyMention.Rotation },
                { "user_id", storyMention.Pk }
            };
        }

        public static JObject ConvertToJson(this InstaStoryQuestionUpload question)
        {
            return new JObject
            {
                { "x", question.X },
                { "y", question.Y },
                { "z", question.Z },
                { "width", question.Width },
                { "height", question.Height },
                { "rotation", question.Rotation },
                { "question", question.Question },
                { "viewer_can_interact", question.ViewerCanInteract },
                { "profile_pic_url", question.ProfilePicture },
                { "question_type", question.QuestionType },
                { "background_color", question.BackgroundColor },
                { "text_color", question.TextColor },
                { "is_sticker", question.IsSticker }
            };
        }

        public static JObject ConvertToJson(this InstaStoryCountdownUpload countdown)
        {
            return new JObject
            {
                { "x", countdown.X },
                { "y", countdown.Y },
                { "z", countdown.Z },
                { "width", countdown.Width },
                { "height", countdown.Height },
                { "rotation", countdown.Rotation },
                { "text", countdown.Text },
                { "start_background_color", countdown.StartBackgroundColor },
                { "end_background_color", countdown.EndBackgroundColor },
                { "digit_color", countdown.DigitColor },
                { "digit_card_color", countdown.DigitCardColor },
                { "end_ts", countdown.EndTime.ToUnixTime() },
                { "text_color", countdown.TextColor },
                { "following_enabled", countdown.FollowingEnabled },
                { "is_sticker", countdown.IsSticker }
            };
        }

        public static string Encode(this long content)
        {
            return content.ToString().Encode();
        }

        public static string Encode(this string content)
        {
            return "\"" + content + "\"";
        }

        public static string EncodeList(this long[] listOfValues, bool appendQuotation = true)
        {
            return EncodeList(listOfValues.ToList(), appendQuotation);
        }

        public static string EncodeList(this string[] listOfValues, bool appendQuotation = true)
        {
            return EncodeList(listOfValues.ToList(), appendQuotation);
        }

        public static string EncodeList(this List<long> listOfValues, bool appendQuotation = true)
        {
            if (!appendQuotation)
            {
                return string.Join(",", listOfValues);
            }

            var list = new List<string>();
            foreach (var item in listOfValues)
            {
                list.Add(item.Encode());
            }

            return string.Join(",", list);
        }

        public static string EncodeList(this List<string> listOfValues, bool appendQuotation = true)
        {
            if (!appendQuotation)
            {
                return string.Join(",", listOfValues);
            }

            var list = new List<string>();
            foreach (var item in listOfValues)
            {
                list.Add(item.Encode());
            }

            return string.Join(",", list);
        }

        public static string EncodeRecipients(this long[] recipients)
        {
            return EncodeRecipients(recipients.ToList());
        }

        public static string EncodeRecipients(this List<long> recipients)
        {
            var list = new List<string>();
            foreach (var item in recipients)
            {
                list.Add($"[{item}]");
            }

            return string.Join(",", list);
        }

        public static string EncodeUri(this string data)
        {
            return WebUtility.UrlEncode(data);
        }

        public static string GenerateFacebookUserAgent()
        {
            var deviceInfo = AndroidDeviceGenerator.GetRandomAndroidDevice();

            //Mozilla/5.0 (Linux; Android 7.0; PRA-LA1 Build/HONORPRA-LA1; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/69.0.3497.100 Mobile Safari/537.36

            return string.Format(
                InstaApiConstants.FacebookUserAgent,
                deviceInfo.AndroidVer.VersionNumber,
                deviceInfo.DeviceModelIdentifier,
                $"{deviceInfo.AndroidBoardName}{deviceInfo.DeviceModel}");
        }

        public static string GenerateRandomString(this int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[Rnd.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        public static string GenerateUserAgent(this AndroidDevice deviceInfo, InstaApiVersion apiVersion)
        {
            if (deviceInfo == null)
            {
                return InstaApiConstants.UserAgentDefault;
            }

            if (deviceInfo.AndroidVer == null)
            {
                deviceInfo.AndroidVer = AndroidVersion.GetRandomAndriodVersion();
            }

            return string.Format(
                InstaApiConstants.UserAgent,
                deviceInfo.Dpi,
                deviceInfo.Resolution,
                deviceInfo.HardwareManufacturer,
                deviceInfo.DeviceModelIdentifier,
                deviceInfo.FirmwareBrand,
                deviceInfo.HardwareModel,
                apiVersion.AppVersion,
                deviceInfo.AndroidVer.ApiLevel,
                deviceInfo.AndroidVer.VersionNumber,
                apiVersion.AppApiVersionCode);
        }

        public static InstaTvChannelType GetChannelType(this string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return InstaTvChannelType.User;
            }

            switch (type.ToLower())
            {
                case "chrono_following":
                    return InstaTvChannelType.ChronoFollowing;
                case "continue_watching":
                    return InstaTvChannelType.ContinueWatching;
                case "for_you":
                    return InstaTvChannelType.ForYou;
                case "popular":
                    return InstaTvChannelType.Popular;
                default:
                case "user":
                    return InstaTvChannelType.User;
            }
        }

        public static string GetJson(this LocationShort location)
        {
            if (location == null)
            {
                return null;
            }

            return new JObject
            {
                { "name", location.Address ?? string.Empty },
                { "address", location.ExternalId ?? string.Empty },
                { "lat", location.Lat },
                { "lng", location.Lng },
                { "external_source", location.ExternalSource ?? "facebook_places" },
                { "facebook_places_id", location.ExternalId }
            }.ToString(Formatting.None);
        }

        public static string GetRealChannelType(this InstaTvChannelType type)
        {
            switch (type)
            {
                case InstaTvChannelType.ChronoFollowing:
                    return "chrono_following";
                case InstaTvChannelType.ContinueWatching:
                    return "continue_watching";
                case InstaTvChannelType.Popular:
                    return "popular";
                case InstaTvChannelType.User:
                    return "user";
                case InstaTvChannelType.ForYou:
                default:
                    return "for_you";
            }
        }

        public static bool IsEmpty(this string content)
        {
            return string.IsNullOrEmpty(content);
        }

        public static bool IsNotEmpty(this string content)
        {
            return !string.IsNullOrEmpty(content);
        }

        public static void PrintInDebug(this object obj)
        {
            Debug.WriteLine(Convert.ToString(obj));
        }
    }
}