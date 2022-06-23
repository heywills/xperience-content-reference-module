using System;

namespace XperienceCommunity.ContentReferenceModule.ContentReferences.Models
{
    /// <summary>
    /// Content reference representing a Xperience document/content item
    /// </summary>
    public class ContentReference
    {
        /// <summary>
        /// The display name of the document
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// The unique Document ID
        /// </summary>
        public int? DocumentID { get; set; }

        /// <summary>
        /// The GUID for the document
        /// </summary>
        public Guid? DocumentGuid { get; set; }

        /// <summary>
        /// The document's culture code.
        /// </summary>
        public string DocumentCulture { get; set; }

        /// <summary>
        /// The unique Node ID
        /// </summary>
        public int? NodeID { get; set; }

        /// <summary>
        /// The GUID for the node
        /// </summary>
        public Guid? NodeGuid { get; set; }

        /// <summary>
        /// The node alias path of the document
        /// </summary>
        public string NodeAliasPath { get; set; }

        /// <summary>
        /// The friendly tree path of the document
        /// </summary>
        public string DocumentPath { get; set; }
    }
}
