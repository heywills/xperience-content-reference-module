using System;
using System.Collections.Generic;
using System.Text;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Core;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Models;

namespace ContentReferenceModule.IntegrationTests.TestHelpers
{
    public static class TestFactories
    {
        public static ISmartIndexSettings CreateTestSmartIndexSettings()
        {
            var smartIndexSettings = new SmartIndexSettings()
            {
                IndexName = "ContentReferenceModule_IntegrationTests",
                IndexDisplayName = "Content Reference Module (Integration Tests Index)"
            };
            return smartIndexSettings;
        }
    }
}
