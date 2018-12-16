Ter Woord - Logging Analytics is a small .NET Core based web services, which lets you use Microsoft's Application Insights SDK's, to produce logging, and import that logging into ElasticSearch.

# Usage

For now, you have to build the .NET Core project. Next thing you do, is add the following contents to the appsettings.json file:

```json
    "ElasticSearchUrls": ["http://<address of elasticsearch node 1>:9200/"],
    "OutputIndexNameFormat": "twla-{tenant-id}-{datetime}-{message-type}", /* The format of indexes to use in elasticsearch */
    "OutputIndexNameDateFormat": "yyyy-MM", 
	"TenantListIndexName": "twla-config"  /* the elasticsearch index to use to store the tenants
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
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
