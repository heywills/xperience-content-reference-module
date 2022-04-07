using CMS.Base;
using CMS.Search;
using CMS.SiteProvider;
using System.Linq;
using XperienceCommunity.ContentReferenceModule.Core;
using XperienceCommunity.ContentReferenceModule.Factories;
using XperienceCommunity.ContentReferenceModule.Helpers;

namespace XperienceCommunity.ContentReferenceModule.Managers
{
    /// <summary>
    /// Helper methods for creating and managing a Kentico Smart Index.
    /// </summary>
    public class SmartIndexConfigurationManager
    {
        private ISiteInfoProvider _siteInfoProvider;
        private ISearchIndexSiteInfoProvider _searchIndexSiteInfoProvider;
        private ISearchIndexCultureInfoProvider _searchIndexCultureInfoProvider;
        private ISmartIndexSettings _smartIndexSettings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteInfoProvider"></param>
        /// <param name="searchIndexSiteInfoProvider"></param>
        /// <param name="searchIndexCultureInfoProvider"></param>
        public SmartIndexConfigurationManager(ISiteInfoProvider siteInfoProvider,
                                              ISearchIndexSiteInfoProvider searchIndexSiteInfoProvider,
                                              ISearchIndexCultureInfoProvider searchIndexCultureInfoProvider,
                                              ISmartIndexSettings smartIndexSettings)
        {
            Guard.ArgumentNotNull(smartIndexSettings);
            Guard.ArgumentNotNull(siteInfoProvider);
            Guard.ArgumentNotNull(searchIndexSiteInfoProvider);
            Guard.ArgumentNotNull(searchIndexCultureInfoProvider);
            Guard.ArgumentNotNullOrEmpty(smartIndexSettings.IndexName);
            Guard.ArgumentNotNullOrEmpty(smartIndexSettings.IndexDisplayName);
            _smartIndexSettings = smartIndexSettings;
            _siteInfoProvider = siteInfoProvider;
            _searchIndexSiteInfoProvider = searchIndexSiteInfoProvider;
            _searchIndexCultureInfoProvider = searchIndexCultureInfoProvider;
        }

        /// <summary>
        /// Initialize page index in Kentico with the provided index settings
        /// </summary>
        public void InitializeSmartIndex()
        {
            // TODO: Don't create a new index if one exists
            using (new CMSActionContext { LogSynchronization = false })
            {
                var searchIndexInfo = SearchIndexInfoFactory.Create(_smartIndexSettings.IndexName,
                                                                    _smartIndexSettings.IndexDisplayName);
                SearchIndexInfoProvider.SetSearchIndexInfo(searchIndexInfo);
                AddAllSitesToIndex(searchIndexInfo);
                RebuildIndex(searchIndexInfo);
            }
        }

        /// <summary>
        /// Helper method to add a site to the content index
        /// </summary>
        /// <param name="indexId">The identifier of the index</param>
        /// <param name="siteInfo">The SiteInfo to add</param>
        private void AddSite(int indexId, SiteInfo siteInfo)
        {
            _searchIndexSiteInfoProvider.Add(indexId, siteInfo.SiteID);
            var siteCultures = CultureSiteInfoProvider.GetSiteCultures(siteInfo.SiteName).ToList();
            siteCultures.ForEach(c => AddCulture(indexId, c.CultureID));
        }

        /// <summary>
        /// Helper method to add a culture to the content index
        /// </summary>
        /// <param name="indexId">The identifier of the index</param>
        /// <param name="cultureId">The identifier of the culture</param>
        private void AddCulture(int indexId, int cultureId)
        {
            _searchIndexCultureInfoProvider.Add(indexId, cultureId);
        }

        /// <summary>
        /// Add all sites to the index
        /// </summary>
        /// <param name="searchIndexInfo"></param>
        private void AddAllSitesToIndex(SearchIndexInfo searchIndexInfo)
        {
            var sites = _siteInfoProvider.Get().ToList();
            var indexId = searchIndexInfo.IndexID;
            sites.ForEach(s => AddSite(indexId, s));
        }

        /// <summary>
        /// Rebuild the index.
        /// </summary>
        /// <param name="searchIndexInfo"></param>
        private void RebuildIndex(SearchIndexInfo searchIndexInfo)
        {
            SearchTaskInfoProvider.CreateTask(SearchTaskTypeEnum.Rebuild,
                                              null,
                                              null,
                                              searchIndexInfo.IndexName, 
                                              searchIndexInfo.IndexID);
        }
    }
}
