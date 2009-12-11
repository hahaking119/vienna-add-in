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
            get { return GetTaggedValue(TaggedValues.dictionaryEntryName).Value; }
        }

        private IUmlTaggedValue GetTaggedValue(TaggedValues taggedValueName)
        {
            return umlDataType.GetTaggedValue(taggedValueName) ?? new EmptyUmlTaggedValue();
        }

        public string Definition
        {
            get { return GetTaggedValue(TaggedValues.definition).Value; }
        }

        public string UniqueIdentifier
        {
            get { return GetTaggedValue(TaggedValues.uniqueIdentifier).Value; }
        }

        public string VersionIdentifier
        {
            get { return GetTaggedValue(TaggedValues.versionIdentifier).Value; }
        }

        public string LanguageCode
        {
            get { return GetTaggedValue(TaggedValues.languageCode).Value; }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return GetTaggedValue(TaggedValues.businessTerm).SplitValues; }
        }

        public IPrimLibrary PrimLibrary
        {
            get { return new UpccPrimLibrary(umlDataType.Package); }
        }

        public string Pattern
        {
            get { return GetTaggedValue(TaggedValues.pattern).Value; }
        }

        public string FractionDigits
        {
            get { return GetTaggedValue(TaggedValues.fractionDigits).Value; }
        }

        public string Length
        {
            get { return GetTaggedValue(TaggedValues.length).Value; }
        }

        public string MaximumExclusive
        {
            get { return GetTaggedValue(TaggedValues.maximumExclusive).Value; }
        }

        public string MaximumInclusive
        {
            get { return GetTaggedValue(TaggedValues.maximumInclusive).Value; }
        }

        public string MaximumLength
        {
            get { return GetTaggedValue(TaggedValues.maximumLength).Value; }
        }

        public string MinimumExclusive
        {
            get { return GetTaggedValue(TaggedValues.minimumExclusive).Value; }
        }

        public string MinimumInclusive
        {
            get { return GetTaggedValue(TaggedValues.minimumInclusive).Value; }
        }

        public string MinimumLength
        {
            get { return GetTaggedValue(TaggedValues.minimumLength).Value; }
        }

        public string TotalDigits
        {
            get { return GetTaggedValue(TaggedValues.totalDigits).Value; }
        }

        public string WhiteSpace
        {
            get { return GetTaggedValue(TaggedValues.whiteSpace).Value; }
        }

        public IPrim IsEquivalentTo
        {
            get
            {
                var dependencies = umlDataType.GetDependenciesByStereotype(Stereotype.isEquivalentTo);
                return dependencies.Count() == 0 ? null : new UpccPrim((IUmlDataType) dependencies.First().Target);
            }
        }

        #endregion
    }
}