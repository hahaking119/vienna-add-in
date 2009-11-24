using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc
{
    internal class UpccPrim : IPRIM
    {
        private readonly IUmlClass umlClass;

        public UpccPrim(IUmlClass umlClass)
        {
            this.umlClass = umlClass;
        }

        #region IPRIM Members

        public int Id
        {
            get { return umlClass.Id; }
        }

        public string GUID
        {
            get { return umlClass.GUID; }
        }

        public string Name
        {
            get { return umlClass.Name; }
        }

        public string DictionaryEntryName
        {
            get { return umlClass.GetTaggedValue(TaggedValues.dictionaryEntryName).Value; }
        }

        public string Definition
        {
            get { return umlClass.GetTaggedValue(TaggedValues.definition).Value; }
        }

        public string UniqueIdentifier
        {
            get { return umlClass.GetTaggedValue(TaggedValues.uniqueIdentifier).Value; }
        }

        public string VersionIdentifier
        {
            get { return umlClass.GetTaggedValue(TaggedValues.versionIdentifier).Value; }
        }

        public string LanguageCode
        {
            get { return umlClass.GetTaggedValue(TaggedValues.languageCode).Value; }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return umlClass.GetTaggedValue(TaggedValues.businessTerm).SplitValues; }
        }

        public IBusinessLibrary Library
        {
            get { return new UpccBLibrary(umlClass.Package); }
        }

        public string Pattern
        {
            get { return umlClass.GetTaggedValue(TaggedValues.pattern).Value; }
        }

        public string FractionDigits
        {
            get { return umlClass.GetTaggedValue(TaggedValues.fractionDigits).Value; }
        }

        public string Length
        {
            get { return umlClass.GetTaggedValue(TaggedValues.length).Value; }
        }

        public string MaxExclusive
        {
            get { return umlClass.GetTaggedValue(TaggedValues.maxExclusive).Value; }
        }

        public string MaxInclusive
        {
            get { return umlClass.GetTaggedValue(TaggedValues.maxInclusive).Value; }
        }

        public string MaxLength
        {
            get { return umlClass.GetTaggedValue(TaggedValues.maxLength).Value; }
        }

        public string MinExclusive
        {
            get { return umlClass.GetTaggedValue(TaggedValues.minExclusive).Value; }
        }

        public string MinInclusive
        {
            get { return umlClass.GetTaggedValue(TaggedValues.minInclusive).Value; }
        }

        public string MinLength
        {
            get { return umlClass.GetTaggedValue(TaggedValues.minLength).Value; }
        }

        public string TotalDigits
        {
            get { return umlClass.GetTaggedValue(TaggedValues.totalDigits).Value; }
        }

        public string WhiteSpace
        {
            get { return umlClass.GetTaggedValue(TaggedValues.whiteSpace).Value; }
        }

        public IPRIM IsEquivalentTo
        {
            get
            {
                var dependencies = umlClass.GetDependenciesByStereotype(Stereotype.IsEquivalentTo);
                return dependencies.Count() == 0 ? null : new UpccPrim(dependencies.First().Target);
            }
        }

        #endregion
    }
}