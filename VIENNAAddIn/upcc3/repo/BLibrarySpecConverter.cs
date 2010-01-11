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
    internal static partial class BLibrarySpecConverter
    {
		internal static UmlPackageSpec Convert(BLibrarySpec bLibrarySpec)
		{
			var umlPackageSpec = new UmlPackageSpec
				{
					Stereotype = "bLibrary",
					Name = bLibrarySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", bLibrarySpec.BusinessTerms) ,
							new UmlTaggedValueSpec("copyright", bLibrarySpec.Copyrights) ,
							new UmlTaggedValueSpec("owner", bLibrarySpec.Owners) ,
							new UmlTaggedValueSpec("reference", bLibrarySpec.References) ,
							new UmlTaggedValueSpec("status", bLibrarySpec.Status) ,
							new UmlTaggedValueSpec("uniqueIdentifier", bLibrarySpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(bLibrarySpec) },
							new UmlTaggedValueSpec("versionIdentifier", bLibrarySpec.VersionIdentifier) ,
						},
					DiagramType = UmlDiagramType.Package,
				};

			return umlPackageSpec;
		}
	}
}

