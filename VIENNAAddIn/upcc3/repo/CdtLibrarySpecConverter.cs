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
    internal static partial class CdtLibrarySpecConverter
    {
		internal static UmlPackageSpec Convert(CdtLibrarySpec cdtLibrarySpec)
		{
			var umlPackageSpec = new UmlPackageSpec
				{
					Stereotype = "CDTLibrary",
					Name = cdtLibrarySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", cdtLibrarySpec.BusinessTerms) ,
							new UmlTaggedValueSpec("copyright", cdtLibrarySpec.Copyrights) ,
							new UmlTaggedValueSpec("owner", cdtLibrarySpec.Owners) ,
							new UmlTaggedValueSpec("reference", cdtLibrarySpec.References) ,
							new UmlTaggedValueSpec("status", cdtLibrarySpec.Status) ,
							new UmlTaggedValueSpec("uniqueIdentifier", cdtLibrarySpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(cdtLibrarySpec) },
							new UmlTaggedValueSpec("versionIdentifier", cdtLibrarySpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("baseURN", cdtLibrarySpec.BaseURN) ,
							new UmlTaggedValueSpec("namespacePrefix", cdtLibrarySpec.NamespacePrefix) ,
						},
					DiagramType = UmlDiagramType.Class,
				};

			return umlPackageSpec;
		}
	}
}

