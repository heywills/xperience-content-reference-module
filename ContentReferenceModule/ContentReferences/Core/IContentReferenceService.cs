using CMS.Base;
using System;
using System.Collections.Generic;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Models;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Core
{
    public interface IContentReferenceService
    {
        IEnumerable<ContentReference> GetParentReferences(ITreeNode node);

        IEnumerable<ContentReference> GetParentReferences(Guid nodeGuid, string cultureCode);
    }
}