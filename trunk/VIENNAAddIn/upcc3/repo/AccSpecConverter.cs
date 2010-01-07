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
    internal static class AccSpecConverter
    {
		internal static UmlClassSpec Convert(AccSpec accSpec)
		{
			var umlClassSpec = new UmlClassSpec
				{
					Name = accSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", accSpec.BusinessTerms),
							new UmlTaggedValueSpec("definition", accSpec.Definition),
							new UmlTaggedValueSpec("dictionaryEntryName", accSpec.DictionaryEntryName),
							new UmlTaggedValueSpec("languageCode", accSpec.LanguageCode),
							new UmlTaggedValueSpec("uniqueIdentifier", accSpec.UniqueIdentifier),
							new UmlTaggedValueSpec("versionIdentifier", accSpec.VersionIdentifier),
							new UmlTaggedValueSpec("usageRule", accSpec.UsageRules),
						},
					Dependencies = new []
						{
							new UmlDependencySpec<IUmlClass>
							{
								Stereotype = "isEquivalentTo",
								Target = ((UpccAcc) accSpec.IsEquivalentTo).UmlClass,
								LowerBound = "0",
								UpperBound = "1",
							},
						},
				};

			var attributeSpecs = new List<UmlAttributeSpec>();
			foreach (var bccSpec in accSpec.Bccs)
			{
				attributeSpecs.Add(BccSpecConverter.Convert(bccSpec));
			}
			umlClassSpec.Attributes = attributeSpecs;
			umlClassSpec.Attributes = attributeSpecs;

			var associationSpecs = new List<UmlAssociationSpec>();
			foreach (var asccSpec in accSpec.Asccs)
			{
				associationSpecs.Add(AsccSpecConverter.Convert(asccSpec));
			}
			umlClassSpec.Associations = associationSpecs;

			return umlClassSpec;
		}
	}
}

