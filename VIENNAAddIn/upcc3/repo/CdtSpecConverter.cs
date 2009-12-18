// ReSharper disable RedundantUsingDirective
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
    internal static class CdtSpecConverter
    {
		internal static UmlClassSpec Convert(CdtSpec cdtSpec)
		{
			var umlClassSpec = new UmlClassSpec
				{
					Name = cdtSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", cdtSpec.BusinessTerms),
							new UmlTaggedValueSpec("definition", cdtSpec.Definition),
							new UmlTaggedValueSpec("dictionaryEntryName", cdtSpec.DictionaryEntryName),
							new UmlTaggedValueSpec("languageCode", cdtSpec.LanguageCode),
							new UmlTaggedValueSpec("uniqueIdentifier", cdtSpec.UniqueIdentifier),
							new UmlTaggedValueSpec("versionIdentifier", cdtSpec.VersionIdentifier),
							new UmlTaggedValueSpec("usageRule", cdtSpec.UsageRules),
						},
					Dependencies = new []
						{
							new UmlDependencySpec<IUmlClass>
							{
								Stereotype = "isEquivalentTo",
								Target = ((UpccCdt) cdtSpec.IsEquivalentTo).UmlClass,
								LowerBound = "0",
								UpperBound = "1",
							},
						},
				};

			var attributeSpecs = new List<UmlAttributeSpec>();
			attributeSpecs.Add(CdtConSpecConverter.Convert(cdtSpec.Con));
			umlClassSpec.Attributes = attributeSpecs;
			foreach (var cdtSupSpec in cdtSpec.Sups)
			{
				attributeSpecs.Add(CdtSupSpecConverter.Convert(cdtSupSpec));
			}
			umlClassSpec.Attributes = attributeSpecs;
			umlClassSpec.Attributes = attributeSpecs;

			return umlClassSpec;
		}
	}
}

