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
    public class WidgetReferenceInspectorTests
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
            var widgetReferenceInspector = new WidgetReferenceInspector(dataClassInfoRepository);
            var guidList = widgetReferenceInspector.GetPotentialContentReferences(treeNode);
            Assert.IsNotNull(guidList);

        }
    }
}
