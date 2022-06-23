using System.Linq;
using CMS.DocumentEngine;
using NUnit.Framework;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Inspectors;

namespace ContentReferenceModule.IntegrationTests.ContentReferences.Inspectors
{
    [TestFixture]
    public class PageRelationshipInspectorTests
    {
        [Test()]
        public void GetPotentialContentReferences_Returns_Discovered_Guids()
        {
            const bool onlyPublished = false;
            var treeNode = DocumentHelper.GetDocuments()
                .Path("/Home")
                .OnSite("DfwJobs.Web")
                .Culture("en-us")
                .WithCoupledColumns()
                .Published(onlyPublished)
                .LatestVersion(!onlyPublished)
                .FirstOrDefault();
            var pageRelationshipInspector = new PageRelationshipInspector();
            var guidList = pageRelationshipInspector.GetPotentialContentReferences(treeNode);
            Assert.IsNotNull(guidList);

        }
    }
}
