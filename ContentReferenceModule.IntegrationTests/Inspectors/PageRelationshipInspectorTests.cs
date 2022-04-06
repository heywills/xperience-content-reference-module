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
    public class PageRelationshipInspectorTests
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
            var pageRelationshipInspector = new PageRelationshipInspector();
            var guidList = pageRelationshipInspector.GetPotentialContentReferences(treeNode);
            Assert.IsNotNull(guidList);

        }
    }
}
