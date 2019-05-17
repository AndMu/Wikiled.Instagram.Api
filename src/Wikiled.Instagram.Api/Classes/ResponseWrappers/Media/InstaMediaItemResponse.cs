﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaMediaItemResponse
    {
        [JsonProperty("can_viewer_reshare")]
        public bool CanViewerReshare { get; set; }

        [JsonProperty("can_viewer_save")]
        public bool CanViewerSave { get; set; }

        [JsonProperty("can_view_more_preview_comments")]
        public bool CanViewMorePreviewComments { get; set; }

        [JsonProperty("caption")]
        public InstaCaptionResponse Caption { get; set; }

        [JsonProperty("caption_is_edited")]
        public bool CaptionIsEdited { get; set; }

        [JsonProperty("carousel_media")]
        public InstaCarouselResponse CarouselMedia { get; set; }

        [JsonProperty("client_cache_key")]
        public string ClientCacheKey { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("comment_likes_enabled")]
        public bool CommentLikesEnabled { get; set; }

        [JsonProperty("comment_count")]
        public string CommentsCount { get; set; }

        [JsonProperty("comment_threading_enabled")]
        public bool CommentThreadingEnabled { get; set; }

        [JsonProperty("device_timestamp")]
        public string DeviceTimeStampUnixLike { get; set; }

        [JsonProperty("direct_reply_to_author_enabled")]
        public bool? DirectReplyToAuthorEnabled { get; set; }

        [JsonProperty("filter_type")]
        public string FilterType { get; set; }

        [JsonProperty("has_audio")]
        public bool HasAudio { get; set; }

        [JsonProperty("has_liked")]
        public bool HasLiked { get; set; }

        [JsonProperty("has_more_comments")]
        public bool HasMoreComments { get; set; }

        [JsonProperty("has_viewer_saved")]
        public bool HasViewerSaved { get; set; }

        [JsonProperty("original_height")]
        public string Height { get; set; }

        [JsonProperty("image_versions2")]
        public InstaImageCandidatesResponse Images { get; set; }

        [JsonProperty("id")]
        public string Identifier { get; set; }

        [JsonProperty("comments_disabled")]
        public bool IsCommentsDisabled { get; set; }

        [JsonProperty("likers")]
        public List<InstaUserShortResponse> Likers { get; set; }

        [JsonProperty("like_count")]
        public int LikesCount { get; set; }

        [JsonProperty("location")]
        public LocationResponse Location { get; set; }

        [JsonProperty("max_num_visible_preview_comments")]
        public int MaxNumVisiblePreviewComments { get; set; }

        [JsonProperty("media_type")]
        public InstaMediaType MediaType { get; set; }

        [JsonProperty("nearly_complete_copyright_match")]
        public bool? NearlyCompleteCopyrightMatch { get; set; }

        [JsonProperty("next_max_id")]
        public string NextMaxId { get; set; }

        [JsonProperty("number_of_qualities")]
        public int? NumberOfQualities { get; set; }

        [JsonProperty("photo_of_you")]
        public bool PhotoOfYou { get; set; }

        [JsonProperty("pk")]
        public string Pk { get; set; }

        [JsonProperty("preview_comments")]
        public List<InstaCommentResponse> PreviewComments { get; set; }

        [JsonProperty("product_tags")]
        public InstaProductTagsContainerResponse ProductTags { get; set; }

        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        [JsonProperty("taken_at")]
        public string TakenAtUnixLike { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("organic_tracking_token")]
        public string TrackingToken { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("user")]
        public InstaUserResponse User { get; set; }

        [JsonProperty("usertags")]
        public InstaUserTagListResponse UserTagList { get; set; }

        [JsonProperty("video_duration")]
        public double? VideoDuration { get; set; }

        [JsonProperty("video_versions")]
        public List<InstaVideoResponse> Videos { get; set; }

        [JsonProperty("view_count")]
        public double ViewCount { get; set; }

        [JsonProperty("original_width")]
        public int Width { get; set; }
    }
}