using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TerWoord.LoggingAnalytics.Server.Services;

namespace TerWoord.LoggingAnalytics.Server.Controllers
{
    [Route("api/v2")]
    public class AppInsightsV2Controller: Controller
    {
        private readonly ITenantRepository  _tenantRepository;
        private readonly IMessageFilter     _messageFilter;
        private readonly IIndexNameBuilder  _indexNameBuilder;
        private readonly ConnectionSettings _elasticClientSettings;

        public AppInsightsV2Controller([NotNull] ITenantRepository  tenantRepository,
                                       [NotNull] IMessageFilter     messageFilter,
                                       [NotNull] IIndexNameBuilder  indexNameBuilder,
                                       [NotNull] ConnectionSettings elasticClientSettings)
        {
            _tenantRepository      = tenantRepository      ?? throw new ArgumentNullException(nameof(tenantRepository));
            _messageFilter         = messageFilter         ?? throw new ArgumentNullException(nameof(messageFilter));
            _indexNameBuilder      = indexNameBuilder      ?? throw new ArgumentNullException(nameof(indexNameBuilder));
            _elasticClientSettings = elasticClientSettings ?? throw new ArgumentNullException(nameof(elasticClientSettings));
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _tenantRepository.EnsureInitializedAsync().ConfigureAwait(false);

            using (var memoryStream = new MemoryStream((int)Request.ContentLength.GetValueOrDefault(0)))
            {
                if (Request.Headers["Content-Encoding"] == "gzip")
                {
                    using (var gunzip = new GZipStream(Request.Body, CompressionMode.Decompress))
                    {
                        await gunzip.CopyToAsync(memoryStream).ConfigureAwait(false);
                    }
                }
                else
                {
                    await Request.Body.CopyToAsync(memoryStream).ConfigureAwait(false);
                }

                string instrumentationKey = null;
                memoryStream.Position = 0;

                var objectsByIndex = new Dictionary<string, LinkedList<JObject>>();

                using (var reader = new JsonTextReader(new StreamReader(memoryStream, Encoding.UTF8, false, 8192, true))
                                    {
                                        SupportMultipleContent = true
                                    })
                {
                    while (reader.Read())
                    {
                        var loadedObject = JObject.Load(reader);
                        var thisKey      = loadedObject["iKey"]?.Value<string>();
                        if (instrumentationKey == null)
                        {
                            instrumentationKey = thisKey;
                        }
                        else if (instrumentationKey != thisKey)
                        {
                            throw new Exception("cannot mix messages for different targets!");
                        }

                        var time = loadedObject["time"].Value<DateTime>();

                        var messageType = loadedObject["data"]?["baseType"].Value<string>() ?? "Unknown";

                        var indexName = _indexNameBuilder.GetIndexName(thisKey, time, messageType);

                        _messageFilter.SanitizeMessage(loadedObject);

                        if (!objectsByIndex.TryGetValue(indexName, out var list))
                        {
                            list = new LinkedList<JObject>();
                            objectsByIndex.Add(indexName, list);
                        }
                        list.AddLast(loadedObject);
                    }
                }

                var tenant = _tenantRepository.All.SingleOrDefault(i => i.Identifier == instrumentationKey);
                if (tenant == null)
                {
                    return base.BadRequest();
                }

                var client = new ElasticClient(_elasticClientSettings);

                foreach (var item in objectsByIndex)
                {
                    // hand-craft a bulk request here
                    var builder = new StringBuilder();
                    foreach (var obj in item.Value)
                    {
                        builder.Append("{\"index\": { \"_type\": \"doc\", \"_index\": \"");
                        builder.Append(item.Key);
                        builder.AppendLine("\"}}");
                        builder.AppendLine(JsonConvert.SerializeObject(obj, Formatting.None));
                    }

                    await client.LowLevel.BulkAsync<StringResponse>(builder.ToString()).ConfigureAwait(false);

                    // process results.
                }

                return Accepted();
            }
        }
    }
}