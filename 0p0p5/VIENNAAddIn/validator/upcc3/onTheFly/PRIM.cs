using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class PRIM : IPRIM
    {
        private readonly Element element;

        public PRIM(Element element)
        {
            this.element = element;
        }

        #region IPRIM Members

        public int Id
        {
            get { throw new NotImplementedException(); }
        }

        public string GUID
        {
            get { return element.ElementGUID; }
        }

        public string Name
        {
            get { return element.Name; }
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

        public IBusinessLibrary Library
        {
            get { throw new NotImplementedException(); }
        }

        public string Pattern
        {
            get { throw new NotImplementedException(); }
        }

        public string FractionDigits
        {
            get { throw new NotImplementedException(); }
        }

        public string Length
        {
            get { throw new NotImplementedException(); }
        }

        public string MaxExclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MaxInclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MaxLength
        {
            get { throw new NotImplementedException(); }
        }

        public string MinExclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MinInclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MinLength
        {
            get { throw new NotImplementedException(); }
        }

        public string TotalDigits
        {
            get { throw new NotImplementedException(); }
        }

        public string WhiteSpace
        {
            get { throw new NotImplementedException(); }
        }

        public IPRIM IsEquivalentTo
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}