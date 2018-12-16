using Newtonsoft.Json.Linq;

namespace TerWoord.LoggingAnalytics.Server.Services.MessageFilters
{
    public class DefaultMessageFilter: BaseMessageFilter
    {
        public override void SanitizeMessage(JObject message)
        {
            message.Remove("iKey");
            JsonHelpers.FlattenNestedObjects(message);
        }
    }
}