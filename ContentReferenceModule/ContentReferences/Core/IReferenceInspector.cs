using System;
using System.Collections.Generic;
using CMS.DocumentEngine;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Core
{
    /// <summary>
    /// Supports various implementations capable of inspecting an Xperience page for content
    /// references, and returning a list of discovered guids. The guids are not guaranteed to
    /// be NodeGuids, so its necessary to validate them after all IReferenceInspecfors are used
    /// to create a list of guids.
    /// </summary>
    public interface IReferenceInspector
    {
        IEnumerable<Guid> GetPotentialContentReferences(TreeNode treeNode);
    }
}
