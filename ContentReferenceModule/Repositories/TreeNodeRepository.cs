using CMS.DocumentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using XperienceCommunity.ContentReferenceModule.Constants;
using XperienceCommunity.ContentReferenceModule.Core;

namespace XperienceCommunity.ContentReferenceModule.Repositories
{
    public class TreeNodeRepository: ITreeNodeRepository
    {
        public IEnumerable<TreeNode> GetTreeNodesByGuids(IEnumerable<Guid> guids, string culture, bool onlyPublished)
        {
            return DocumentHelper.GetDocuments()
                .WhereIn(TreeNodeFieldNameConstants.NodeGUID, guids.ToList())
                .Culture(culture)
                .Published(onlyPublished)
                .LatestVersion(!onlyPublished)
                .WithCoupledColumns()
                .ToList<TreeNode>();
        }
    }
}
