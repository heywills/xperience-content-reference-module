using CMS.Base;
using System;
using System.Collections.Generic;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Models;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Core
{
    public interface IContentReferenceService
    {
        IEnumerable<ContentReference> GetParentReferencesByNode(ITreeNode node);

        IEnumerable<ContentReference> GetParentReferencesByNodeGuidAndCulture(Guid nodeGuid, string cultureCode);
    }
}