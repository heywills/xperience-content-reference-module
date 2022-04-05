using CMS.DataEngine;

namespace XperienceCommunity.ContentReferenceModule.Core
{
    public interface IDataClassRepository
    {
        ClassStructureInfo GetClassStructureInfo(string className);
    }
}
