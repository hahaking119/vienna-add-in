using System;
using System.Collections.Generic;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class DOCLibrary : BusinessLibrary, IDocLibrary
    {
        public DOCLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        #region IDocLibrary Members

        public IEnumerable<IAbie> RootElements
        {
            get { throw new NotImplementedException(); }
        }

        int IDocLibrary.Id
        {
            get { return Id.Value; }
        }

        public IEnumerable<IAbie> Elements
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public IAbie ElementByName(string name)
        {
            throw new NotImplementedException();
        }

        public IAbie CreateElement(AbieSpec spec)
        {
            throw new NotImplementedException();
        }

        public IAbie UpdateElement(IAbie element, AbieSpec spec)
        {
            throw new NotImplementedException();
        }
    }
}