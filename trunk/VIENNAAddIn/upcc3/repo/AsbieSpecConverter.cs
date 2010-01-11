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
    internal static partial class AsbieSpecConverter
    {
		internal static UmlAssociationSpec Convert(AsbieSpec asbieSpec)
		{
			var associatingClassifierType = ((UpccAbie) asbieSpec.AssociatingAbie).UmlClass;
			var associatedClassifierType = ((UpccAbie) asbieSpec.AssociatedAbie).UmlClass;
			var umlAssociationSpec = new UmlAssociationSpec
				{
					Stereotype = "ASBIE",
					Name = asbieSpec.Name,
					UpperBound = asbieSpec.UpperBound,
					LowerBound = asbieSpec.LowerBound,
					AggregationKind = asbieSpec.AggregationKind,
					AssociatingClassifier = associatingClassifierType,
					AssociatedClassifier = associatedClassifierType,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", asbieSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", asbieSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", asbieSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(asbieSpec) },
							new UmlTaggedValueSpec("languageCode", asbieSpec.LanguageCode) ,
							new UmlTaggedValueSpec("sequencingKey", asbieSpec.SequencingKey) ,
							new UmlTaggedValueSpec("uniqueIdentifier", asbieSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(asbieSpec) },
							new UmlTaggedValueSpec("versionIdentifier", asbieSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", asbieSpec.UsageRules) ,
						},
				};

			return umlAssociationSpec;
		}
	}
}

