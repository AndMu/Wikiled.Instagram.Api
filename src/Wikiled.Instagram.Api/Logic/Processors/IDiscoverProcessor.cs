using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.Discover;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Discover api functions.
    /// </summary>
    public interface IDiscoverProcessor
    {
        /// <summary>
        ///     Clear Recent searches
        /// </summary>
        Task<IResult<bool>> ClearRecentSearchsAsync();

        /// <summary>
        ///     Get discover user chaining list
        /// </summary>
        Task<IResult<InstaUserChainingList>> GetChainingUsersAsync();

        /// <summary>
        ///     Get recent searches
        /// </summary>
        Task<IResult<InstaDiscoverRecentSearches>> GetRecentSearchesAsync();

        /// <summary>
        ///     Get suggested searches
        /// </summary>
        /// <param name="searchType">Search type(only blended and users works)</param>
        Task<IResult<InstaDiscoverSuggestedSearches>> GetSuggestedSearchesAsync(InstaDiscoverSearchType searchType = InstaDiscoverSearchType.Users);

        /// <summary>
        ///     Get top searches
        /// </summary>
        /// <param name="querry">querry string of the search</param>
        /// <param name="searchType">Search type(only blended and users works)</param>
        /// <param name="timezoneOffset">
        ///     Timezone offset of the search region (GMT Offset * 60 * 60 - Like Tehran GMT +3:30 = 3.5*
        ///     60*60 = 12600)
        /// </param>
        /// <returns></returns>
        Task<IResult<InstaDiscoverTopSearches>> GetTopSearchesAsync(string querry = "", InstaDiscoverSearchType searchType = InstaDiscoverSearchType.Users, int timezoneOffset = 12600);

        /// <summary>
        ///     Search user people
        /// </summary>
        /// <param name="query">Query to search</param>
        /// <param name="count">Count</param>
        Task<IResult<InstaDiscoverSearchResult>> SearchPeopleAsync(string query, int count = 50);

        /// <summary>
        ///     Sync your phone contact list to instagram
        ///     <para>Note:You can find your friends in instagram with this function</para>
        /// </summary>
        /// <param name="instaContacts">Contact list</param>
        Task<IResult<InstaContactUserList>> SyncContactsAsync(params InstaContact[] instaContacts);

        /// <summary>
        ///     Sync your phone contact list to instagram
        ///     <para>Note:You can find your friends in instagram with this function</para>
        /// </summary>
        /// <param name="instaContacts">Contact list</param>
        Task<IResult<InstaContactUserList>> SyncContactsAsync(InstaContactList instaContacts);
    }
}