using System;
using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class CDTLibrary : BusinessLibrary, ICdtLibrary
    {
        public CDTLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        #region ICdtLibrary Members

        int ICdtLibrary.Id
        {
            get { return Id.Value; }
        }

        public IEnumerable<ICdt> Elements
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public ICdt ElementByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICdt CreateElement(CdtSpec spec)
        {
            throw new NotImplementedException();
        }

        public ICdt UpdateElement(ICdt element, CdtSpec spec)
        {
            throw new NotImplementedException();
        }
    }
}