using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccAcc : IAcc
    {
        public IUmlClass UmlClass { get; private set; }

        public UpccAcc(IUmlClass umlClass)
        {
            UmlClass = umlClass;
        }

        public int Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public ICcLibrary CcLibrary
        {
            get { throw new NotImplementedException(); }
        }

        public IAcc IsEquivalentTo
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IBcc> Bccs
        {
            get { throw new NotImplementedException(); }
        }

        public IBcc CreateBcc(BccSpec specification)
        {
            throw new NotImplementedException();
        }

        public IBcc UpdateBcc(IBcc bcc, BccSpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveBcc(IBcc bcc)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAscc> Asccs
        {
            get { throw new NotImplementedException(); }
        }

        public IAscc CreateAscc(AsccSpec specification)
        {
            throw new NotImplementedException();
        }

        public IAscc UpdateAscc(IAscc ascc, AsccSpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveAscc(IAscc ascc)
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