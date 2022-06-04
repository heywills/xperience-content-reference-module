using System;
using System.Linq;
using CMS.Base;
using CMS.Localization;
using CMS.Scheduler;
using CMS.Search;
using CMS.SiteProvider;
using XperienceCommunity.ContentReferenceModule.Constants;
using XperienceCommunity.ContentReferenceModule.Helpers;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Core;

namespace XperienceCommunity.ContentReferenceModule.SmartSearch.Index
{
    /// <summary>
    /// Helper methods for creating and managing a Kentico Smart Index.
    /// </summary>
    public class SmartIndexConfigurationManager : ISmartIndexConfigurationManager
    {
        private readonly ISiteInfoProvider _siteInfoProvider;
        private readonly ISearchIndexSiteInfoProvider _searchIndexSiteInfoProvider;
        private readonly ISearchIndexCultureInfoProvider _searchIndexCultureInfoProvider;
        private readonly ITaskInfoProvider _taskInfoProvider;
        private SearchIndexInfo _searchIndexInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteInfoProvider"></param>
        /// <param name="searchIndexSiteInfoProvider"></param>
        /// <param name="searchIndexCultureInfoProvider"></param>
        /// <param name="taskInfoProvider"></param>
        public SmartIndexConfigurationManager(ISiteInfoProvider siteInfoProvider,
                                              ISearchIndexSiteInfoProvider searchIndexSiteInfoProvider,
                                              ISearchIndexCultureInfoProvider searchIndexCultureInfoProvider,
                                              ITaskInfoProvider taskInfoProvider)
        {
            Guard.ArgumentNotNull(siteInfoProvider);
            Guard.ArgumentNotNull(searchIndexSiteInfoProvider);
            Guard.ArgumentNotNull(searchIndexCultureInfoProvider);
            _siteInfoProvider = siteInfoProvider;
            _searchIndexSiteInfoProvider = searchIndexSiteInfoProvider;
            _searchIndexCultureInfoProvider = searchIndexCultureInfoProvider;
            _taskInfoProvider = taskInfoProvider;
        }

        /// <summary>
        /// Initialize page index in Kentico with the provided index settings
        /// </summary>
        public void Initialize(ISmartIndexSettings smartIndexSettings)
        {
            Guard.ArgumentNotNull(smartIndexSettings);
            Guard.ArgumentNotNullOrEmpty(smartIndexSettings.IndexName);
            Guard.ArgumentNotNullOrEmpty(smartIndexSettings.IndexDisplayName);
            _searchIndexInfo = GetSearchIndex(smartIndexSettings) ??
                               CreateSearchIndex(smartIndexSettings);
        }

        private SearchIndexInfo GetSearchIndex(ISmartIndexSettings smartIndexSettings)
        {
            // TODO: Verify and update search index settings
            var searchIndexInfo = SearchIndexInfoProvider.GetSearchIndexInfo(smartIndexSettings.IndexName);
            return searchIndexInfo;
        }

        private SearchIndexInfo CreateSearchIndex(ISmartIndexSettings smartIndexSettings)
        {
            using (new CMSActionContext 
                       {
                            LogSynchronization = false,
                            ContinuousIntegrationAllowObjectSerialization = false
                       })
            {
                var searchIndexInfo = SearchIndexInfoFactory.Create(smartIndexSettings.IndexName,
                                                                    smartIndexSettings.IndexDisplayName);
                SearchIndexInfoProvider.SetSearchIndexInfo(searchIndexInfo);
                AddAllSitesToIndex(searchIndexInfo);
                RebuildIndex(searchIndexInfo);
                ExecuteSearchTasks();
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

        /// <summary>
        /// Add a culture to the content of the managed index
        /// </summary>
        /// <param name="cultureSiteInfo"></param>
        public void AddCulture(CultureSiteInfo cultureSiteInfo)
        {
            VerifyInitialization();
            AddCulture(_searchIndexInfo, cultureSiteInfo.CultureID);
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
            siteCultures.ForEach(c => AddCulture(searchIndexInfo, c.CultureID));
        }

        /// <summary>
        /// Helper method to add a culture to the content index
        /// </summary>
        /// <param name="searchIndexInfo"></param>
        /// <param name="cultureId"></param>
        private void AddCulture(SearchIndexInfo searchIndexInfo, int cultureId)
        {
            _searchIndexCultureInfoProvider.Add(searchIndexInfo.IndexID, cultureId);
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

        private void ExecuteSearchTasks()
        {
            var taskInfo = _taskInfoProvider.Get(ScheduleTaskNameConstants.SearchTaskExecutor, 0);
            if (taskInfo == null)
            {
                // TODO: Log that the 'Execute local search tasks' scheduled task was not found.
                return;
            }

            if (taskInfo.TaskIsRunning)
            {
                // TODO: Log that the task is already running
                return;
            }

            if (!taskInfo.TaskEnabled)
            {
                // TODO: Log that the task is not enabled
            }

            SchedulingExecutor.ExecuteTask(taskInfo);
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
