using System;
using System.Collections.Generic;
using CctsRepository.BdtLibrary;
using CctsRepository.BLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccBdtLibrary : IBdtLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccBdtLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region IBdtLibrary Members

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

        public IEnumerable<IBdt> Bdts
        {
            get
            {
                foreach (IUmlClass umlClass in umlPackage.Classes)
                {
                    yield return new UpccBdt(umlClass);
                }
            }
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

        public void RemoveBdt(IBdt bdt)
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

        public IBdt GetBdtByName(string name)
        {
            foreach (var bdt in Bdts)
            {
                if (bdt.Name == name)
                {
                    return bdt;
                }
            }
            return null;
        }

        public IBdt CreateBdt(BdtSpec spec)
        {
            throw new NotImplementedException();
        }

        public IBdt UpdateBdt(IBdt element, BdtSpec spec)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}