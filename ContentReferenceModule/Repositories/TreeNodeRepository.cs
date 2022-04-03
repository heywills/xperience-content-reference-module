using CMS.Base;
using CMS.DocumentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XperienceCommunity.ContentReferenceModule.Constants;
using XperienceCommunity.ContentReferenceModule.Core;

namespace XperienceCommunity.ContentReferenceModule.Repositories
{
    public class TreeNodeRepository: ITreeNodeRepository
    {
        public IEnumerable<ITreeNode> GetTreeNodesByGuids(IEnumerable<Guid> guids, string culture, bool onlyPublished)
        {
            return DocumentHelper.GetDocuments()
                .WhereIn(TreeNodeConstants.NodeGUID, guids.ToList())
                .Culture(culture)
                .Published(onlyPublished)
                .LatestVersion(!onlyPublished)
                .WithCoupledColumns()
                .ToList<ITreeNode>();
        }
    }
}
