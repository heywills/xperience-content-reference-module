using CMS.DocumentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XperienceCommunity.ContentReferenceModule.Cms.Extensions
{
    public static class TreeNodeExtensions
    {
        public static string CreateDocumentNamePath(this TreeNode treeNode)
        {
            var documentsOnPath = treeNode.DocumentsOnPath;
            var documentNamesInOrder = documentsOnPath.OrderBy(n => n.NodeAliasPath.Length)
                                                      .Select(n => n.DocumentName);
            var documentNamePath = string.Join("/", documentNamesInOrder);
            return documentNamePath;
        }
    }
}
