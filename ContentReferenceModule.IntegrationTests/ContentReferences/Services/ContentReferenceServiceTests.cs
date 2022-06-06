using ContentReferenceModule.IntegrationTests.TestHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Services;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Core;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Models;
using XperienceCommunity.ContentReferenceModule.SmartSearch.Search;

namespace ContentReferenceModule.IntegrationTests.ContentReferences.Services
{
    [TestFixture]
    public class ContentReferenceServiceTests
    {
        private readonly ISmartIndexSettings _smartIndexSettings = TestFactories.CreateTestSmartIndexSettings();

        [Test()]
        public void GetParentReferencesByNodeGuidAndCulture_With_Valid_NodeGuid_And_Culture_Returns_Results()
        {
            var smartSearchHelper = new SmartSearchHelper(_smartIndexSettings);
            var contentReferenceService = new ContentReferenceService(smartSearchHelper);
            var contentReferences = contentReferenceService.GetParentReferencesByNodeGuidAndCulture(new Guid("c462b63b-cd96-4123-ae6a-8695411ba6b8"), "en-us"); // Jimmy Dimmick

            Assert.True(contentReferences != null && contentReferences.Count() != 0, "GetParentReferencesByNodeGuidAndCulture should have returned results");
        }
    }
}
