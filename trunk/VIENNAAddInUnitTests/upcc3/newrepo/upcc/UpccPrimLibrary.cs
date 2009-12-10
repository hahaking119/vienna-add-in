using System;
using System.Collections.Generic;
using CctsRepository.BLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc
{
    internal class UpccPrimLibrary : IPrimLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccPrimLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region IPrimLibrary Members

        public int Id
        {
            get { return umlPackage.Id; }
        }

        public string Name
        {
            get { return umlPackage.Name; }
        }

        public IBLibrary BLibrary
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

        public void RemovePrim(IPrim prim)
        {
            throw new NotImplementedException();
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

        public IEnumerable<IPrim> Prims
        {
            get
            {
                foreach (var umlDataType in umlPackage.DataTypes)
                {
                    yield return new UpccPrim(umlDataType);
                }
            }
        }

        public IPrim GetPrimByName(string name)
        {
            foreach (IPrim prim in Prims)
            {
                if (prim.Name == name)
                {
                    return prim;
                }
            }
            return null;
        }

        public IPrim CreatePrim(PrimSpec spec)
        {
            throw new NotImplementedException();
        }

        public IPrim UpdatePrim(IPrim element, PrimSpec spec)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}