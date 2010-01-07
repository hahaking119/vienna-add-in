using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccBcc : IBcc
    {
        public IUmlAttribute UmlAttribute { get; set; }

        public UpccBcc(IUmlAttribute umlAttribute)
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

        public IAcc Acc
        {
            get { throw new NotImplementedException(); }
        }

        public ICdt Cdt
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