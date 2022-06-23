using System;
using System.Collections.Generic;
using CMS.DocumentEngine;

namespace XperienceCommunity.ContentReferenceModule.Cms.Core
{
    public interface ITreeNodeRepository
    {
        IEnumerable<TreeNode> GetTreeNodesByGuids(IEnumerable<Guid> guids, string culture, bool onlyPublished);
    }
}
