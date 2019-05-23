using System.Threading.Tasks;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart.Location
{
    public interface ISmartTagsByLocation
    {
        Task<HashTagData[]> Get(Classes.Models.Location.Location location);
    }
}
