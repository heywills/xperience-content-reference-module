using CMS.DataEngine;

namespace XperienceCommunity.ContentReferenceModule.Cms.Core
{
    public interface IDataClassRepository
    {
        ClassStructureInfo GetClassStructureInfo(string className);
    }
}
