﻿using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.SiteProvider;
using System;
using XperienceCommunity.ContentReferenceModule.Constants;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Core;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Core;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Services
{
    /// <summary>
    /// Ensure the Content Reference index is created, that its being updated when
    /// content is changed, and that its configuration is updated when site and site-culture
    /// changes occur.
    /// </summary>
    internal class ContentReferenceIndexService : IContentReferenceIndexService
    {
        private readonly ISmartIndexConfigurationManager _smartIndexConfigurationManager;
        private readonly ISmartIndexSettings _smartIndexSettings;
        private readonly IContentInspectorService _contentInspectorService;

        internal ContentReferenceIndexService(ISmartIndexConfigurationManager smartIndexConfigurationManager,
                                              ISmartIndexSettings smartIndexSettings,
                                              IContentInspectorService contentInspectorService)
        {
            _smartIndexConfigurationManager = smartIndexConfigurationManager;
            _smartIndexSettings = smartIndexSettings;
            _contentInspectorService = contentInspectorService;
        }

        public void Initialize()
        {
            _smartIndexConfigurationManager.Initialize(_smartIndexSettings);
            SetupSystemMonitoringEvents();
        }

        /// <summary>
        /// Setup Kentico Global System event listners to moninitor content, sites,
        /// and site-cultures.
        /// </summary>
        private void SetupSystemMonitoringEvents()
        {
            DocumentEvents.GetContent.Execute += ContentObjectUsageIndex_GetContentExecute;

            SiteInfo.TYPEINFO.Events.Insert.After += ContentObjectUsageIndex_AddSite;
            CultureSiteInfo.TYPEINFO.Events.Insert.After += ContentObjectUsageIndex_AddCultureSite;

        }

        private void ContentObjectUsageIndex_GetContentExecute(object sender, DocumentSearchEventArgs e)
        {
            var indexInfo = e.IndexInfo;
            if (!indexInfo.IndexCodeName.Equals(_smartIndexSettings.IndexName,
                                                StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var treeNode = e.Node;
            var contentReferences = _contentInspectorService.GetContentReferences(treeNode);


            var nodeGuidsAsSearchTersm = string.Join(" ", contentReferences);
            e.SearchDocument.Add(ContentReferenceServiceConstants.IndexNodeReferencesFieldName,
                                 nodeGuidsAsSearchTersm,
                                 false,
                                 true);
        }

        private void ContentObjectUsageIndex_AddSite(object sender, ObjectEventArgs e)
        {
            var siteInfo = (SiteInfo)e?.Object;
            if (siteInfo != null)
            {
                _smartIndexConfigurationManager.AddSite(siteInfo);
            }
        }

        private void ContentObjectUsageIndex_AddCultureSite(object sender, ObjectEventArgs e)
        {
            var cultureSiteInfo = (CultureSiteInfo)e?.Object;
            if (cultureSiteInfo != null)
            {
                _smartIndexConfigurationManager.AddCulture(cultureSiteInfo);
            }
        }

    }
}