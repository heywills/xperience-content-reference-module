﻿using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Search;
using CMS.SiteProvider;
using System;
using XperienceCommunity.ContentReferenceModule.Cms.Extensions;
using XperienceCommunity.ContentReferenceModule.Constants;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Core;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Core;

namespace XperienceCommunity.ContentReferenceModule.SmartSearch.Index
{
    /// <summary>
    /// Ensure the Content Reference index is created, that its being updated when
    /// content is changed, and that its configuration is updated when site and site-culture
    /// changes occur.
    /// </summary>
    /// <remarks>
    /// Using a Page indexer has wonderful benefits:
    /// - Handles query performance and scalability for us.
    /// - Handles incremental updates automatically.
    ///
    /// Put is has a few problems
    /// - As of KX11 it forces custom field values to be lower case.
    /// - Add requests and processes full-page content even though we don't need it.
    /// - Includes fields we don't need.
    /// - Page Types can be excluded outside of our control.
    /// 
    /// Ideas:
    /// - Create a replacement for ILuceneSearchDocument. In AddGeneralField, set the
    ///   valueToLower to false. This only solves the first problem.
    ///   
    /// - Create a custom index. This is a lot more work
    ///   but will result in:
    ///   - a solution that is more stable in different environments
    ///   - a smaller index
    ///   To handle performance concerns, look at:
    ///   - DocumentSearchablesRetriever
    ///   - DocumentSearchIndexer.Rebuild 
    ///   To handle incremental update concerns, look at:
    ///   - DocumentHelper.UpdateDocument. It calls UpdateSearchIndexIfAllowed.
    ///   - Consder creating a custom SearchTaskInfo using SearchTaskInfoProvider.CreateTask
    ///     with the parameters necessary to call the customer indexer.
    /// 
    /// </remarks>
    internal class ContentReferenceIndexService : IContentReferenceIndexService
    {
        private readonly ISmartIndexConfigurationManager _smartIndexConfigurationManager;
        private readonly ISmartIndexSettings _smartIndexSettings;
        private readonly IContentInspectorService _contentInspectorService;

        public ContentReferenceIndexService(ISmartIndexConfigurationManager smartIndexConfigurationManager,
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
            var nodeGuidsAsSearchTerms = string.Join(" ", contentReferences);
            var documentNamePath = treeNode.CreateDocumentNamePath();
            var searchDocument = e.SearchDocument;
            searchDocument.Add(name: ContentReferenceServiceConstants.IndexNodeReferencesFieldName,
                               value: nodeGuidsAsSearchTerms,
                               store: true,
                               tokenize: true);

            // TODO: Find away to add DocumentNamePath without it being converted to lowercase.
            // Kentico's abstraction from LuceneDocument forces this, starting with KX11.
            // https://devnet.kentico.com/questions/search-index-stored-in-lower-case
            searchDocument.Add(name: SmartSearchColumnNameConstants.DocumentPath,
                               value: documentNamePath,
                               store: true,
                               tokenize: false);
            e.Content = string.Empty;
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
