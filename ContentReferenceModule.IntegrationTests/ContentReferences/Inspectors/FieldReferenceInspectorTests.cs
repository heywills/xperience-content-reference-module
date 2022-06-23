using System.Linq;
using CMS.DocumentEngine;
using NUnit.Framework;
using XperienceCommunity.ContentReferenceModule.Cms.Repositories;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Inspectors;

namespace ContentReferenceModule.IntegrationTests.ContentReferences.Inspectors
{
    [TestFixture]
    public class FieldReferenceInspectorTests
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
            var dataClassInfoRepository = new DataClassRepository();
            var fieldReferenceInspector = new FieldReferenceInspector(dataClassInfoRepository);
            var guidList = fieldReferenceInspector.GetPotentialContentReferences(treeNode);
            Assert.IsNotNull(guidList);

        }
    }
}
