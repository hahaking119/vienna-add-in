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

        public IPrimLibrary PrimLibrary
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

        public string MaximumExclusive
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.maximumExclusive).Value; }
        }

        public string MaximumInclusive
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.maximumInclusive).Value; }
        }

        public string MaximumLength
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.maximumLength).Value; }
        }

        public string MinimumExclusive
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.minimumExclusive).Value; }
        }

        public string MinimumInclusive
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.minimumInclusive).Value; }
        }

        public string MinimumLength
        {
            get { return umlDataType.GetTaggedValue(TaggedValues.minimumLength).Value; }
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
                var dependencies = umlDataType.GetDependenciesByStereotype(Stereotype.isEquivalentTo);
                return dependencies.Count() == 0 ? null : new UpccPrim(dependencies.First().Target);
            }
        }

        #endregion
    }
}