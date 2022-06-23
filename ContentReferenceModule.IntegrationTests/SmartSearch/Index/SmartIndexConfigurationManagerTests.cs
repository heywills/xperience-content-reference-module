using CMS.Core;
using CMS.Scheduler;
using CMS.Search;
using CMS.SiteProvider;
using ContentReferenceModule.IntegrationTests.TestHelpers;
using Moq;
using NUnit.Framework;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Core;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Index;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Models;

namespace ContentReferenceModule.IntegrationTests.SmartSearch.Index
{
    [TestFixture]
    public class SmartIndexConfigurationManagerTests
    {
        private readonly ISiteInfoProvider _siteInfoProvider = Service.Resolve<ISiteInfoProvider>();
        private readonly ISearchIndexSiteInfoProvider _searchIndexSiteInfoProvider = Service.Resolve<ISearchIndexSiteInfoProvider>();
        private readonly ISearchIndexCultureInfoProvider _searchIndexCultureInfoProvider = Service.Resolve<ISearchIndexCultureInfoProvider>();
        private readonly ITaskInfoProvider _taskInfoProvider = Service.Resolve<ITaskInfoProvider>();
        private readonly ISmartIndexSettings _smartIndexSettings = TestFactories.CreateTestSmartIndexSettings();
        private readonly IEventLogService _eventLogService = new Mock<IEventLogService>().Object;


        [Test()]
        public void InitializeSmartIndex_Creates_A_SmartIndex()
        {
            var smartIndexConfigurationHelper = new SmartIndexConfigurationManager(_siteInfoProvider,
                                                                                  _searchIndexSiteInfoProvider,
                                                                                  _searchIndexCultureInfoProvider,
                                                                                  _taskInfoProvider,
                                                                                  _eventLogService);
            smartIndexConfigurationHelper.Initialize(_smartIndexSettings);
            Assert.Pass();
        }
    }
}
