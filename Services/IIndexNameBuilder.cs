using System;

namespace TerWoord.LoggingAnalytics.Server.Services
{
    public interface IIndexNameBuilder
    {
        string GetIndexName(string tenantId, DateTime messageDateTime, string messageType);
    }
}