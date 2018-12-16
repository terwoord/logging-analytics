using System;
using System.Linq;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using TerWoord.LoggingAnalytics.Server.Models;
using TerWoord.LoggingAnalytics.Server.Services;
using TerWoord.LoggingAnalytics.Server.Services.MessageFilters;

namespace TerWoord.LoggingAnalytics.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration
        {
            get;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerDocument();

            services.AddSingleton<IConfigurationService, ConfigurationServiceImpl>();

            services.AddSingleton(sp =>
                                  {
                                      var configurationService = sp.GetRequiredService<IConfigurationService>();
                                      var connectionPool       = new StickyConnectionPool(configurationService.ElasticSearchHosts.Select(i => new Uri(i)));
                                      var result               = new ConnectionSettings(connectionPool);
                                      result.DefaultMappingFor<TenantObj>(m => m.IndexName(configurationService.TenantListIndexName)
                                                                                .IdProperty(i => i.Identifier)
                                                                                .TypeName("tenant"));
                                      return result;
                                  });

            //services.AddSingleton<ITransport<IConnectionConfigurationValues>>(sp =>
            //                                                                  {
            //                                                                      var configurationService    = sp.GetRequiredService<IConfigurationService>();
            //                                                                      //var connectionPool          = new StickyConnectionPool(configurationService.ElasticSearchHosts.Select(i => new Uri(i)));
            //                                                                      var connectionConfiguration = new ConnectionConfiguration(new Uri(configurationService.ElasticSearchHosts[0]));

            //                                                                      return new Transport<IConnectionConfigurationValues>(connectionConfiguration);
            //                                                                  });

            services.AddSingleton<IMessageFilter, DefaultMessageFilter>();
            services.AddSingleton<ITenantRepository, TenantRepositoryImpl>();
            services.AddSingleton<IIndexNameBuilder, IndexNameBuilderImpl>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi3();
        }
    }
}