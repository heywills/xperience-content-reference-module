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

        ISmartSearchHelper _smartSearchHelper;

        public ContentReferenceService(ISmartSearchHelper smartSearchHelper)
        {
            _smartSearchHelper = smartSearchHelper;
        }

        public IEnumerable<ContentReference> GetParentReferencesByNodeGuidAndCulture(Guid nodeGuid, string cultureCode)
        {
            var searchResultItems = _smartSearchHelper.GetSearchResultItemsByFieldTerm(ContentReferenceServiceConstants.IndexNodeReferencesFieldName,
                                                                                       nodeGuid.ToString(),
                                                                                       cultureCode);
            var contentReferences = searchResultItems.Select(x => CreateContentReferenceFromSearchResultItem(x));
            return contentReferences;
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
