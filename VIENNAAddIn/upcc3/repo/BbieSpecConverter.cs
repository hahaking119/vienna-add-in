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

namespace VIENNAAddIn.upcc3.repo
{
    internal static partial class BbieSpecConverter
    {
		internal static UmlAttributeSpec Convert(BbieSpec bbieSpec, string className)
		{
			var type = ((UpccBdt) bbieSpec.Bdt).UmlClass;
			var umlAttributeSpec = new UmlAttributeSpec
				{
					Stereotype = "BBIE",
					Name = bbieSpec.Name,
					Type = type,
					UpperBound = bbieSpec.UpperBound,
					LowerBound = bbieSpec.LowerBound,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", bbieSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", bbieSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", bbieSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(bbieSpec, className) },
							new UmlTaggedValueSpec("languageCode", bbieSpec.LanguageCode) ,
							new UmlTaggedValueSpec("sequencingKey", bbieSpec.SequencingKey) ,
							new UmlTaggedValueSpec("uniqueIdentifier", bbieSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(bbieSpec, className) },
							new UmlTaggedValueSpec("versionIdentifier", bbieSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", bbieSpec.UsageRules) ,
						},
	
				};

			return umlAttributeSpec;
		}
	}
}

