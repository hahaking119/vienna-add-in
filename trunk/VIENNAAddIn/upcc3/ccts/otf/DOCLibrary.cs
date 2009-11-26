using System;
using System.Collections.Generic;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class DOCLibrary : ElementLibrary<IAbie, AbieSpec>, IDOCLibrary
    {
        public DOCLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        #region IDOCLibrary Members

        public IEnumerable<IAbie> RootElements
        {
            get { throw new NotImplementedException(); }
        }

        int IDOCLibrary.Id
        {
            get { return Id.Value; }
        }

        #endregion
    }
}