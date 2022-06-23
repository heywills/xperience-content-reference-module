using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Search;

namespace XperienceCommunity.ContentReferenceModule.SmartSearch.Index
{
    public static class SearchIndexInfoFactory
    {
        /// <summary>
        /// Create a SearchIndexInfo for pages using the WhiteSpaceAnalyzer
        /// </summary>
        /// <param name="indexName">The code name of the index</param>
        /// <param name="indexDisplayName">The display name of the index. Optional. If not provided the index name will be used.</param>
        /// <param name="searchAnalyzerType">The analyzer type. The default is WhiteSpaceAnalyzer because it will allow each GUID in the index to be treated as a separate index term.</param>
        /// <returns>An initialized SearchIndexInfo object</returns>
        public static SearchIndexInfo Create(string indexName,
                                             string indexDisplayName = null,
                                             SearchAnalyzerTypeEnum searchAnalyzerType = SearchAnalyzerTypeEnum.WhiteSpaceAnalyzer)
        {
            var searchIndexInfo = new SearchIndexInfo
            {
                IndexName = indexName,
                IndexDisplayName = indexDisplayName ?? indexName,
                IndexAnalyzerType = searchAnalyzerType,
                IndexType = TreeNode.OBJECT_TYPE,
                IndexProvider = SearchIndexInfo.LUCENE_SEARCH_PROVIDER
            };
            var searchIndexSettings = SearchIndexSettingsFactory.CreateIndexSettingsForAllPages();
            searchIndexInfo.IndexSettings = searchIndexSettings;
            return searchIndexInfo;
        }
    }
}
