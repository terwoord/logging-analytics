Ter Woord - Logging Analytics
===

ElasticSearch-based sink for Microsoft Application Insights.

# Usage

For now, you have to build the .NET Core project. Next thing you do, is add the following contents to the appsettings.json file:

```json
    "ElasticSearchUrls": ["http://<address of elasticsearch node 1>:9200/"],
    "OutputIndexNameFormat": "twla-{tenant-id}-{datetime}-{message-type}", /* The format of indexes to use in elasticsearch */
    "OutputIndexNameDateFormat": "yyyy-MM", 
    "TenantListIndexName": "twla-config"  /* the elasticsearch index to use to store the tenants */
```

Multiple node addresses can be specified.

Now you can start the server process. This will start listening on all IP addresses at port ```5000```.

Now you need to add known instrumentation keys. Do so by ```POST```ing the following json document to http://twla:5000/ where twla is the address of the machine:

```json
{
	"identifier": "<instrumentation key>",
	"name": "<descriptive name>"
}
```

The identifier is the exact instrumentation key to use in the Client SDK configuration. The name is a descriptive name, which is not yet used anywhere.

Now you have to restart the service, to make the changes effect. (This is a known limitation, to be resolved at some point. See #1)

Now the service is usable, and is able to process Application Insight data.

# BSD 2-Clause License

Copyright (c) 2018, ter Woord Computers, Germany.

For details, see license.md