using CMS.Base;
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
        private readonly IContentReferenceFactory _contentReferenceFactory;
        private readonly IEventLogService _eventLogService;


        public ContentReferenceService(ISmartSearchHelper smartSearchHelper,
                                       IContentReferenceFactory contentReferenceFactory,
                                       IEventLogService eventLogService)
        {
            _smartSearchHelper = smartSearchHelper;
            _contentReferenceFactory = contentReferenceFactory;
            _eventLogService = eventLogService;

        }

        public IEnumerable<ContentReference> GetParentReferencesByNode(ITreeNode node)
        {
            return GetParentReferencesByNodeGuidAndCulture(node.NodeGUID, node.DocumentCulture);
        }

        public IEnumerable<ContentReference> GetParentReferencesByNodeGuidAndCulture(Guid nodeGuid, string cultureCode)
        {
            try
            {
                var searchResultItems = _smartSearchHelper.GetSearchResultItemsByFieldTerm(ContentReferenceServiceConstants.IndexNodeReferencesFieldName,
                                                                                           nodeGuid.ToString(),
                                                                                           cultureCode);
                var contentReferences = searchResultItems.Select(x => _contentReferenceFactory.CreateContentReferenceFromSearchResultItem(x))
                                                         .ToList();
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
    }
}
