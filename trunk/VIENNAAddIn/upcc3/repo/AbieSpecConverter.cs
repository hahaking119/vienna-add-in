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
    internal static class AbieSpecConverter
    {
		internal static UmlClassSpec Convert(AbieSpec abieSpec)
		{
			var umlClassSpec = new UmlClassSpec
				{
					Name = abieSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", abieSpec.BusinessTerms),
							new UmlTaggedValueSpec("definition", abieSpec.Definition),
							new UmlTaggedValueSpec("dictionaryEntryName", abieSpec.DictionaryEntryName),
							new UmlTaggedValueSpec("languageCode", abieSpec.LanguageCode),
							new UmlTaggedValueSpec("uniqueIdentifier", abieSpec.UniqueIdentifier),
							new UmlTaggedValueSpec("versionIdentifier", abieSpec.VersionIdentifier),
							new UmlTaggedValueSpec("usageRule", abieSpec.UsageRules),
						},
					Dependencies = new []
						{
							new UmlDependencySpec<IUmlClass>
							{
								Stereotype = "isEquivalentTo",
								Target = ((UpccAbie) abieSpec.IsEquivalentTo).UmlClass,
								LowerBound = "0",
								UpperBound = "1",
							},
							new UmlDependencySpec<IUmlClass>
							{
								Stereotype = "basedOn",
								Target = ((UpccAcc) abieSpec.BasedOn).UmlClass,
								LowerBound = "0",
								UpperBound = "1",
							},
						},
				};

			var attributeSpecs = new List<UmlAttributeSpec>();
			foreach (var bbieSpec in abieSpec.Bbies)
			{
				attributeSpecs.Add(BbieSpecConverter.Convert(bbieSpec));
			}
			umlClassSpec.Attributes = attributeSpecs;
			umlClassSpec.Attributes = attributeSpecs;

			var associationSpecs = new List<UmlAssociationSpec>();
			foreach (var asbieSpec in abieSpec.Asbies)
			{
				associationSpecs.Add(AsbieSpecConverter.Convert(asbieSpec));
			}
			umlClassSpec.Associations = associationSpecs;

			return umlClassSpec;
		}
	}
}

