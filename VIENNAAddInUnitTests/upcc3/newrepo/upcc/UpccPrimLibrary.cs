using System;
using System.Collections.Generic;
using UPCCRepositoryInterface;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc
{
    internal class UpccPrimLibrary : IPRIMLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccPrimLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region IPRIMLibrary Members

        public int Id
        {
            get { return umlPackage.Id; }
        }

        public string Name
        {
            get { return umlPackage.Name; }
        }

        public IBLibrary Parent
        {
            get { return new UpccBLibrary(umlPackage.Parent); }
        }

        public string Status
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.status).Value; }
        }

        public string UniqueIdentifier
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.uniqueIdentifier).Value; }
        }

        public string VersionIdentifier
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.versionIdentifier).Value; }
        }

        public string BaseURN
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.baseURN).Value; }
        }

        public string NamespacePrefix
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.namespacePrefix).Value; }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.businessTerm).SplitValues; }
        }

        public IEnumerable<string> Copyrights
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.copyright).SplitValues; }
        }

        public IEnumerable<string> Owners
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.owner).SplitValues; }
        }

        public IEnumerable<string> References
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.reference).SplitValues; }
        }

        public IEnumerable<IPRIM> Elements
        {
            get
            {
                foreach (IUmlClass primClass in umlPackage.Classes)
                {
                    yield return new UpccPrim(primClass);
                }
            }
        }

        public IPRIM ElementByName(string name)
        {
            foreach (IPRIM prim in Elements)
            {
                if (prim.Name == name)
                {
                    return prim;
                }
            }
            return null;
        }

        public IPRIM CreateElement(PRIMSpec spec)
        {
            throw new NotImplementedException();
        }

        public IPRIM UpdateElement(IPRIM element, PRIMSpec spec)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}