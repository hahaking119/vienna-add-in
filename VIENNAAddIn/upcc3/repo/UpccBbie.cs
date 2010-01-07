using System;
using System.Collections.Generic;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccBbie : IBbie
    {
        public IUmlAttribute UmlAttribute { get; private set; }

        public UpccBbie(IUmlAttribute umlAttribute)
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

        public IAbie Abie
        {
            get { throw new NotImplementedException(); }
        }

        public IBdt Bdt
        {
            get { throw new NotImplementedException(); }
        }

        public IBcc BasedOn
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