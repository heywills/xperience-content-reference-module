using CMS.Core;
using CMS.Helpers;
using CMS.Membership;
using CMS.Search;
using System.Collections.Generic;
using System.Linq;
using XperienceCommunity.ContentReferenceModule.Helpers;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Core;

namespace XperienceCommunity.ContentReferenceModule.SmartSearch.Search
{
    public class SmartSearchHelper : ISmartSearchHelper
    {
        private readonly ISmartIndexSettings _smartIndexSettings;
        private readonly IEventLogService _eventLogService;
        private SearchIndexInfo _searchIndexInfo;

        public SmartSearchHelper(ISmartIndexSettings smartIndexSettings,
                                 IEventLogService eventLogService)
        {
            _smartIndexSettings = smartIndexSettings;
            _eventLogService = eventLogService;
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
            if(_searchIndexInfo == null)
            {
                _eventLogService.LogError(nameof(ContentReferenceModule),
                                          "GetSearchIndex",
                                          $"The Smart Search index, {_smartIndexSettings.IndexDisplayName} ({_smartIndexSettings.IndexName}), could not be found.\r\n" +
                                           "Check the following:\r\n" +
                                           " - The index was successfully created when this module was loaded.\r\n" +
                                           " - It was not deleted after the module was loaded.\r\n" +
                                           " - It's name was not changed." +
                                           " - Web Farm synchronization is syncronizing smart indexes to all members of the farm.");
            }
            return _searchIndexInfo;
        }
    }
}
