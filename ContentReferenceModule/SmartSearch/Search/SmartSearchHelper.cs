using CMS.Helpers;
using CMS.Membership;
using CMS.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XperienceCommunity.ContentReferenceModule.Helpers;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Core;

namespace XperienceCommunity.ContentReferenceModule.SmartSearch.Search
{
    public class SmartSearchHelper : ISmartSearchHelper
    {
        private readonly ISmartIndexSettings _smartIndexSettings;
        private SearchIndexInfo _searchIndexInfo;

        public SmartSearchHelper(ISmartIndexSettings smartIndexSettings)
        {
            _smartIndexSettings = smartIndexSettings;

        }

        public IEnumerable<SearchResultItem> GetSearchResultItemsByFieldTerm(string fieldName,
                                                                             string term,
                                                                             string cultureCode)
        {
            Guard.ArgumentNotNull(fieldName);
            Guard.ArgumentNotNull(term);
            var searchIndexInfo = GetSearchIndexInfo();
            if(searchIndexInfo == null)
            {
                // TODO: Log missing index
                return Enumerable.Empty<SearchResultItem>();
            }
            var searchParameters = CreateSearchParameters(searchIndexInfo.IndexName, fieldName, term, cultureCode);
            var searchResult = SearchHelper.Search(searchParameters);
            var searchResultItems = searchResult.Items;
            if(searchResultItems == null)
            {
                return Enumerable.Empty<SearchResultItem>();
            }
            return searchResultItems;
        }

        public SearchParameters CreateSearchParameters(string indexName,
                                                       string fieldName,
                                                       string term,
                                                       string culture)
        {
            var searchExpression = $"{fieldName.ToLowerInvariant()}:{term}";

            return new SearchParameters
            {
                Path = "/%",
                DefaultCulture = CultureHelper.EnglishCulture.IetfLanguageTag,
                MaxScore = 99999,
                NumberOfProcessedResults = 100,
                DisplayResults = 100,
                AttachmentOrderBy = string.Empty,
                AttachmentWhere = string.Empty,
                NumberOfResults = 0,
                StartingPosition = 0,
                CurrentCulture = culture,
                User = MembershipContext.AuthenticatedUser,
                SearchIndexes = indexName,
                SearchFor = searchExpression,
                CombineWithDefaultCulture = false,
                ClassNames = string.Empty,
                SearchInAttachments = false
            };
        }


        private SearchIndexInfo GetSearchIndexInfo()
        {
            if(_searchIndexInfo == null)
            {
                _searchIndexInfo = SearchIndexInfoProvider.GetSearchIndexInfo(_smartIndexSettings.IndexName);
            }
            return _searchIndexInfo;
        }
    }
}
