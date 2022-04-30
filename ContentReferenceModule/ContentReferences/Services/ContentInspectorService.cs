using CMS.DocumentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using XperienceCommunity.ContentReferenceModule.Cms.Core;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Core;
using XperienceCommunity.ContentReferenceModule.Helpers;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Services
{
    /// <summary>
    /// Uses the list of registered IReferenceInspectors to find all nodes
    /// referenced by a treenode.
    /// </summary>
    public class ContentInspectorService : IContentInspectorService
    {
        IReferenceInspector[] _referenceInspectors;
        ITreeNodeRepository _treeNodeRepository;

        public ContentInspectorService(IReferenceInspector[] referenceInspectors,
                                       ITreeNodeRepository treeNodeRepository)
        {
            _referenceInspectors = referenceInspectors;
            _treeNodeRepository = treeNodeRepository;
        }

        /// <summary>
        /// Uses the list of registered IReferenceInspectors to find all nodes
        /// referenced by a treenode. Ensures the list is unique and that each
        /// node exists in the TreeNode's culture.
        /// </summary>
        /// <param name="treeNode"></param>
        public IEnumerable<Guid> GetContentReferences(TreeNode treeNode)
        {
            Guard.ArgumentNotNull(treeNode);
            var contentReferences = _referenceInspectors
                                        .SelectMany(i => i.GetPotentialContentReferences(treeNode))
                                        .Distinct();
            var treeNodes = _treeNodeRepository.GetTreeNodesByGuids(contentReferences,
                                                                    treeNode.DocumentCulture,
                                                                    true);
            var validatedContentReferences = treeNodes.Select(t => t.NodeGUID);
            return validatedContentReferences;
        }
    }
}
