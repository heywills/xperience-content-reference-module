using System;
using System.Collections.Generic;
using System.Text;

namespace XperienceCommunity.ContentReferenceModule.Core
{
    public interface ISmartIndexSettings
    {
        string IndexName { get; }

        string IndexDisplayName { get; }
    }
}
