using System;
using System.Collections.Generic;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccAbie : IAbie
    {
        public IUmlClass UmlClass { get; set; }

        public UpccAbie(IUmlClass umlClass)
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

        public IBieLibrary BieLibrary
        {
            get { throw new NotImplementedException(); }
        }

        public IAbie IsEquivalentTo
        {
            get { throw new NotImplementedException(); }
        }

        public IAcc BasedOn
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IBbie> Bbies
        {
            get { throw new NotImplementedException(); }
        }

        public IBbie CreateBbie(BbieSpec specification)
        {
            throw new NotImplementedException();
        }

        public IBbie UpdateBbie(IBbie bbie, BbieSpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveBbie(IBbie bbie)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAsbie> Asbies
        {
            get { throw new NotImplementedException(); }
        }

        public IAsbie CreateAsbie(AsbieSpec specification)
        {
            throw new NotImplementedException();
        }

        public IAsbie UpdateAsbie(IAsbie asbie, AsbieSpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveAsbie(IAsbie asbie)
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