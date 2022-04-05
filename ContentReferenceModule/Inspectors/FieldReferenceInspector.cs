using CMS.Base;
using CMS.DataEngine;
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
        IDataClassRepository _dataClassRepository;

        public FieldReferenceInspector(IDataClassRepository dataClassRepository)
        {
            _dataClassRepository = dataClassRepository;
        }

        public IEnumerable<Guid> GetPotentialContentReferences(ITreeNode treeNode)
        {
            // TODO: Add parameter guard
            var returnList = GetAllGuidReferences(treeNode);
            return returnList;
        }

        private IEnumerable<Guid> GetAllGuidReferences(ITreeNode treeNode)
        {
            var returnList = new List<Guid>();
            var classStructureInfo = _dataClassRepository.GetClassStructureInfo(treeNode.ClassName);
            // TODO: Validate classStructureInfo
            var columnDefinitions = classStructureInfo.ColumnDefinitions;
            var guidsFromStringColumns = columnDefinitions.Where(c => c.ColumnType == typeof(string))
                             .ToList()
                             .SelectMany(c => GetGuidsFromTextColumn(treeNode, c.ColumnName))
                             .ToList();
            var guidsFromGuidColumns = columnDefinitions.Where(c => c.ColumnType == typeof(Guid))
                             .ToList()
                             .Select(c => treeNode.GetValue(c.ColumnName)?.ToGuid(Guid.Empty))
                             .Where(g => (g.HasValue && (g != Guid.Empty)))
                             .Select(g => g.Value);
            returnList.Union(guidsFromStringColumns)
                      .Union(guidsFromGuidColumns);
            return returnList;
        }

        private IEnumerable<Guid> GetGuidsFromTextColumn(ITreeNode treeNode, string columnName)
        {
            // TODO: Consider that some of text columns might have huge values. Consider
            // how to detect if its a value that contains delimited guids before calling Guidify
            var returnList = new List<Guid>();
            object columnValue;
            if (!(treeNode.TryGetValue(columnName, out columnValue) &&
                columnValue is string))
            {
                return returnList;
            }
            var guidList = ((string)columnValue).Guidify();
            returnList.AddRange(guidList);
            return returnList;
        }
    }
}
