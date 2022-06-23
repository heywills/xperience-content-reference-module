using CMS.Search;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Models;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Core
{
    public interface IContentReferenceFactory
    {
        ContentReference CreateContentReferenceFromSearchResultItem(SearchResultItem searchResultItem);
    }
}