using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Comment;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Media
{
    internal class InstaCommentConverter
        : IObjectConverter<InstaComment, InstaCommentResponse>
    {
        public InstaCommentResponse SourceObject { get; set; }

        public InstaComment Convert()
        {
            var comment = new InstaComment
            {
                BitFlags = SourceObject.BitFlags,
                ContentType =
                    (InstaContentType)Enum.Parse(typeof(InstaContentType), SourceObject.ContentType, true),
                CreatedAt = InstaDateTimeHelper.UnixTimestampToDateTime(SourceObject.CreatedAt),
                CreatedAtUtc = InstaDateTimeHelper.UnixTimestampToDateTime(SourceObject.CreatedAtUtc),
                LikesCount = SourceObject.LikesCount,
                Pk = SourceObject.Pk,
                Status = SourceObject.Status,
                Text = SourceObject.Text,
                Type = SourceObject.Type,
                UserId = SourceObject.UserId,
                User = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject.User).Convert(),
                DidReportAsSpam = SourceObject.DidReportAsSpam,
                ChildCommentCount = SourceObject.ChildCommentCount,
                HasLikedComment = SourceObject.HasLikedComment,
                HasMoreHeadChildComments = SourceObject.HasMoreHeadChildComments,
                HasMoreTailChildComments = SourceObject.HasMoreTailChildComments
            };
            if (SourceObject.OtherPreviewUsers != null && SourceObject.OtherPreviewUsers.Any())
            {
                if (comment.OtherPreviewUsers == null)
                {
                    comment.OtherPreviewUsers = new List<UserShortDescription>();
                }

                foreach (var user in SourceObject.OtherPreviewUsers)
                {
                    comment.OtherPreviewUsers.Add(InstaConvertersFabric.Instance.GetUserShortConverter(user).Convert());
                }
            }

            if (SourceObject.PreviewChildComments != null && SourceObject.PreviewChildComments.Any())
            {
                if (comment.PreviewChildComments == null)
                {
                    comment.PreviewChildComments = new List<InstaCommentShort>();
                }

                foreach (var cm in SourceObject.PreviewChildComments)
                {
                    comment.PreviewChildComments.Add(InstaConvertersFabric.Instance.GetCommentShortConverter(cm).Convert());
                }
            }

            return comment;
        }
    }
}