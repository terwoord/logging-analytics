using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nest;
using TerWoord.LoggingAnalytics.Server.Models;

namespace TerWoord.LoggingAnalytics.Server.Services
{
    public class TenantRepositoryImpl: ITenantRepository
    {
        private readonly ElasticClient _client;

        public TenantRepositoryImpl([NotNull] ConnectionSettings connectionSettings)
        {
            if (connectionSettings == null)
            {
                throw new ArgumentNullException(nameof(connectionSettings));
            }

            _client = new ElasticClient(connectionSettings);
            _initializationTask = Task.Run(DoInitializeAsync);
        }

        private readonly Task _initializationTask;

        public async Task EnsureInitializedAsync()
        {
            await _initializationTask.ConfigureAwait(false);
        }

        private async Task DoInitializeAsync()
        {
            var tenants = await _client.SearchAsync<TenantObj>(s => s.Query(q => q.MatchAll())).ConfigureAwait(false);
            All = tenants.Documents.ToArray();
        }

        public TenantObj[] All
        {
            get;
            private set;
        }
    }
}