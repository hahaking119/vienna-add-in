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
    internal static partial class DocLibrarySpecConverter
    {
		internal static UmlPackageSpec Convert(DocLibrarySpec docLibrarySpec)
		{
			var umlPackageSpec = new UmlPackageSpec
				{
					Stereotype = "DOCLibrary",
					Name = docLibrarySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", docLibrarySpec.BusinessTerms) ,
							new UmlTaggedValueSpec("copyright", docLibrarySpec.Copyrights) ,
							new UmlTaggedValueSpec("owner", docLibrarySpec.Owners) ,
							new UmlTaggedValueSpec("reference", docLibrarySpec.References) ,
							new UmlTaggedValueSpec("status", docLibrarySpec.Status) ,
							new UmlTaggedValueSpec("uniqueIdentifier", docLibrarySpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(docLibrarySpec) },
							new UmlTaggedValueSpec("versionIdentifier", docLibrarySpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("baseURN", docLibrarySpec.BaseURN) ,
							new UmlTaggedValueSpec("namespacePrefix", docLibrarySpec.NamespacePrefix) ,
						},
					DiagramType = UmlDiagramType.Class,
				};

			return umlPackageSpec;
		}
	}
}

