using System;
using System.Linq;
using CMS.Base;
using CMS.Core;
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
        private readonly IEventLogService _eventLogService;
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
                                              ITaskInfoProvider taskInfoProvider,
                                              IEventLogService eventLogService)
        {
            Guard.ArgumentNotNull(siteInfoProvider);
            Guard.ArgumentNotNull(searchIndexSiteInfoProvider);
            Guard.ArgumentNotNull(searchIndexCultureInfoProvider);
            _siteInfoProvider = siteInfoProvider;
            _searchIndexSiteInfoProvider = searchIndexSiteInfoProvider;
            _searchIndexCultureInfoProvider = searchIndexCultureInfoProvider;
            _taskInfoProvider = taskInfoProvider;
            _eventLogService = eventLogService;
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
            // https://github.com/heywills/xperience-content-reference-module/issues/3
            var searchIndexInfo = SearchIndexInfoProvider.GetSearchIndexInfo(smartIndexSettings.IndexName);
            return searchIndexInfo;
        }

        private SearchIndexInfo CreateSearchIndex(ISmartIndexSettings smartIndexSettings)
        {
            try
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
                    _eventLogService.LogInformation(nameof(ContentReferenceModule),
                                                    "CreateSearchIndex",
                                                    $"Created new Smart Search index, {smartIndexSettings?.IndexName}.");
                    return searchIndexInfo;
                }
            }
            catch (Exception ex)
            {
                _eventLogService.LogWarning(nameof(ContentReferenceModule),
                                            "CreateSearchIndex",
                                            $"An unexpected exception occured when creating the Smart Search index, {smartIndexSettings?.IndexName}.\r\n\r\n"
                                            + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Add a site to the content of the managed index.
        /// </summary>
        /// <param name="siteInfo"></param>
        public void AddSite(SiteInfo siteInfo)
        {
            try
            {
                VerifyInitialization();
                AddSite(_searchIndexInfo, siteInfo);
            }
            catch (Exception ex)
            {
                _eventLogService.LogWarning(nameof(ContentReferenceModule),
                                            "AddSite",
                                            $"An unexpected exception occured when adding a site to the Smart Search index.\r\n" +
                                            $"Index: {_searchIndexInfo?.IndexName}\r\n" +
                                            $"Site:{siteInfo?.SiteName}\r\n\r\n" +
                                            ex.ToString());
            }
        }

        /// <summary>
        /// Add a culture to the content of the managed index
        /// </summary>
        /// <param name="cultureSiteInfo"></param>
        public void AddCulture(CultureSiteInfo cultureSiteInfo)
        {
            try
            {
                VerifyInitialization();
                AddCulture(_searchIndexInfo, cultureSiteInfo.CultureID);
            }
            catch (Exception ex)
            {
                _eventLogService.LogWarning(nameof(ContentReferenceModule),
                                            "AddCulture",
                                            $"An unexpected exception occured when adding a culture to the Smart Search index.\r\n" +
                                            $"Index: {_searchIndexInfo?.IndexName}\r\n" +
                                            $"Culture ID:{cultureSiteInfo?.CultureID}\r\n\r\n" +
                                            ex.ToString());
            }
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
                _eventLogService.LogWarning(nameof(ContentReferenceModule),
                                            "ExecuteSearchTasks",
                                            $"The 'Execute local search tasks' (${ScheduleTaskNameConstants.SearchTaskExecutor}) scheduled task was not found.");
                return;
            }

            if (taskInfo.TaskIsRunning)
            {
                _eventLogService.LogInformation(nameof(ContentReferenceModule),
                                                "ExecuteSearchTasks",
                                                $"The 'Execute local search tasks' (${ScheduleTaskNameConstants.SearchTaskExecutor}) scheduled task is already running.");
                return;
            }

            if (!taskInfo.TaskEnabled)
            {
                _eventLogService.LogWarning(nameof(ContentReferenceModule),
                                            "ExecuteSearchTasks",
                                            $"The 'Execute local search tasks' (${ScheduleTaskNameConstants.SearchTaskExecutor}) scheduled task cannot be run, because it is not enabled.");
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
