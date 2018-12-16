namespace TerWoord.LoggingAnalytics.Server.Services.DummyServices
{
    public class DummyConfigurationService: IConfigurationService
    {
        public DummyConfigurationService(string[] elasticSearchHosts,
                                         string   tenantListIndexName,
                                         string   outputIndexNameFormat,
                                         string   outputIndexNameDateFormat)
        {
            ElasticSearchHosts        = elasticSearchHosts;
            TenantListIndexName       = tenantListIndexName;
            OutputIndexNameFormat     = outputIndexNameFormat;
            OutputIndexNameDateFormat = outputIndexNameDateFormat;
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