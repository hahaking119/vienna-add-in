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
    internal static partial class AsccSpecConverter
    {
		internal static UmlAssociationSpec Convert(AsccSpec asccSpec)
		{
			var associatingClassifierType = ((UpccAcc) asccSpec.AssociatingAcc).UmlClass;
			var associatedClassifierType = ((UpccAcc) asccSpec.AssociatedAcc).UmlClass;
			var umlAssociationSpec = new UmlAssociationSpec
				{
					Stereotype = "ASCC",
					Name = asccSpec.Name,
					UpperBound = asccSpec.UpperBound,
					LowerBound = asccSpec.LowerBound,
					AggregationKind = AggregationKind.Shared,
					AssociatingClassifier = associatingClassifierType,
					AssociatedClassifier = associatedClassifierType,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", asccSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", asccSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", asccSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(asccSpec) },
							new UmlTaggedValueSpec("languageCode", asccSpec.LanguageCode) ,
							new UmlTaggedValueSpec("sequencingKey", asccSpec.SequencingKey) ,
							new UmlTaggedValueSpec("uniqueIdentifier", asccSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(asccSpec) },
							new UmlTaggedValueSpec("versionIdentifier", asccSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", asccSpec.UsageRules) ,
						},
				};

			return umlAssociationSpec;
		}
	}
}

