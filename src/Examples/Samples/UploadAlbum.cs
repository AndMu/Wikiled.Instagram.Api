﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Logic;

/////////////////////////////////////////////////////////////////////
////////////////////// IMPORTANT NOTE ///////////////////////////////
// Please check wiki pages for more information:
// https://github.com/ramtinak/InstagramApiSharp/wiki
////////////////////// IMPORTANT NOTE ///////////////////////////////
/////////////////////////////////////////////////////////////////////
namespace Examples.Samples
{
    internal class InstaUploadAlbum : IDemoSample
    {
        // There are two way that you can upload your videos and photos as an album.
        // Way 1 is DoShow() function, but it has an issue that described in https://github.com/ramtinak/InstagramApiSharp/issues/95
        // Way 2 [NewAlbumUpload() function] fixes this issue but it's little bit harder.

        private readonly IInstaApi api;

        public InstaUploadAlbum(IInstaApi instaApi)
        {
            api = instaApi;
        }

        public async Task DoShow()
        {
            var images = new[]
            {
                new ImageUpload
                {
                    // leave zero, if you don't know how height and width is it.
                    Height = 0,
                    Width = 0,
                    Uri = @"c:\image1.jpg",
                    // add user tags to your images
                    UserTags = new List<UserTagUpload>
                    {
                        new UserTagUpload { Username = "rmt4006", X = 0.5, Y = 0.5 }
                    }
                },
                new ImageUpload
                {
                    // leave zero, if you don't know how height and width is it.
                    Height = 0, Width = 0, Uri = @"c:\image2.jpg"
                }
            };

            var videos = new[]
            {
                new VideoUpload
                {
                    // leave zero, if you don't know how height and width is it.
                    Video = new InstaVideo(@"c:\video1.mp4", 0, 0),
                    VideoThumbnail = new InstaImage(@"c:\video thumbnail 1.jpg", 0, 0),
                    // Add user tag (tag people)
                    UserTags = new List<UserTagVideoUpload>
                    {
                        new UserTagVideoUpload { Username = "rmt4006" }
                    }
                },
                new VideoUpload
                {
                    // leave zero, if you don't know how height and width is it.
                    Video = new InstaVideo(@"c:\video2.mp4", 0, 0),
                    VideoThumbnail = new InstaImage(@"c:\video thumbnail 2.jpg", 0, 0)
                }
            };

            var result = await api.MediaProcessor.UploadAlbumAsync(images, videos, "Hey, this my first album upload via InstagramApiSharp library.").ConfigureAwait(false);

            // Above result will be something like this: IMAGE1, IMAGE2, VIDEO1, VIDEO2
            Console.WriteLine(result.Succeeded
                                  ? $"Media created: {result.Value.Pk}, {result.Value.Caption}"
                                  : $"Unable to upload album: {result.Info.Message}");
        }

        public async Task NewAlbumUpload()
        {
            var album = new List<AlbumUpload>();
            // IMPORTANT NOTE: only set one of ImageToUpload or VideoToUpload in AlbumUpload class!
            // unless it will choose ImageToUpload automatically!.

            // IMAGE 1
            album.Add(new AlbumUpload
            {
                ImageToUpload = new ImageUpload
                {
                    // leave zero, if you don't know how height and width is it.
                    Height = 0,
                    Width = 0,
                    Uri = @"c:\image1.jpg",
                    // add user tags to your images
                    UserTags = new List<UserTagUpload>
                    {
                        new UserTagUpload { Username = "rmt4006", X = 0.5, Y = 0.5 }
                    }
                }
            });

            // VIDEO 1
            album.Add(new AlbumUpload
            {
                VideoToUpload = new VideoUpload
                {
                    // leave zero, if you don't know how height and width is it.
                    Video = new InstaVideo(@"c:\video1.mp4", 0, 0),
                    VideoThumbnail = new InstaImage(@"c:\video thumbnail 1.jpg", 0, 0),
                    // Add user tag (tag people)
                    UserTags = new List<UserTagVideoUpload>
                    {
                        new UserTagVideoUpload { Username = "rmt4006" }
                    }
                }
            });

            // VIDEO 2
            album.Add(new AlbumUpload
            {
                VideoToUpload = new VideoUpload
                {
                    // leave zero, if you don't know how height and width is it.
                    Video = new InstaVideo(@"c:\video2.mp4", 0, 0),
                    VideoThumbnail = new InstaImage(@"c:\video thumbnail 2.jpg", 0, 0)
                }
            });

            // IMAGE 2
            album.Add(new AlbumUpload
            {
                ImageToUpload = new ImageUpload
                {
                    // leave zero, if you don't know how height and width is it.
                    Height = 0, Width = 0, Uri = @"c:\image2.jpg"
                }
            });


            var result = await api.MediaProcessor.UploadAlbumAsync(album.ToArray(),
                                                                   "Hey, this my first album upload via InstagramApiSharp library.").ConfigureAwait(false);

            // Above result will be something like this: IMAGE1, VIDEO1, VIDEO2, IMAGE2 [You can mix photos and videos together]

            Console.WriteLine(result.Succeeded
                                  ? $"Media created: {result.Value.Pk}, {result.Value.Caption}"
                                  : $"Unable to upload album: {result.Info.Message}");
        }
    }
}