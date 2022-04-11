using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using XperienceCommunity.ContentReferenceModule.Cms.Core;
using XperienceCommunity.ContentReferenceModule.Constants;

namespace XperienceCommunity.ContentReferenceModule.Cms.Repositories
{
    public class TreeNodeRepository: ITreeNodeRepository
    {
        public IEnumerable<TreeNode> GetTreeNodesByGuids(IEnumerable<Guid> guids, string culture, bool onlyPublished)
        {
            return DocumentHelper.GetDocuments()
                .WhereIn(TreeNodeFieldNameConstants.NodeGuid, guids.ToList())
                .Culture(culture)
                .Published(onlyPublished)
                .LatestVersion(!onlyPublished)
                .WithCoupledColumns()
                .ToList();
        }
    }
}
