using System.Collections.Generic;
using System.Linq;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc
{
    internal class UpccPrim : IPrim
    {
        private readonly IUmlDataType umlDataType;

        public UpccPrim(IUmlDataType umlDataType)
        {
            this.umlDataType = umlDataType;
        }

        #region IPrim Members

        public int Id
        {
            get { return umlDataType.Id; }
        }

        public string Name
        {
            get { return umlDataType.Name; }
        }

        public string DictionaryEntryName
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.dictionaryEntryName).Value; }
        }

        public string Definition
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.definition).Value; }
        }

        public string UniqueIdentifier
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.uniqueIdentifier).Value; }
        }

        public string VersionIdentifier
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.versionIdentifier).Value; }
        }

        public string LanguageCode
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.languageCode).Value; }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.businessTerm).SplitValues; }
        }

        public IPrimLibrary Library
        {
            get { return new UpccPrimLibrary(umlDataType.Package); }
        }

        public string Pattern
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.pattern).Value; }
        }

        public string FractionDigits
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.fractionDigits).Value; }
        }

        public string Length
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.length).Value; }
        }

        public string MaxExclusive
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.maxExclusive).Value; }
        }

        public string MaxInclusive
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.maxInclusive).Value; }
        }

        public string MaxLength
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.maxLength).Value; }
        }

        public string MinExclusive
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.minExclusive).Value; }
        }

        public string MinInclusive
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.minInclusive).Value; }
        }

        public string MinLength
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.minLength).Value; }
        }

        public string TotalDigits
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.totalDigits).Value; }
        }

        public string WhiteSpace
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.whiteSpace).Value; }
        }

        public IPrim IsEquivalentTo
        {
            get
            {
                var dependencies = umlDataType.GetDependenciesByStereotype(Stereotype.IsEquivalentTo);
                return dependencies.Count() == 0 ? null : new UpccPrim(dependencies.First().Target);
            }
        }

        #endregion
    }
}