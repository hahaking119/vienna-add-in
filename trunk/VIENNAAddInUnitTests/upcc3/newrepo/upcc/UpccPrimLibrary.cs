using System;
using System.Collections.Generic;
using CctsRepository.BLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc
{
    internal class UpccPrimLibrary : IPrimLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccPrimLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region IPrimLibrary Members

        public int Id
        {
            get { return umlPackage.Id; }
        }

        public string Name
        {
            get { return umlPackage.Name; }
        }

        public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(umlPackage.Parent); }
        }

        public void RemovePrim(IPrim prim)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPrim> Prims
        {
            get
            {
                foreach (var umlDataType in umlPackage.DataTypes)
                {
                    yield return new UpccPrim(umlDataType);
                }
            }
        }

        public IPrim GetPrimByName(string name)
        {
            foreach (IPrim prim in Prims)
            {
                if (prim.Name == name)
                {
                    return prim;
                }
            }
            return null;
        }

        public IPrim CreatePrim(PrimSpec spec)
        {
            var newDataType = umlPackage.CreateDataType(
                new UmlDataTypeSpec
                {
                    Name = spec.Name,
                    TaggedValues = new[]
                                   {
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.businessTerm.ToString(),
                                           Value = MultiPartTaggedValue.Merge(spec.BusinessTerms),
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.definition.ToString(),
                                           Value = spec.Definition,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.dictionaryEntryName.ToString(),
                                           Value = spec.DictionaryEntryName,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.fractionDigits.ToString(),
                                           Value = spec.FractionDigits,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.languageCode.ToString(),
                                           Value = spec.LanguageCode,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.length.ToString(),
                                           Value = spec.Length,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.maximumExclusive.ToString(),
                                           Value = spec.MaximumExclusive,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.maximumInclusive.ToString(),
                                           Value = spec.MaximumInclusive,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.maximumLength.ToString(),
                                           Value = spec.MaximumLength,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.minimumExclusive.ToString(),
                                           Value = spec.MinimumExclusive,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.minimumInclusive.ToString(),
                                           Value = spec.MinimumInclusive,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.minimumLength.ToString(),
                                           Value = spec.MinimumLength,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.pattern.ToString(),
                                           Value = spec.Pattern,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.totalDigits.ToString(),
                                           Value = spec.TotalDigits,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.uniqueIdentifier.ToString(),
                                           Value = spec.UniqueIdentifier,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.versionIdentifier.ToString(),
                                           Value = spec.VersionIdentifier,
                                       },
                                       new UmlTaggedValueSpec
                                       {
                                           Name = TaggedValues.whiteSpace.ToString(),
                                           Value = spec.WhiteSpace,
                                       },
                                   },
                });
            if (spec.IsEquivalentTo != null)
            {
                var equivalentDataType = umlPackage.GetDataTypeById(spec.IsEquivalentTo.Id);
                newDataType.CreateDependency(new UmlDependencySpec<IUmlDataType>
                                             {
                                                 Stereotype = Stereotype.isEquivalentTo,
                                                 Target = equivalentDataType,
                                                 LowerBound = "0",
                                                 UpperBound = "1",
                                             });
            }
            return new UpccPrim(newDataType);
        }

        public IPrim UpdatePrim(IPrim element, PrimSpec spec)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return GetTaggedValue(TaggedValues.businessTerm).SplitValues; }
        }

        public IEnumerable<string> Copyrights
        {
            get { return GetTaggedValue(TaggedValues.copyright).SplitValues; }
        }

        public IEnumerable<string> Owners
        {
            get { return GetTaggedValue(TaggedValues.owner).SplitValues; }
        }

        public IEnumerable<string> References
        {
            get { return GetTaggedValue(TaggedValues.reference).SplitValues; }
        }

        public string Status
        {
            get { return GetTaggedValue(TaggedValues.status).Value; }
        }

        public string UniqueIdentifier
        {
            get { return GetTaggedValue(TaggedValues.uniqueIdentifier).Value; }
        }

        public string VersionIdentifier
        {
            get { return GetTaggedValue(TaggedValues.versionIdentifier).Value; }
        }

        public string BaseURN
        {
            get { return GetTaggedValue(TaggedValues.baseURN).Value; }
        }

        public string NamespacePrefix
        {
            get { return GetTaggedValue(TaggedValues.namespacePrefix).Value; }
        }

        #endregion

        private IUmlTaggedValue GetTaggedValue(TaggedValues taggedValueName)
        {
            return umlPackage.GetTaggedValue(taggedValueName) ?? new EmptyUmlTaggedValue();
        }
    }
}