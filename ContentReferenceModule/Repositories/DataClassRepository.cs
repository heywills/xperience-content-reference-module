using CMS.DataEngine;
using XperienceCommunity.ContentReferenceModule.Core;

namespace XperienceCommunity.ContentReferenceModule.Repositories
{
    public class DataClassRepository : IDataClassRepository
    {
        public ClassStructureInfo GetClassStructureInfo(string className)
        {
            // TODO: Add parameter guard
            var dataClassInfo = DataClassInfoProviderBase<DataClassInfoProvider>.GetDataClassInfo(className);
            var classStructureInfo = new ClassStructureInfo(dataClassInfo.ClassName, dataClassInfo.ClassXmlSchema, dataClassInfo.ClassTableName);
            return classStructureInfo;
        }
    }
}
