using System;
using System.Collections.Generic;
using CctsRepository.CdtLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccCdt : ICdt
    {
        public IUmlClass UmlClass { get; set; }

        public UpccCdt(IUmlClass umlClass)
        {
            UmlClass = umlClass;
            throw new NotImplementedException();
        }

        public int Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public ICdtLibrary CdtLibrary
        {
            get { throw new NotImplementedException(); }
        }

        public ICdt IsEquivalentTo
        {
            get { throw new NotImplementedException(); }
        }

        public ICdtCon Con
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<ICdtSup> Sups
        {
            get { throw new NotImplementedException(); }
        }

        public ICdtSup CreateCdtSup(CdtSupSpec specification)
        {
            throw new NotImplementedException();
        }

        public ICdtSup UpdateCdtSup(ICdtSup cdtSup, CdtSupSpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveCdtSup(ICdtSup cdtSup)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> BusinessTerms
        {
            get { throw new NotImplementedException(); }
        }

        public string Definition
        {
            get { throw new NotImplementedException(); }
        }

        public string DictionaryEntryName
        {
            get { throw new NotImplementedException(); }
        }

        public string LanguageCode
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

        public IEnumerable<string> UsageRules
        {
            get { throw new NotImplementedException(); }
        }
    }
}