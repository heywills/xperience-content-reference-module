using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.DocumentEngine;
using NUnit.Framework;
using XperienceCommunity.ContentReferenceModule.Inspectors;
using XperienceCommunity.ContentReferenceModule.Repositories;

namespace ContentReferenceModule.IntegrationTests.Inspectors
{
    [TestFixture]
    public class FieldReferenceInspectorTests
    {
        [Test()]
        public void GetPotentialContentReferences_Returns_Discovered_Guids()
        {
            bool onlyPublished = false;
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
