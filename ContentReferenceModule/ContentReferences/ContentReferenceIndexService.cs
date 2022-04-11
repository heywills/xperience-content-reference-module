using XperienceCommunity.ContentReferenceModule.SmartSearch.Core;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences
{
    public class ContentReferenceIndexService
    {
        private readonly ISmartIndexConfigurationManager _smartIndexConfigurationManager;
        private readonly ISmartIndexSettings _smartIndexSettings;

        public ContentReferenceIndexService(ISmartIndexConfigurationManager smartIndexConfigurationManager,
                                            ISmartIndexSettings smartIndexSettings)
        {
            _smartIndexConfigurationManager = smartIndexConfigurationManager;
            _smartIndexSettings = smartIndexSettings;
        }

        public void Initialize()
        {
            _smartIndexConfigurationManager.Initialize(_smartIndexSettings);
        }
    }
}
