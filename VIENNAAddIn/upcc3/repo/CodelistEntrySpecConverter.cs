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
    internal static partial class CodelistEntrySpecConverter
    {
		internal static UmlEnumerationLiteralSpec Convert(CodelistEntrySpec codelistEntrySpec)
		{
			var umlEnumerationLiteralySpec = new UmlEnumerationLiteralSpec
				{
					Stereotype = "CodelistEntry",
					Name = codelistEntrySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("codeName", codelistEntrySpec.CodeName) ,
							new UmlTaggedValueSpec("status", codelistEntrySpec.Status) ,
						},
	
				};

			return umlEnumerationLiteralySpec;
		}
	}
}

