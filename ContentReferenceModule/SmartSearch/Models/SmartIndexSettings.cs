using XperienceCommunity.ContentReferenceModule.SmartSearch.Core;

namespace XperienceCommunity.ContentReferenceModule.SmartSearch.Models
{
    public class SmartIndexSettings : ISmartIndexSettings
    {
        public string IndexName { get; set; }

        public string IndexDisplayName { get; set; }
    }
}
