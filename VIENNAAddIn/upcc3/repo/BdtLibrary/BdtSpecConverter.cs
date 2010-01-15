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

namespace VIENNAAddIn.upcc3.repo.BdtLibrary
{
    internal static partial class BdtSpecConverter
    {
		internal static UmlClassSpec Convert(BdtSpec bdtSpec)
		{
			var umlClassSpec = new UmlClassSpec
				{
					Stereotype = "BDT",
					Name = bdtSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", bdtSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", bdtSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", bdtSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(bdtSpec) },
							new UmlTaggedValueSpec("languageCode", bdtSpec.LanguageCode) ,
							new UmlTaggedValueSpec("uniqueIdentifier", bdtSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(bdtSpec) },
							new UmlTaggedValueSpec("versionIdentifier", bdtSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", bdtSpec.UsageRules) ,
						},
				};

			var dependencySpecs = new List<UmlDependencySpec>();
			if (bdtSpec.IsEquivalentTo != null)
			{
				dependencySpecs.Add(new UmlDependencySpec
									{
										Stereotype = "isEquivalentTo",
										Target = ((UpccBdt) bdtSpec.IsEquivalentTo).UmlClass,
										LowerBound = "0",
										UpperBound = "1",
									});
			}
			if (bdtSpec.BasedOn != null)
			{
				dependencySpecs.Add(new UmlDependencySpec
									{
										Stereotype = "basedOn",
										Target = ((UpccCdt) bdtSpec.BasedOn).UmlClass,
										LowerBound = "0",
										UpperBound = "1",
									});
			}
			umlClassSpec.Dependencies = dependencySpecs;

			var attributeSpecs = new List<UmlAttributeSpec>();
			attributeSpecs.Add(BdtConSpecConverter.Convert(bdtSpec.Con, bdtSpec.Name));
			umlClassSpec.Attributes = attributeSpecs;
			foreach (var bdtSupSpec in bdtSpec.Sups)
			{
				attributeSpecs.Add(BdtSupSpecConverter.Convert(bdtSupSpec, bdtSpec.Name));
			}
			umlClassSpec.Attributes = attributeSpecs;
			umlClassSpec.Attributes = attributeSpecs;

			return umlClassSpec;
		}
	}
}

