using CMS.DocumentEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Core
{
    interface IContentInspectorService
    {
        public IEnumerable<Guid> GetContentReferences(TreeNode treeNode);
    }
}
