using CMS.Search;
using System.Collections.Generic;

namespace XperienceCommunity.ContentReferenceModule.SmartSearch.Core
{
    public interface ISmartSearchHelper
    {
        IEnumerable<SearchResultItem> GetSearchResultItemsByFieldTerm(string fieldName, string term, string culture);
    }
}
