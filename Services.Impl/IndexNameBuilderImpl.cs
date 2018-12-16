using System;
using JetBrains.Annotations;

namespace TerWoord.LoggingAnalytics.Server.Services
{
    public class IndexNameBuilderImpl: IIndexNameBuilder
    {
        private readonly IConfigurationService _configurationService;

        public IndexNameBuilderImpl([NotNull] IConfigurationService configurationService)
        {
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        public string GetIndexName(string tenantId, DateTime messageDateTime, string messageType)
        {
            var result = _configurationService.OutputIndexNameFormat;

            result = result.Replace("{tenant-id}", tenantId.ToLower());
            result = result.Replace("{datetime}", messageDateTime.ToString(_configurationService.OutputIndexNameDateFormat));
            result = result.Replace("{message-type}", messageType.ToLower());
            return result;
        }
    }
}