using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccAsbie : IAsbie
    {
        public IUmlAssociation UmlAssociation { get; set; }

        public UpccAsbie(IUmlAssociation umlAssociation)
        {
            UmlAssociation = umlAssociation;
        }

        public int Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string UpperBound
        {
            get { throw new NotImplementedException(); }
        }

        public string LowerBound
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsOptional()
        {
            throw new NotImplementedException();
        }

        public AggregationKind AggregationKind
        {
            get { throw new NotImplementedException(); }
        }

        public IAbie AssociatingAbie
        {
            get { throw new NotImplementedException(); }
        }

        public IAbie AssociatedAbie
        {
            get { throw new NotImplementedException(); }
        }

        public IAscc BasedOn
        {
            get { throw new NotImplementedException(); }
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

        public string SequencingKey
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