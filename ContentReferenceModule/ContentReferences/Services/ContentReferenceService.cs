using CMS.Core;
using CMS.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XperienceCommunity.ContentReferenceModule.Constants;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Core;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Models;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Core;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Services
{
    public class ContentReferenceService : IContentReferenceService
    {

        private readonly ISmartSearchHelper _smartSearchHelper;
        private readonly IEventLogService _eventLogService;


        public ContentReferenceService(ISmartSearchHelper smartSearchHelper,
                                       IEventLogService eventLogService)
        {
            _smartSearchHelper = smartSearchHelper;
            _eventLogService = eventLogService;

        }

        public IEnumerable<ContentReference> GetParentReferencesByNodeGuidAndCulture(Guid nodeGuid, string cultureCode)
        {
            try
            {
                var searchResultItems = _smartSearchHelper.GetSearchResultItemsByFieldTerm(ContentReferenceServiceConstants.IndexNodeReferencesFieldName,
                                                                                           nodeGuid.ToString(),
                                                                                           cultureCode);
                var contentReferences = searchResultItems.Select(x => CreateContentReferenceFromSearchResultItem(x));
                return contentReferences;
            }
            catch (Exception ex)
            {
                _eventLogService.LogError(nameof(ContentReferenceModule),
                                          "GetReferences",
                                          $"An unexpected exception occured when querying the Smart Search index.\r\n" +
                                                $"NodeGuid: {nodeGuid}\r\n" +
                                                $"Culture: {cultureCode}\r\n\r\n" +
                                                $"Exception:\r\n" +
                                                ex.ToString());
                return Enumerable.Empty<ContentReference>();
            }

        }

        internal ContentReference CreateContentReferenceFromSearchResultItem(SearchResultItem searchResultItem)
        {
            var documentName = searchResultItem.GetSearchValue(SmartSearchColumnNameConstants.DocumentName)?.ToString();
            var documentGuid = searchResultItem.GetSearchValue(SmartSearchColumnNameConstants.DocumentGuid)?.ToString();
            var documentCulture = searchResultItem.GetSearchValue(SmartSearchColumnNameConstants.DocumentCulture)?.ToString();

            var nodeId = Convert.ToInt32(searchResultItem.GetSearchValue(SmartSearchColumnNameConstants.NodeId));
            var nodeGuid = searchResultItem.GetSearchValue(SmartSearchColumnNameConstants.NodeGuid)?.ToString();
            var nodeAliasPath = searchResultItem.GetSearchValue(SmartSearchColumnNameConstants.NodeAliasPath)?.ToString();
            var documentPath = searchResultItem.GetSearchValue(SmartSearchColumnNameConstants.DocumentPath)?.ToString();


            return new ContentReference()
            {
                NodeID = nodeId
            };

        }
    }
}
