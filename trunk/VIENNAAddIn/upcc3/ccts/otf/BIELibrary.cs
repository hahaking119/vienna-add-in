using System.Collections.Generic;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class BIELibrary : ElementLibrary<IABIE, ABIESpec>, IBIELibrary
    {
        public BIELibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        #region IBIELibrary Members

        int IBIELibrary.Id
        {
            get { return Id.Value; }
        }

        #endregion
    }
}