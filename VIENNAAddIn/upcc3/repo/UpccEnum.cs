using System;
using System.Collections.Generic;
using CctsRepository.EnumLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccEnum : IEnum
    {
        public UpccEnum(IUmlEnumeration umlEnumeration)
        {
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

        public IEnumLibrary EnumLibrary
        {
            get { throw new NotImplementedException(); }
        }

        public IEnum IsEquivalentTo
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<ICodelistEntry> CodelistEntries
        {
            get { throw new NotImplementedException(); }
        }

        public ICodelistEntry CreateCodelistEntry(CodelistEntrySpec specification)
        {
            throw new NotImplementedException();
        }

        public ICodelistEntry UpdateCodelistEntry(ICodelistEntry codelistEntry, CodelistEntrySpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveCodelistEntry(ICodelistEntry codelistEntry)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> BusinessTerms
        {
            get { throw new NotImplementedException(); }
        }

        public string CodeListAgencyIdentifier
        {
            get { throw new NotImplementedException(); }
        }

        public string CodeListAgencyName
        {
            get { throw new NotImplementedException(); }
        }

        public string CodeListIdentifier
        {
            get { throw new NotImplementedException(); }
        }

        public string CodeListName
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

        public string EnumerationURI
        {
            get { throw new NotImplementedException(); }
        }

        public string LanguageCode
        {
            get { throw new NotImplementedException(); }
        }

        public bool ModificationAllowedIndicator
        {
            get { throw new NotImplementedException(); }
        }

        public string RestrictedPrimitive
        {
            get { throw new NotImplementedException(); }
        }

        public string Status
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
    }
}