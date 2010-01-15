// ReSharper disable RedundantUsingDirective
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.repo.BdtLibrary;
using VIENNAAddIn.upcc3.repo.BieLibrary;
using VIENNAAddIn.upcc3.repo.BLibrary;
using VIENNAAddIn.upcc3.repo.CcLibrary;
using VIENNAAddIn.upcc3.repo.CdtLibrary;
using VIENNAAddIn.upcc3.repo.DocLibrary;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddIn.upcc3.repo.PrimLibrary;
// ReSharper restore RedundantUsingDirective
using VIENNAAddIn.upcc3.uml;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.repo.PrimLibrary
{
    internal static partial class PrimSpecConverter
    {
		internal static UmlDataTypeSpec Convert(PrimSpec primSpec)
		{
			var umlDataTypeSpec = new UmlDataTypeSpec
				{
					Stereotype = "PRIM",
					Name = primSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", primSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", primSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", primSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(primSpec) },
							new UmlTaggedValueSpec("fractionDigits", primSpec.FractionDigits) ,
							new UmlTaggedValueSpec("languageCode", primSpec.LanguageCode) ,
							new UmlTaggedValueSpec("length", primSpec.Length) ,
							new UmlTaggedValueSpec("maximumExclusive", primSpec.MaximumExclusive) ,
							new UmlTaggedValueSpec("maximumInclusive", primSpec.MaximumInclusive) ,
							new UmlTaggedValueSpec("maximumLength", primSpec.MaximumLength) ,
							new UmlTaggedValueSpec("minimumExclusive", primSpec.MinimumExclusive) ,
							new UmlTaggedValueSpec("minimumInclusive", primSpec.MinimumInclusive) ,
							new UmlTaggedValueSpec("minimumLength", primSpec.MinimumLength) ,
							new UmlTaggedValueSpec("pattern", primSpec.Pattern) ,
							new UmlTaggedValueSpec("totalDigits", primSpec.TotalDigits) ,
							new UmlTaggedValueSpec("uniqueIdentifier", primSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(primSpec) },
							new UmlTaggedValueSpec("versionIdentifier", primSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("whiteSpace", primSpec.WhiteSpace) ,
						},
				};

			var dependencySpecs = new List<UmlDependencySpec>();
			if (primSpec.IsEquivalentTo != null)
			{
				dependencySpecs.Add(new UmlDependencySpec
									{
										Stereotype = "isEquivalentTo",
										Target = ((UpccPrim) primSpec.IsEquivalentTo).UmlDataType,
										LowerBound = "0",
										UpperBound = "1",
									});
			}
			umlDataTypeSpec.Dependencies = dependencySpecs;

			return umlDataTypeSpec;
		}
	}
}

