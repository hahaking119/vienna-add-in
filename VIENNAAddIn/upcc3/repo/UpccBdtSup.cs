using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BdtLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccBdtSup : IBdtSup
    {
        public IUmlAttribute UmlAttribute { get; set; }

        public UpccBdtSup(IUmlAttribute umlAttribute)
        {
            UmlAttribute = umlAttribute;
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

        public IBdt Bdt
        {
            get { throw new NotImplementedException(); }
        }

        public BasicType BasicType
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

        public string Enumeration
        {
            get { throw new NotImplementedException(); }
        }

        public string FractionDigits
        {
            get { throw new NotImplementedException(); }
        }

        public string LanguageCode
        {
            get { throw new NotImplementedException(); }
        }

        public string MaximumExclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MaximumInclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MaximumLength
        {
            get { throw new NotImplementedException(); }
        }

        public string MinimumExclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MinimumInclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MinimumLength
        {
            get { throw new NotImplementedException(); }
        }

        public string ModificationAllowedIndicator
        {
            get { throw new NotImplementedException(); }
        }

        public string Pattern
        {
            get { throw new NotImplementedException(); }
        }

        public string TotalDigits
        {
            get { throw new NotImplementedException(); }
        }

        public string UniqueIdentifier
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> UsageRules
        {
            get { throw new NotImplementedException(); }
        }

        public string VersionIdentifier
        {
            get { throw new NotImplementedException(); }
        }
    }
}