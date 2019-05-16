using System;
using System.Globalization;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Media
{
    internal class InstaMediaAlbumConverter : IObjectConverter<InstaMedia, InstaMediaAlbumResponse>
    {
        public InstaMediaAlbumResponse SourceObject { get; set; }

        public InstaMedia Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var media = new InstaMedia
            {
                Identifier = SourceObject.Media.Identifier,
                Code = SourceObject.Media.Code,
                Pk = SourceObject.Media.Pk,
                ClientCacheKey = SourceObject.Media.ClientCacheKey,
                CommentsCount = SourceObject.Media.CommentsCount,
                DeviceTimeStamp =
                    InstaDateTimeHelper.UnixTimestampToDateTime(SourceObject.Media.DeviceTimeStampUnixLike),
                HasLiked = SourceObject.Media.HasLiked,
                PhotoOfYou = SourceObject.Media.PhotoOfYou,
                TrackingToken = SourceObject.Media.TrackingToken,
                TakenAt = InstaDateTimeHelper.UnixTimestampToDateTime(SourceObject.Media.TakenAtUnixLike),
                Height = SourceObject.Media.Height,
                LikesCount = SourceObject.Media.LikesCount,
                MediaType = SourceObject.Media.MediaType,
                FilterType = SourceObject.Media.FilterType,
                Width = SourceObject.Media.Width,
                HasAudio = SourceObject.Media.HasAudio,
                ViewCount = int.Parse(SourceObject.Media.ViewCount.ToString(CultureInfo.InvariantCulture)),
                IsCommentsDisabled = SourceObject.Media.IsCommentsDisabled
            };
            if (SourceObject.Media.CarouselMedia != null)
            {
                media.Carousel = InstaConvertersFabric.Instance.GetCarouselConverter(SourceObject.Media.CarouselMedia)
                    .Convert();
            }

            if (SourceObject.Media.User != null)
            {
                media.User = InstaConvertersFabric.Instance.GetUserConverter(SourceObject.Media.User).Convert();
            }

            if (SourceObject.Media.Caption != null)
            {
                media.Caption = InstaConvertersFabric.Instance.GetCaptionConverter(SourceObject.Media.Caption).Convert();
            }

            if (SourceObject.Media.NextMaxId != null)
            {
                media.NextMaxId = SourceObject.Media.NextMaxId;
            }

            if (SourceObject.Media.Likers != null && SourceObject.Media.Likers?.Count > 0)
            {
                foreach (var liker in SourceObject.Media.Likers)
                {
                    media.Likers.Add(InstaConvertersFabric.Instance.GetUserShortConverter(liker).Convert());
                }
            }

            if (SourceObject.Media.UserTagList?.In != null && SourceObject.Media.UserTagList?.In?.Count > 0)
            {
                foreach (var tag in SourceObject.Media.UserTagList.In)
                {
                    media.UserTags.Add(InstaConvertersFabric.Instance.GetUserTagConverter(tag).Convert());
                }
            }

            if (SourceObject.Media.PreviewComments != null)
            {
                foreach (var comment in SourceObject.Media.PreviewComments)
                {
                    media.PreviewComments.Add(InstaConvertersFabric.Instance.GetCommentConverter(comment).Convert());
                }
            }

            if (SourceObject.Media.Location != null)
            {
                media.Location = InstaConvertersFabric.Instance.GetLocationConverter(SourceObject.Media.Location).Convert();
            }

            if (SourceObject.Media.Images?.Candidates == null)
            {
                return media;
            }

            foreach (var image in SourceObject.Media.Images.Candidates)
            {
                media.Images.Add(new InstaImage(image.Url, int.Parse(image.Width), int.Parse(image.Height)));
            }

            if (SourceObject.Media.Videos == null)
            {
                return media;
            }

            foreach (var video in SourceObject.Media.Videos)
            {
                media.Videos.Add(
                    new InstaVideo(
                        video.Url,
                        int.Parse(video.Width),
                        int.Parse(video.Height),
                        video.Type));
            }

            return media;
        }
    }
}