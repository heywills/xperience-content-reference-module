using XperienceCommunity.ContentReferenceModule.Core;

namespace XperienceCommunity.ContentReferenceModule.Models
{
    public class SmartIndexSettings : ISmartIndexSettings
    {
        public string IndexName { get; set; }

        public string IndexDisplayName { get; set; }
    }
}
