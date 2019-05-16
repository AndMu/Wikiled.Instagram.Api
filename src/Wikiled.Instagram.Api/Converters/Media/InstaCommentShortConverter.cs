using System;
using Wikiled.Instagram.Api.Classes.Models.Comment;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Media
{
    internal class InstaCommentShortConverter : IObjectConverter<InstaCommentShort, InstaCommentShortResponse>
    {
        public InstaCommentShortResponse SourceObject { get; set; }

        public InstaCommentShort Convert()
        {
            if (SourceObject == null)
            {
                return null;
            }

            var shortComment = new InstaCommentShort
            {
                CommentLikeCount = SourceObject.CommentLikeCount,
                ContentType =
                    (InstaContentType)Enum.Parse(typeof(InstaContentType), SourceObject.ContentType, true),
                CreatedAt = InstaDateTimeHelper.UnixTimestampToDateTime(SourceObject.CreatedAt),
                CreatedAtUtc = InstaDateTimeHelper.UnixTimestampToDateTime(SourceObject.CreatedAtUtc),
                Pk = SourceObject.Pk,
                Status = SourceObject.Status,
                Text = SourceObject.Text,
                Type = SourceObject.Type,
                User = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject.User).Convert(),
                HasLikedComment = SourceObject.HasLikedComment,
                MediaId = SourceObject.MediaId,
                ParentCommentId = SourceObject.ParentCommentId
            };
            return shortComment;
        }
    }
}