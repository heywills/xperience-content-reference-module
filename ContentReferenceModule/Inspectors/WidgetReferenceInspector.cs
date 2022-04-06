using CMS.DocumentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using XperienceCommunity.ContentReferenceModule.Constants;
using XperienceCommunity.ContentReferenceModule.Core;

namespace XperienceCommunity.ContentReferenceModule.Inspectors
{
    public class WidgetReferenceInspector : IReferenceInspector
    {

        public WidgetReferenceInspector()
        {
        }

        public IEnumerable<Guid> GetPotentialContentReferences(TreeNode treeNode)
        {
            // TODO: Add parameter guard
            var returnList = GetAllGuidReferences(treeNode);
            return returnList;
        }

        private IEnumerable<Guid> GetAllGuidReferences(TreeNode treeNode)
        {
            var documentPageBuilderWidgets = treeNode.GetValue(TreeNodeFieldNameConstants.DocumentPageBuilderWidgets, string.Empty);
            var guidRegex = new Regex(RegexConstants.GuidPattern);
            var guidMatches = guidRegex.Matches(documentPageBuilderWidgets);
            var guids = guidMatches
                             .Select(m => Guid.TryParse(m.Value, out var g) ? g : Guid.Empty)
                             .Where(g => g != Guid.Empty)
                             .Distinct()
                             .ToList();
            return guids;
        }
    }
}
