using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using XperienceCommunity.ContentReferenceModule.Cms.Core;
using XperienceCommunity.ContentReferenceModule.Constants;
using XperienceCommunity.ContentReferenceModule.Helpers;

namespace XperienceCommunity.ContentReferenceModule.Cms.Repositories
{
    public class TreeNodeRepository: ITreeNodeRepository
    {
        public IEnumerable<TreeNode> GetTreeNodesByGuids(IEnumerable<Guid> guids, string culture, bool onlyPublished)
        {
            Guard.ArgumentNotNullOrEmpty(culture, nameof(culture));
            Guard.ArgumentNotNull(guids, nameof(guids));
            if(guids.Count() == 0)
            {
                return Enumerable.Empty<TreeNode>();
            }

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
