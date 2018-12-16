using System.Threading.Tasks;
using TerWoord.LoggingAnalytics.Server.Models;

namespace TerWoord.LoggingAnalytics.Server.Services
{
    public interface ITenantRepository
    {
        Task EnsureInitializedAsync();

        TenantObj[] All
        {
            get;
        }
    }
}