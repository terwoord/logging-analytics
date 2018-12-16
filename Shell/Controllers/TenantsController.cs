using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Nest;
using TerWoord.LoggingAnalytics.Server.Models;
using TerWoord.LoggingAnalytics.Server.Services;

namespace TerWoord.LoggingAnalytics.Server.Controllers
{
    [ApiController]
    [Route("/api/Tenants")]
    public class TenantsController: Controller
    {
        private readonly ITenantRepository  _repository;
        private readonly ConnectionSettings _connectionSettings;

        public TenantsController([NotNull] ITenantRepository repository, [NotNull] ConnectionSettings connectionSettings)
        {
            _repository         = repository         ?? throw new ArgumentNullException(nameof(repository));
            _connectionSettings = connectionSettings ?? throw new ArgumentNullException(nameof(connectionSettings));
        }

        public async Task<TenantObj[]> Get()
        {
            await _repository.EnsureInitializedAsync().ConfigureAwait(false);
            return _repository.All;
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TenantObj tenant)
        {
            var client = new ElasticClient(_connectionSettings);
            await client.CreateDocumentAsync(tenant).ConfigureAwait(false);
            return Ok();
        }
    }
}