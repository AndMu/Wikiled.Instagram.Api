using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Hashtags.Data;

namespace Wikiled.Instagram.Api.Hashtags
{
    public interface ISmartTagsManager
    {
        Task<SmartResults[]> GetAll(string[] tags);

        Task<LocationResult> GetByLocation(Location location, int radius);

        Task<string[]> GetByLocationSmart(Location location, int total = 3);

        Task<string[]> GetSmart(int total, params string[] tags);
    }
}