using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Comment;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment;

namespace Wikiled.Instagram.Api.Converters.Media
{
    internal class InstaCommentListConverter : IObjectConverter<InstaCommentList, InstaCommentListResponse>
    {
        public InstaCommentListResponse SourceObject { get; set; }

        public InstaCommentList Convert()
        {
            var commentList = new InstaCommentList
            {
                Caption = SourceObject.Caption != null
                    ? InstaConvertersFabric.Instance.GetCaptionConverter(SourceObject.Caption).Convert()
                    : null,
                CanViewMorePreviewComments = SourceObject.CanViewMorePreviewComments,
                CaptionIsEdited = SourceObject.CaptionIsEdited,
                CommentsCount = SourceObject.CommentsCount,
                MoreCommentsAvailable = SourceObject.MoreCommentsAvailable,
                InitiateAtTop = SourceObject.InitiateAtTop,
                InsertNewCommentToTop = SourceObject.InsertNewCommentToTop,
                MediaHeaderDisplay = SourceObject.MediaHeaderDisplay,
                ThreadingEnabled = SourceObject.ThreadingEnabled,
                LikesEnabled = SourceObject.LikesEnabled,
                MoreHeadLoadAvailable = SourceObject.MoreHeadLoadAvailable,
                NextMaxId = SourceObject.NextMaxId,
                NextMinId = SourceObject.NextMinId
            };
            if (SourceObject.Comments == null || !(SourceObject?.Comments?.Count > 0))
            {
                return commentList;
            }

            foreach (var commentResponse in SourceObject.Comments)
            {
                var converter = InstaConvertersFabric.Instance.GetCommentConverter(commentResponse);
                commentList.Comments.Add(converter.Convert());
            }

            if (SourceObject.PreviewComments != null && SourceObject.PreviewComments.Any())
            {
                foreach (var cmt in SourceObject.PreviewComments)
                {
                    try
                    {
                        commentList.PreviewComments.Add(InstaConvertersFabric.Instance.GetCommentConverter(cmt).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            return commentList;
        }
    }
}