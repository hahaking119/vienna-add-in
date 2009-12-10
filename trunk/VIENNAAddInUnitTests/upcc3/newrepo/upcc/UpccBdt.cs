using System;
using System.Collections.Generic;
using CctsRepository.BdtLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc
{
    internal class UpccBdt : IBdt
    {
        private readonly IUmlClass umlClass;

        public UpccBdt(IUmlClass umlClass)
        {
            this.umlClass = umlClass;
        }

        public int Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public IBdtLibrary BdtLibrary
        {
            get { throw new NotImplementedException(); }
        }

        public IBdtCon Con
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IBdtSup> Sups
        {
            get { throw new NotImplementedException(); }
        }

        public IBdtSup CreateBdtSup(BdtSupSpec specification)
        {
            throw new NotImplementedException();
        }

        public IBdtSup UpdateBdtSup(IBdtSup bdtSup, BdtSupSpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveBdtSup(IBdtSup bdtSup)
        {
            throw new NotImplementedException();
        }

        public IBdt IsEquivalentTo
        {
            get { throw new NotImplementedException(); }
        }

        public ICdt BasedOn
        {
            get { throw new NotImplementedException(); }
        }

        public string DictionaryEntryName
        {
            get { throw new NotImplementedException(); }
        }

        public string Definition
        {
            get { throw new NotImplementedException(); }
        }

        public string UniqueIdentifier
        {
            get { throw new NotImplementedException(); }
        }

        public string VersionIdentifier
        {
            get { throw new NotImplementedException(); }
        }

        public string LanguageCode
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> UsageRules
        {
            get { throw new NotImplementedException(); }
        }
    }
}