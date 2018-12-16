using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TerWoord.LoggingAnalytics.Server.Services;
using TerWoord.LoggingAnalytics.Server.Services.DummyServices;

namespace Services.Impl.Tests
{
    [TestClass]
    public class IndexNameBuilderImplTests
    {
        [TestMethod]
        public void TestDefaultConfigurationValues()
        {
            var config = new DummyConfigurationService(new[]
                                                       {
                                                           ""
                                                       },
                                                       "twla-config",
                                                       "twla-{tenant-id}-{datetime}-{message-type}",
                                                       "yyyy.MM");

            var indexNameBuilder = new IndexNameBuilderImpl(config);

            indexNameBuilder.GetIndexName("tenant1", new DateTime(2018, 12, 15, 18, 35, 36), "Metric").Should().Be("twla-tenant1-2018.12-metric");
        }
    }
}
