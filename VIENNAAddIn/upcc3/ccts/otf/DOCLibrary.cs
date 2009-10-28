using System;
using System.Collections.Generic;
using UPCCRepositoryInterface;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class DOCLibrary : ElementLibrary<IABIE, ABIESpec>, IDOCLibrary
    {
        #region IDOCLibrary Members

        public DOCLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        public IEnumerable<IABIE> RootElements
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}