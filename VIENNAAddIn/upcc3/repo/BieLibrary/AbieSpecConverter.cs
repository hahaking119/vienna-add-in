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

namespace VIENNAAddIn.upcc3.repo.BieLibrary
{
    internal static partial class AbieSpecConverter
    {
		internal static UmlClassSpec Convert(AbieSpec abieSpec)
		{
			var umlClassSpec = new UmlClassSpec
				{
					Stereotype = "ABIE",
					Name = abieSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", abieSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", abieSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", abieSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(abieSpec) },
							new UmlTaggedValueSpec("languageCode", abieSpec.LanguageCode) ,
							new UmlTaggedValueSpec("uniqueIdentifier", abieSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(abieSpec) },
							new UmlTaggedValueSpec("versionIdentifier", abieSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", abieSpec.UsageRules) ,
						},
				};

			var dependencySpecs = new List<UmlDependencySpec>();
			if (abieSpec.IsEquivalentTo != null)
			{
				dependencySpecs.Add(new UmlDependencySpec
									{
										Stereotype = "isEquivalentTo",
										Target = ((UpccAbie) abieSpec.IsEquivalentTo).UmlClass,
										LowerBound = "0",
										UpperBound = "1",
									});
			}
			if (abieSpec.BasedOn != null)
			{
				dependencySpecs.Add(new UmlDependencySpec
									{
										Stereotype = "basedOn",
										Target = ((UpccAcc) abieSpec.BasedOn).UmlClass,
										LowerBound = "0",
										UpperBound = "1",
									});
			}
			umlClassSpec.Dependencies = dependencySpecs;

			var attributeSpecs = new List<UmlAttributeSpec>();
			foreach (var bbieSpec in abieSpec.Bbies)
			{
				attributeSpecs.Add(BbieSpecConverter.Convert(bbieSpec, abieSpec.Name));
			}
			umlClassSpec.Attributes = attributeSpecs;
			umlClassSpec.Attributes = attributeSpecs;

			var associationSpecs = new List<UmlAssociationSpec>();
			foreach (var asbieSpec in abieSpec.Asbies)
			{
				associationSpecs.Add(AsbieSpecConverter.Convert(asbieSpec, abieSpec.Name));
			}
			umlClassSpec.Associations = associationSpecs;

			return umlClassSpec;
		}
	}
}

