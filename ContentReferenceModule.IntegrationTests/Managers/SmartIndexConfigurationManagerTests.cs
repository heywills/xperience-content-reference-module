using CMS.Core;
using CMS.Search;
using CMS.SiteProvider;
using NUnit.Framework;
using XperienceCommunity.ContentReferenceModule.Core;
using XperienceCommunity.ContentReferenceModule.Managers;
using XperienceCommunity.ContentReferenceModule.Models;

namespace ContentReferenceModule.IntegrationTests.Helpers
{
    [TestFixture]
    public class SmartIndexConfigurationManagerTests
    {
        private ISiteInfoProvider _siteInfoProvider = Service.Resolve<ISiteInfoProvider>();
        private ISearchIndexSiteInfoProvider _searchIndexSiteInfoProvider = Service.Resolve<ISearchIndexSiteInfoProvider>();
        private ISearchIndexCultureInfoProvider _searchIndexCultureInfoProvider = Service.Resolve<ISearchIndexCultureInfoProvider>();
        private ISmartIndexSettings _smartIndexSettings = new SmartIndexSettings()
        {
            IndexName = "ContentRefererenceModule_IntegrationTests",
            IndexDisplayName = "Content Reference Module (Integration Tests Index)"
        };

        [Test()]
        public void InitializeSmartIndex_Creates_A_SmartIndex()
        {
            var smartIndexConfigurationHelper = new SmartIndexConfigurationManager(_siteInfoProvider,
                                                                                  _searchIndexSiteInfoProvider,
                                                                                  _searchIndexCultureInfoProvider,
                                                                                  _smartIndexSettings);
            smartIndexConfigurationHelper.InitializeSmartIndex();
            Assert.Pass();
        }
    }
}
