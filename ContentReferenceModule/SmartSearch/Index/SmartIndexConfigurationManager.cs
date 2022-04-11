using System;
using System.Linq;
using CMS.Base;
using CMS.Localization;
using CMS.Search;
using CMS.SiteProvider;
using XperienceCommunity.ContentReferenceModule.Helpers;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Models;

namespace XperienceCommunity.ContentReferenceModule.SmartSearch.Index
{
    /// <summary>
    /// Helper methods for creating and managing a Kentico Smart Index.
    /// </summary>
    public class SmartIndexConfigurationManager
    {
        private readonly ISiteInfoProvider _siteInfoProvider;
        private readonly ISearchIndexSiteInfoProvider _searchIndexSiteInfoProvider;
        private readonly ISearchIndexCultureInfoProvider _searchIndexCultureInfoProvider;
        private SearchIndexInfo _searchIndexInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteInfoProvider"></param>
        /// <param name="searchIndexSiteInfoProvider"></param>
        /// <param name="searchIndexCultureInfoProvider"></param>
        public SmartIndexConfigurationManager(ISiteInfoProvider siteInfoProvider,
                                              ISearchIndexSiteInfoProvider searchIndexSiteInfoProvider,
                                              ISearchIndexCultureInfoProvider searchIndexCultureInfoProvider)
        {
            Guard.ArgumentNotNull(siteInfoProvider);
            Guard.ArgumentNotNull(searchIndexSiteInfoProvider);
            Guard.ArgumentNotNull(searchIndexCultureInfoProvider);
            _siteInfoProvider = siteInfoProvider;
            _searchIndexSiteInfoProvider = searchIndexSiteInfoProvider;
            _searchIndexCultureInfoProvider = searchIndexCultureInfoProvider;
        }

        /// <summary>
        /// Initialize page index in Kentico with the provided index settings
        /// </summary>
        public void InitializeSmartIndex(SmartIndexSettings smartIndexSettings)
        {
            Guard.ArgumentNotNull(smartIndexSettings);
            Guard.ArgumentNotNullOrEmpty(smartIndexSettings.IndexName);
            Guard.ArgumentNotNullOrEmpty(smartIndexSettings.IndexDisplayName);
            _searchIndexInfo = GetSearchIndex(smartIndexSettings) ??
                               CreateSearchIndex(smartIndexSettings);
        }

        private SearchIndexInfo GetSearchIndex(SmartIndexSettings smartIndexSettings)
        {
            // TODO: Verify and update search index settings
            var searchIndexInfo = SearchIndexInfoProvider.GetSearchIndexInfo(smartIndexSettings.IndexName);
            return searchIndexInfo;
        }

        private SearchIndexInfo CreateSearchIndex(SmartIndexSettings smartIndexSettings)
        {
            using (new CMSActionContext {LogSynchronization = false})
            {
                var searchIndexInfo = SearchIndexInfoFactory.Create(smartIndexSettings.IndexName,
                                                                    smartIndexSettings.IndexDisplayName);
                SearchIndexInfoProvider.SetSearchIndexInfo(searchIndexInfo);
                AddAllSitesToIndex(searchIndexInfo);
                RebuildIndex(searchIndexInfo);
                return searchIndexInfo;
            }
        }

        /// <summary>
        /// Add a site to the content of the managed index.
        /// </summary>
        /// <param name="siteInfo"></param>
        public void AddSite(SiteInfo siteInfo)
        {
            VerifyInitialization();
            AddSite(_searchIndexInfo, siteInfo);
        }

        public void AddCulture(CultureInfo cultureInfo)
        {
            VerifyInitialization();
            AddCulture(_searchIndexInfo, cultureInfo);
        }

        /// <summary>
        /// Helper method to add a site to the content index
        /// </summary>
        /// <param name="siteInfo">The SiteInfo to add</param>
        /// <param name="searchIndexInfo"></param>
        private void AddSite(SearchIndexInfo searchIndexInfo, SiteInfo siteInfo)
        {
            _searchIndexSiteInfoProvider.Add(searchIndexInfo.IndexID, siteInfo.SiteID);
            var siteCultures = CultureSiteInfoProvider.GetSiteCultures(siteInfo.SiteName).ToList();
            siteCultures.ForEach(c => AddCulture(searchIndexInfo, c));
        }

        /// <summary>
        /// Helper method to add a culture to the content index
        /// </summary>
        /// <param name="searchIndexInfo"></param>
        /// <param name="cultureInfo"></param>
        private void AddCulture(SearchIndexInfo searchIndexInfo, CultureInfo cultureInfo)
        {
            _searchIndexCultureInfoProvider.Add(searchIndexInfo.IndexID, cultureInfo.CultureID);
        }

        /// <summary>
        /// Add all sites to the index
        /// </summary>
        /// <param name="searchIndexInfo"></param>
        private void AddAllSitesToIndex(SearchIndexInfo searchIndexInfo)
        {
            var sites = _siteInfoProvider.Get().ToList();
            sites.ForEach(s => AddSite(searchIndexInfo, s));
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

        private void VerifyInitialization()
        {
            if (_searchIndexInfo == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(SmartIndexConfigurationManager)} must be initialized before calling this method.");
            }
        }
    }
}
