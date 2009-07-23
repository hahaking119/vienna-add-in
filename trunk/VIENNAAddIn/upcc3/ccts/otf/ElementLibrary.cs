using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public abstract class ElementLibrary<TElement, TElementSpec> : BusinessLibrary
    {
        protected ElementLibrary(int id, string name, int parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references) : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        public IEnumerable<TElement> Elements
        {
            get { throw new NotImplementedException(); }
        }

        public TElement ElementByName(string name)
        {
            throw new NotImplementedException();
        }

        public TElement CreateElement(TElementSpec spec)
        {
            throw new NotImplementedException();
        }

        public TElement UpdateElement(TElement element, TElementSpec spec)
        {
            throw new NotImplementedException();
        }
    }
}