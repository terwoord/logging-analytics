using Newtonsoft.Json.Linq;

namespace TerWoord.LoggingAnalytics.Server.Services
{
    public interface IMessageFilter
    {
        void SanitizeMessage(JObject message);
    }
}