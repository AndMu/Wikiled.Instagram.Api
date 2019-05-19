using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart
{
    public interface ISmartTagsByLocation
    {
        Task<HashTagData[]> Get(Location location);
    }
}
