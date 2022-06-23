using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Core;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Inspectors
{
    public class PageRelationshipInspector : IReferenceInspector
    {
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
