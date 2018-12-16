namespace TerWoord.LoggingAnalytics.Server.Services
{
    public interface IConfigurationService
    {
        string[] ElasticSearchHosts
        {
            get;
        }

        string TenantListIndexName
        {
            get;
        }

        string OutputIndexNameFormat
        {
            get;
        }

        string OutputIndexNameDateFormat
        {
            get;
        }
    }
}