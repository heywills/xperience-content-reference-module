using CMS.Core;
using ContentReferenceModule.IntegrationTests.TestHelpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Core;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Factories;
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
        private readonly IContentReferenceFactory _contentReferenceFactory = new ContentReferenceFactory();
        private readonly IEventLogService _eventLogService = new Mock<IEventLogService>().Object;

        [Test()]
        public void GetParentReferencesByNodeGuidAndCulture_With_Valid_NodeGuid_And_Culture_Returns_Results()
        {
            var smartSearchHelper = new SmartSearchHelper(_smartIndexSettings, _eventLogService);
            var contentReferenceService = new ContentReferenceService(smartSearchHelper, _contentReferenceFactory, _eventLogService);
            var contentReferences = contentReferenceService.GetParentReferences(new Guid("32d194af-76ab-435d-9175-71F8C099DED1"), "EN-us"); 

            Assert.True(contentReferences != null && contentReferences.Count() != 0, "GetParentReferencesByNodeGuidAndCulture should have returned results");
        }
    }
}
