using System;
using System.Collections.Generic;
using CctsRepository.EnumLibrary;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class ENUMLibrary : BusinessLibrary, IEnumLibrary
    {
        public ENUMLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        #region IEnumLibrary Members

        int IEnumLibrary.Id
        {
            get { return Id.Value; }
        }

        public IEnumerable<IEnum> Elements
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public IEnum ElementByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnum CreateElement(EnumSpec spec)
        {
            throw new NotImplementedException();
        }

        public IEnum UpdateElement(IEnum element, EnumSpec spec)
        {
            throw new NotImplementedException();
        }
    }
}