using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart
{
    public interface ITagEnricher
    {
        Task<SmartCaption> Enrich(InstaMedia message);
    }
}
