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
// ReSharper restore RedundantUsingDirective
using VIENNAAddIn.upcc3.uml;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.repo
{
    internal static partial class PrimSpecConverter
    {
		internal static UmlDataTypeSpec Convert(PrimSpec primSpec)
		{
			var umlDataTypeSpec = new UmlDataTypeSpec
				{
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
					Dependencies = new []
						{
							new UmlDependencySpec<IUmlDataType>
							{
								Stereotype = "isEquivalentTo",
								Target = ((UpccPrim) primSpec.IsEquivalentTo).UmlDataType,
								LowerBound = "0",
								UpperBound = "1",
							},
						},
				};

			return umlDataTypeSpec;
		}
	}
}

