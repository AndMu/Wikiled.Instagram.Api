﻿using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Location api functions.
    /// </summary>
    public interface ILocationProcessor
    {
        /// <summary>
        ///     Get location(place) information by external id or facebook places id
        ///     <para>
        ///         Get external id from this function:
        ///         <see cref="ILocationProcessor.SearchLocationAsync(double, double, string)" />
        ///     </para>
        ///     <para>
        ///         Get facebook places id from this function:
        ///         <see cref="ILocationProcessor.SearchPlacesAsync(double, double, string)(double, double, string)" />
        ///     </para>
        /// </summary>
        /// <param name="externalIdOrFacebookPlacesId">
        ///     External id or facebook places id of an location/place
        ///     <para>
        ///         Get external id from this function:
        ///         <see cref="ILocationProcessor.SearchLocationAsync(double, double, string)" />
        ///     </para>
        ///     <para>
        ///         Get facebook places id from this function:
        ///         <see cref="ILocationProcessor.SearchPlacesAsync(double, double, string)(double, double, string)" />
        ///     </para>
        /// </param>
        Task<IResult<PlaceShort>> GetLocationInfoAsync(string externalIdOrFacebookPlacesId);

        /// <summary>
        ///     Gets the stories of particular location.
        /// </summary>
        /// <param name="locationId">Location identifier (location pk, external id, facebook id)</param>
        /// <returns>
        ///     Location stories
        /// </returns>
        Task<IResult<InstaStory>> GetLocationStoriesAsync(long locationId);

        /// <summary>
        ///     Get recent location media feeds.
        ///     <para>Important note: Be careful of using this function, because it's an POST request</para>
        /// </summary>
        /// <param name="locationId">Location identifier (location pk, external id, facebook id)</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        Task<IResult<SectionMedia>> GetRecentLocationFeedsAsync(long locationId, PaginationParameters paginationParameters);

        /// <summary>
        ///     Get top (ranked) location media feeds.
        ///     <para>Important note: Be careful of using this function, because it's an POST request</para>
        /// </summary>
        /// <param name="locationId">Location identifier (location pk, external id, facebook id)</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        Task<IResult<SectionMedia>> GetTopLocationFeedsAsync(long locationId, PaginationParameters paginationParameters);

        /// <summary>
        ///     Searches for specific location by provided geo-data or search query.
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="query">Search query</param>
        /// <returns>
        ///     List of locations (short format)
        /// </returns>
        Task<IResult<LocationShortList>> SearchLocationAsync(double latitude, double longitude, string query);

        /// <summary>
        ///     Search places in facebook
        ///     <para>Note: This works for non-facebook accounts too!</para>
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="PlaceList" />
        /// </returns>
        Task<IResult<PlaceList>> SearchPlacesAsync(double latitude, double longitude, PaginationParameters paginationParameters);

        /// <summary>
        ///     Search places in facebook
        ///     <para>Note: This works for non-facebook accounts too!</para>
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="query">Query to search (city, country or ...)</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="PlaceList" />
        /// </returns>
        Task<IResult<PlaceList>> SearchPlacesAsync(double latitude, double longitude, string query, PaginationParameters paginationParameters);

        /// <summary>
        ///     Search user by location
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="desireUsername">Desire username</param>
        /// <param name="count">Maximum user count</param>
        Task<IResult<InstaUserSearchLocation>> SearchUserByLocationAsync(double latitude, double longitude, string desireUsername, int count = 50);
    }
}