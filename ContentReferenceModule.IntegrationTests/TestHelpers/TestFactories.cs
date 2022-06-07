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
                IndexName = "ContentReferenceModuleIndex",
                IndexDisplayName = "Content Reference Module"
            };
            return smartIndexSettings;
        }
    }
}
