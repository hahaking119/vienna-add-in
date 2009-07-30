using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class CDTLibrary : ElementLibrary<ICDT, CDTSpec>, ICDTLibrary
    {
        public CDTLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }
    }
}