using CMS.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XperienceCommunity.ContentReferenceModule.SmartSearch.Search
{
    public static class SearchResultItemExtensions
    {
        public static string GetSearchString(this SearchResultItem searchResultItem, string fieldName)
        {
            var value = searchResultItem.GetSearchValue(fieldName);
            return value?.ToString() ?? string.Empty;
        }
        public static int? GetSearchInt(this SearchResultItem searchResultItem, string fieldName)
        {
            var value = searchResultItem.GetSearchValue(fieldName);
            if(int.TryParse(value?.ToString(), out int result))
            {
                return result;
            }

            return null;
        }
        public static Guid? GetSearchGuid(this SearchResultItem searchResultItem, string fieldName)
        {
            var value = searchResultItem.GetSearchValue(fieldName);
            if (Guid.TryParse(value?.ToString(), out Guid result))
            {
                return result;
            }
            return null;
        }
    }
}
