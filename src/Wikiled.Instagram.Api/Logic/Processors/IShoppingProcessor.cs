using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Shopping;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Shopping and commerce api functions.
    /// </summary>
    public interface IShoppingProcessor
    {
        /// <summary>
        ///     Get product info
        /// </summary>
        /// <param name="productId">Product id (get it from <see cref="InstaProduct.ProductId" /> )</param>
        /// <param name="mediaPk">Media Pk (get it from <see cref="InstaMedia.Pk" />)</param>
        /// <param name="deviceWidth">Device width (pixel)</param>
        Task<IResult<InstaProductInfo>> GetProductInfoAsync(long productId, string mediaPk, int deviceWidth = 720);

        /// <summary>
        ///     Get all user shoppable media by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaMediaList" />
        /// </returns>
        Task<IResult<InstaMediaList>> GetUserShoppableMediaAsync(string username,
                                                                 PaginationParameters paginationParameters);

        /// <summary>
        ///     Get all user shoppable media by user id (pk)
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaMediaList" />
        /// </returns>
        Task<IResult<InstaMediaList>> GetUserShoppableMediaByIdAsync(long userId,
                                                                     PaginationParameters paginationParameters);

        //Task<IResult<InstaProductInfo>> GetCatalogsAsync();
    }
}