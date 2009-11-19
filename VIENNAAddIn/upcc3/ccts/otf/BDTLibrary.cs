using System.Collections.Generic;
using CctsRepository;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class BDTLibrary : ElementLibrary<IBDT, BDTSpec>, IBDTLibrary
    {
        public BDTLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }
    }
}