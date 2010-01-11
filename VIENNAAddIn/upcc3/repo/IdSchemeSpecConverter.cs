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
    internal static partial class IdSchemeSpecConverter
    {
		internal static UmlDataTypeSpec Convert(IdSchemeSpec idSchemeSpec)
		{
			var umlDataTypeSpec = new UmlDataTypeSpec
				{
					Stereotype = "IDSCHEME",
					Name = idSchemeSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", idSchemeSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", idSchemeSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", idSchemeSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(idSchemeSpec) },
							new UmlTaggedValueSpec("identifierSchemeAgencyIdentifier", idSchemeSpec.IdentifierSchemeAgencyIdentifier) ,
							new UmlTaggedValueSpec("identifierSchemeAgencyName", idSchemeSpec.IdentifierSchemeAgencyName) ,
							new UmlTaggedValueSpec("modificationAllowedIndicator", idSchemeSpec.ModificationAllowedIndicator) ,
							new UmlTaggedValueSpec("pattern", idSchemeSpec.Pattern) ,
							new UmlTaggedValueSpec("restrictedPrimitive", idSchemeSpec.RestrictedPrimitive) ,
							new UmlTaggedValueSpec("uniqueIdentifier", idSchemeSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(idSchemeSpec) },
							new UmlTaggedValueSpec("versionIdentifier", idSchemeSpec.VersionIdentifier) ,
						},
				};

			return umlDataTypeSpec;
		}
	}
}

