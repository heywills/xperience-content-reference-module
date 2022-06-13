using CMS.Search;
using System;
using XperienceCommunity.ContentReferenceModule.Constants;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Core;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Models;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Search;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Factories
{
    public class ContentReferenceFactory : IContentReferenceFactory
    {
        public ContentReference CreateContentReferenceFromSearchResultItem(SearchResultItem searchResultItem)
        {
            return new ContentReference()
            {
                DocumentName = searchResultItem.GetSearchString(SmartSearchColumnNameConstants.DocumentName),
                DocumentCulture = searchResultItem.GetSearchString(SmartSearchColumnNameConstants.DocumentCulture),
                NodeAliasPath = searchResultItem.GetSearchString(SmartSearchColumnNameConstants.NodeAliasPath),
                DocumentPath = searchResultItem.GetSearchString(SmartSearchColumnNameConstants.DocumentPath),
                DocumentGuid = searchResultItem.GetSearchGuid(SmartSearchColumnNameConstants.DocumentGuid),
                NodeGuid = searchResultItem.GetSearchGuid(SmartSearchColumnNameConstants.NodeGuid),
                NodeID = searchResultItem.GetSearchInt(SmartSearchColumnNameConstants.NodeId)
            };
        }

    }
}
