using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace TerWoord.LoggingAnalytics.Server.Services
{
    public class ConfigurationServiceImpl: IConfigurationService
    {
        public ConfigurationServiceImpl(IConfiguration configuration)
        {
            var section = configuration;
            var urls    = section.GetSection("ElasticSearchUrls").AsEnumerable();
            ElasticSearchHosts = urls.Where(i => !string.IsNullOrWhiteSpace(i.Value))
                                     .Select(i => i.Value)
                                     .ToArray();

            TenantListIndexName       = GetOrDefault(section, "TenantListIndexName",       "twla-config");
            OutputIndexNameFormat     = GetOrDefault(section, "OutputIndexNameFormat",     "twla-{tenant-id}-{datetime}-{message-type}");
            OutputIndexNameDateFormat = GetOrDefault(section, "OutputIndexNameDateFormat", "yyyy-MM");
        }

        private static string GetOrDefault([NotNull] IConfiguration section, string name, string defaultValue)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            var result = section.GetValue<string>(name);
            if (string.IsNullOrWhiteSpace(result))
            {
                return defaultValue;
            }
            return result;
        }

        public string[] ElasticSearchHosts
        {
            get;
        }

        public string TenantListIndexName
        {
            get;
        }

        public string OutputIndexNameFormat
        {
            get;
        }

        public string OutputIndexNameDateFormat
        {
            get;
        }
    }
}