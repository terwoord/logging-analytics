using Newtonsoft.Json.Linq;

namespace TerWoord.LoggingAnalytics.Server.Services.MessageFilters
{
    public abstract class BaseMessageFilter: IMessageFilter
    {
        public abstract void SanitizeMessage(JObject message);
    }
}