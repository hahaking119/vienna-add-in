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
    internal static partial class EnumLibrarySpecConverter
    {
		internal static UmlPackageSpec Convert(EnumLibrarySpec enumLibrarySpec)
		{
			var umlPackageSpec = new UmlPackageSpec
				{
					Stereotype = "ENUMLibrary",
					Name = enumLibrarySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", enumLibrarySpec.BusinessTerms) ,
							new UmlTaggedValueSpec("copyright", enumLibrarySpec.Copyrights) ,
							new UmlTaggedValueSpec("owner", enumLibrarySpec.Owners) ,
							new UmlTaggedValueSpec("reference", enumLibrarySpec.References) ,
							new UmlTaggedValueSpec("status", enumLibrarySpec.Status) ,
							new UmlTaggedValueSpec("uniqueIdentifier", enumLibrarySpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(enumLibrarySpec) },
							new UmlTaggedValueSpec("versionIdentifier", enumLibrarySpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("baseURN", enumLibrarySpec.BaseURN) ,
							new UmlTaggedValueSpec("namespacePrefix", enumLibrarySpec.NamespacePrefix) ,
						},
					DiagramType = UmlDiagramType.Class,
				};

			return umlPackageSpec;
		}
	}
}

