using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace ContentReferenceModule.IntegrationTests
{
    [SetUpFixture]
    public class TestInitialization
    {
        [OneTimeSetUp]
        public void SetupTestRun()
        {
            Service.Use<IConfiguration>(() => new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.local.json", true, true)
                .Build());
            SystemContext.WebApplicationPhysicalPath = @"C:\_git\nctcog\dfwjobs-com-2021\src\CMS";
            CMSApplication.Init();
        }
    }
}
