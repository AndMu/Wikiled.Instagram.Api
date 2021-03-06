﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class
        InstaBroadcastCommentListConverter : IObjectConverter<InstaBroadcastCommentList,
            InstaBroadcastCommentListResponse>
    {
        public InstaBroadcastCommentListResponse SourceObject { get; set; }

        public InstaBroadcastCommentList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var broadcastCommentList = new InstaBroadcastCommentList
            {
                CaptionIsEdited = SourceObject.CaptionIsEdited ?? false,
                CommentCount = SourceObject.CommentCount ?? 0,
                CommentLikesEnabled = SourceObject.CommentLikesEnabled ?? false,
                CommentMuted = SourceObject.CommentMuted ?? 0,
                HasMoreComments = SourceObject.HasMoreComments ?? false,
                HasMoreHeadloadComments = SourceObject.HasMoreHeadloadComments ?? false,
                IsFirstFetch = SourceObject.IsFirstFetch,
                LiveSecondsPerComment = SourceObject.LiveSecondsPerComment ?? 0,
                MediaHeaderDisplay = SourceObject.MediaHeaderDisplay,
                SystemComments = SourceObject.SystemComments
            };

            if (SourceObject.Caption != null)
            {
                broadcastCommentList.Caption = InstaConvertersFabric.Instance
                    .GetCaptionConverter(SourceObject.Caption)
                    .Convert();
            }

            if (SourceObject.PinnedComment != null)
            {
                broadcastCommentList.PinnedComment = InstaConvertersFabric.Instance
                    .GetBroadcastCommentConverter(SourceObject.PinnedComment)
                    .Convert();
            }

            try
            {
                if (SourceObject.Comments?.Count > 0)
                {
                    foreach (var comment in SourceObject.Comments)
                    {
                        broadcastCommentList.Comments.Add(
                            InstaConvertersFabric.Instance
                                .GetBroadcastCommentConverter(comment)
                                .Convert());
                    }
                }
            }
            catch
            {
            }

            return broadcastCommentList;
        }
    }
}