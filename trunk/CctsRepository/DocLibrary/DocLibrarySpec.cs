// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace CctsRepository.DocLibrary
{
    public class DocLibrarySpec
    {
        public DocLibrarySpec(IDOCLibrary docLibrary)
        {
            Name = docLibrary.Name;
            Status = docLibrary.Status;
            UniqueIdentifier = docLibrary.UniqueIdentifier;
            VersionIdentifier = docLibrary.VersionIdentifier;
            BaseURN = docLibrary.BaseURN;
            NamespacePrefix = docLibrary.NamespacePrefix;
            BusinessTerms = new List<string>(docLibrary.BusinessTerms);
            Copyrights = new List<string>(docLibrary.Copyrights);
            Owners = new List<string>(docLibrary.Owners);
            References = new List<string>(docLibrary.References);
        }

        public DocLibrarySpec()
        {
        }

        public string Name { get; set; }

        public string Status { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string BaseURN { get; set; }
        public string NamespacePrefix { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }
        public IEnumerable<string> Copyrights { get; set; }
        public IEnumerable<string> Owners { get; set; }
        public IEnumerable<string> References { get; set; }
    }
}