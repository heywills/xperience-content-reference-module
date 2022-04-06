using CMS.DocumentEngine;
using System;
using System.Collections.Generic;

namespace XperienceCommunity.ContentReferenceModule.Core
{
    interface ITreeNodeRepository
    {
        IEnumerable<TreeNode> GetTreeNodesByGuids(IEnumerable<Guid> guids, string culture, bool onlyPublished);
    }
}
