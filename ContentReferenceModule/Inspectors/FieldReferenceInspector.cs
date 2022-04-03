using CMS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XperienceCommunity.ContentReferenceModule.Core;
using XperienceCommunity.ContentReferenceModule.Extensions;

namespace XperienceCommunity.ContentReferenceModule.Inspectors
{
    public class FieldReferenceInspector : IReferenceInspector
    {
        public IEnumerable<Guid> GetPotentialContentReferences(ITreeNode treeNode)
        {
            var returnList = GetAllGuidReferences(treeNode);

            throw new NotImplementedException();
        }

        private IEnumerable<Guid> GetAllGuidReferences(ITreeNode treeNode)
        {
            var returnList = new List<Guid>();
            var columnNames = treeNode.ColumnNames;
            foreach (var columnName in columnNames)
            {
                object columnValue;
                if (!(treeNode.TryGetValue(columnName, out columnValue) &&
                    columnValue is string))
                {
                    break;
                }
                var guidList = ((string)columnValue).Guidify();
                returnList.AddRange(guidList);
            }
            return returnList;
        }
    }
}
