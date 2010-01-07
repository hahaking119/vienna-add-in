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
    internal static partial class EnumSpecConverter
    {
		internal static UmlEnumerationSpec Convert(EnumSpec enumSpec)
		{
			var umlEnumerationSpec = new UmlEnumerationSpec
				{
					Name = enumSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", enumSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("codeListAgencyIdentifier", enumSpec.CodeListAgencyIdentifier) ,
							new UmlTaggedValueSpec("codeListAgencyName", enumSpec.CodeListAgencyName) ,
							new UmlTaggedValueSpec("codeListIdentifier", enumSpec.CodeListIdentifier) ,
							new UmlTaggedValueSpec("codeListName", enumSpec.CodeListName) ,
							new UmlTaggedValueSpec("dictionaryEntryName", enumSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(enumSpec) },
							new UmlTaggedValueSpec("definition", enumSpec.Definition) ,
							new UmlTaggedValueSpec("enumerationURI", enumSpec.EnumerationURI) ,
							new UmlTaggedValueSpec("languageCode", enumSpec.LanguageCode) ,
							new UmlTaggedValueSpec("modificationAllowedIndicator", enumSpec.ModificationAllowedIndicator) ,
							new UmlTaggedValueSpec("restrictedPrimitive", enumSpec.RestrictedPrimitive) ,
							new UmlTaggedValueSpec("status", enumSpec.Status) ,
							new UmlTaggedValueSpec("uniqueIdentifier", enumSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(enumSpec) },
							new UmlTaggedValueSpec("versionIdentifier", enumSpec.VersionIdentifier) ,
						},
					Dependencies = new []
						{
							new UmlDependencySpec<IUmlEnumeration>
							{
								Stereotype = "isEquivalentTo",
								Target = ((UpccEnum) enumSpec.IsEquivalentTo).UmlEnumeration,
								LowerBound = "0",
								UpperBound = "1",
							},
						},
				};

			var enumerationLiteralSpecs = new List<UmlEnumerationLiteralSpec>();
			foreach (var codelistEntrySpec in enumSpec.CodelistEntries)
			{
				enumerationLiteralSpecs.Add(CodelistEntrySpecConverter.Convert(codelistEntrySpec));
			}
			umlEnumerationSpec.EnumerationLiterals = enumerationLiteralSpecs;

			return umlEnumerationSpec;
		}
	}
}

