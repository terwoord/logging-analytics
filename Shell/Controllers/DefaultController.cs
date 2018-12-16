using System;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using TerWoord.LoggingAnalytics.Server.Services;

namespace TerWoord.LoggingAnalytics.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ITenantRepository _tenantRepository;

        public DefaultController([NotNull] ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var sb = new StringBuilder();
            await _tenantRepository.EnsureInitializedAsync().ConfigureAwait(false);
            sb.AppendLine(_tenantRepository.All.Length + " tenants in index");
            return sb.ToString();
        }
    }
}
