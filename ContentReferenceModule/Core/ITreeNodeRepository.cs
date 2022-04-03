using CMS.Base;
using System;
using System.Collections.Generic;

namespace XperienceCommunity.ContentReferenceModule.Core
{
    interface ITreeNodeRepository
    {
        IEnumerable<ITreeNode> GetTreeNodesByGuids(IEnumerable<Guid> guids, string culture, bool onlyPublished);
    }
}
