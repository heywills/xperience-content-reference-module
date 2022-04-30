using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Base;
using CMS.DocumentEngine;
using XperienceCommunity.ContentReferenceModule.Cms.Core;
using XperienceCommunity.ContentReferenceModule.ContentReferences.Core;
using XperienceCommunity.ContentReferenceModule.Helpers;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Inspectors
{
    public class FieldReferenceInspector : IReferenceInspector
    {
        private readonly IDataClassRepository _dataClassRepository;

        public FieldReferenceInspector(IDataClassRepository dataClassRepository)
        {
            _dataClassRepository = dataClassRepository;
        }

        public IEnumerable<Guid> GetPotentialContentReferences(TreeNode treeNode)
        {
            // TODO: Add parameter guard
            var returnList = GetAllGuidReferences(treeNode);
            return returnList;
        }

        private IEnumerable<Guid> GetAllGuidReferences(TreeNode treeNode)
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
            returnList = returnList.Union(guidsFromStringColumns)
                      .Union(guidsFromGuidColumns)
                      .Distinct()
                      .ToList();
            return returnList;
        }

        private IEnumerable<Guid> GetGuidsFromTextColumn(TreeNode treeNode, string columnName)
        {
            // TODO: Consider that some of text columns might have huge values. Consider
            // how to detect if its a value that contains delimited guids before calling Guidify
            var returnList = new List<Guid>();
            if (!(treeNode.TryGetValue(columnName, out var columnValue) &&
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
