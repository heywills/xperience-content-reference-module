using CMS.Core;
using ContentReferenceModule.IntegrationTests.TestHelpers;
using Moq;
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
        private readonly IEventLogService _eventLogService = new Mock<IEventLogService>().Object;

        [Test()]
        public void GetParentReferencesByNodeGuidAndCulture_With_Valid_NodeGuid_And_Culture_Returns_Results()
        {
            var smartSearchHelper = new SmartSearchHelper(_smartIndexSettings, _eventLogService);
            var contentReferenceService = new ContentReferenceService(smartSearchHelper, _eventLogService);
            var contentReferences = contentReferenceService.GetParentReferencesByNodeGuidAndCulture(new Guid("32d194af-76ab-435d-9175-71f8c099ded1"), "en-us"); 

            Assert.True(contentReferences != null && contentReferences.Count() != 0, "GetParentReferencesByNodeGuidAndCulture should have returned results");
        }
    }
}
