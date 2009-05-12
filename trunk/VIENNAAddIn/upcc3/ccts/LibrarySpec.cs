// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class LibrarySpec : CCTSSpec
    {
        public LibrarySpec(IBusinessLibrary businessLibrary)
        {
            Name = businessLibrary.Name;
            Status = businessLibrary.Status;
            UniqueIdentifier = businessLibrary.UniqueIdentifier;
            VersionIdentifier = businessLibrary.VersionIdentifier;
            BaseURN = businessLibrary.BaseURN;
            NamespacePrefix = businessLibrary.NamespacePrefix;
            BusinessTerms = new List<string>(businessLibrary.BusinessTerms);
            Copyrights = new List<string>(businessLibrary.Copyrights);
            Owners = new List<string>(businessLibrary.Owners);
            References = new List<string>(businessLibrary.References);
        }

        public LibrarySpec()
        {
        }

        public string Name { get; set; }

        [TaggedValue]
        public string Status { get; set; }

        [TaggedValue]
        public string UniqueIdentifier { get; set; }

        [TaggedValue]
        public string VersionIdentifier { get; set; }

        [TaggedValue]
        public string BaseURN { get; set; }

        [TaggedValue]
        public string NamespacePrefix { get; set; }

        [TaggedValue]
        public IEnumerable<string> BusinessTerms { get; set; }

        [TaggedValue]
        public IEnumerable<string> Copyrights { get; set; }

        [TaggedValue]
        public IEnumerable<string> Owners { get; set; }

        [TaggedValue]
        public IEnumerable<string> References { get; set; }
    }
}