using System.Collections.Generic;
using CctsRepository.cc;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class CCLibrary : ElementLibrary<IACC, ACCSpec>, ICCLibrary
    {
        public CCLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }
    }
}