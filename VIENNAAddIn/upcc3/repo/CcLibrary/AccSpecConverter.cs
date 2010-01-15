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

namespace VIENNAAddIn.upcc3.repo.CcLibrary
{
    internal static partial class AccSpecConverter
    {
		internal static UmlClassSpec Convert(AccSpec accSpec)
		{
			var umlClassSpec = new UmlClassSpec
				{
					Stereotype = "ACC",
					Name = accSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", accSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", accSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", accSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(accSpec) },
							new UmlTaggedValueSpec("languageCode", accSpec.LanguageCode) ,
							new UmlTaggedValueSpec("uniqueIdentifier", accSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(accSpec) },
							new UmlTaggedValueSpec("versionIdentifier", accSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", accSpec.UsageRules) ,
						},
					Dependencies = new []
						{
							new UmlDependencySpec
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
				attributeSpecs.Add(BccSpecConverter.Convert(bccSpec, accSpec.Name));
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

