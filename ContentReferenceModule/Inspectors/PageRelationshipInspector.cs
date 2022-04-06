using CMS.DocumentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using XperienceCommunity.ContentReferenceModule.Constants;
using XperienceCommunity.ContentReferenceModule.Core;

namespace XperienceCommunity.ContentReferenceModule.Inspectors
{
    public class PageRelationshipInspector : IReferenceInspector
    {

        public PageRelationshipInspector()
        {
        }

        public IEnumerable<Guid> GetPotentialContentReferences(TreeNode treeNode)
        {
            // TODO: Add parameter guard
            var returnList = treeNode.RelatedDocuments
                                     .All
                                     .Select(n => n.NodeGUID)
                                     .Distinct()
                                     .ToList();
            return returnList;
        }
    }
}
