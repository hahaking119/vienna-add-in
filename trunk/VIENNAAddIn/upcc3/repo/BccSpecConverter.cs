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
    internal static partial class BccSpecConverter
    {
		internal static UmlAttributeSpec Convert(BccSpec bccSpec, string className)
		{
			var type = ((UpccCdt) bccSpec.Cdt).UmlClass;
			var umlAttributeSpec = new UmlAttributeSpec
				{
					Stereotype = "BCC",
					Name = bccSpec.Name,
					Type = type,
					UpperBound = bccSpec.UpperBound,
					LowerBound = bccSpec.LowerBound,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", bccSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", bccSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", bccSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(bccSpec, className) },
							new UmlTaggedValueSpec("languageCode", bccSpec.LanguageCode) ,
							new UmlTaggedValueSpec("sequencingKey", bccSpec.SequencingKey) ,
							new UmlTaggedValueSpec("uniqueIdentifier", bccSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(bccSpec, className) },
							new UmlTaggedValueSpec("versionIdentifier", bccSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", bccSpec.UsageRules) ,
						},
	
				};

			return umlAttributeSpec;
		}
	}
}

