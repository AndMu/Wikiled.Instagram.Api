﻿using System;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.TV;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Instagram TV api functions.
    /// </summary>
    public interface ITvProcessor
    {
        /// <summary>
        ///     Get channel by user id (pk) => channel owner
        /// </summary>
        /// <param name="userId">User id (pk) => channel owner</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        Task<IResult<InstaTvChannel>> GetChannelByIdAsync(long userId, PaginationParameters paginationParameters);

        /// <summary>
        ///     Get channel by <seealso cref="InstaTvChannelType" />
        /// </summary>
        /// <param name="channelType">Channel type</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        Task<IResult<InstaTvChannel>> GetChannelByTypeAsync(InstaTvChannelType channelType,
                                                            PaginationParameters paginationParameters);

        /// <summary>
        ///     Get suggested searches
        /// </summary>
        Task<IResult<InstaTvSearch>> GetSuggestedSearchesAsync();

        /// <summary>
        ///     Get TV Guide (gets popular and suggested channels)
        /// </summary>
        Task<IResult<InstaTv>> GetTvGuideAsync();

        /// <summary>
        ///     Search channels
        /// </summary>
        /// <param name="query">Channel or username</param>
        Task<IResult<InstaTvSearch>> SearchAsync(string query);

        /// <summary>
        ///     Upload video to Instagram TV
        /// </summary>
        /// <param name="video">
        ///     Video to upload (aspect ratio is very important for thumbnail and video | range 0.5 - 1.0 | Width =
        ///     480, Height = 852)
        /// </param>
        /// <param name="title">Title</param>
        /// <param name="caption">Caption</param>
        Task<IResult<InstaMedia>> UploadVideoAsync(VideoUpload video, string title, string caption);

        /// <summary>
        ///     Upload video to Instagram TV with progress
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="video">
        ///     Video to upload (aspect ratio is very important for thumbnail and video | range 0.5 - 1.0 | Width =
        ///     480, Height = 852)
        /// </param>
        /// <param name="title">Title</param>
        /// <param name="caption">Caption</param>
        Task<IResult<InstaMedia>> UploadVideoAsync(Action<UploaderProgress> progress,
                                                   VideoUpload video,
                                                   string title,
                                                   string caption);
    }
}