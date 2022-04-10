using CMS.Core;
using CMS.Search;
using CMS.SiteProvider;
using NUnit.Framework;
using XperienceCommunity.ContentReferenceModule.Core;
using XperienceCommunity.ContentReferenceModule.Managers;
using XperienceCommunity.ContentReferenceModule.Models;

namespace ContentReferenceModule.IntegrationTests.Managers
{
    [TestFixture]
    public class SmartIndexConfigurationManagerTests
    {
        private readonly ISiteInfoProvider _siteInfoProvider = Service.Resolve<ISiteInfoProvider>();
        private readonly ISearchIndexSiteInfoProvider _searchIndexSiteInfoProvider = Service.Resolve<ISearchIndexSiteInfoProvider>();
        private readonly ISearchIndexCultureInfoProvider _searchIndexCultureInfoProvider = Service.Resolve<ISearchIndexCultureInfoProvider>();
        private readonly ISmartIndexSettings _smartIndexSettings = new SmartIndexSettings()
        {
            IndexName = "ContentReferenceModule_IntegrationTests",
            IndexDisplayName = "Content Reference Module (Integration Tests Index)"
        };

        [Test()]
        public void InitializeSmartIndex_Creates_A_SmartIndex()
        {
            var smartIndexConfigurationHelper = new SmartIndexConfigurationManager(_siteInfoProvider,
                                                                                  _searchIndexSiteInfoProvider,
                                                                                  _searchIndexCultureInfoProvider);
            smartIndexConfigurationHelper.InitializeSmartIndex(_smartIndexSettings);
            Assert.Pass();
        }
    }
}
