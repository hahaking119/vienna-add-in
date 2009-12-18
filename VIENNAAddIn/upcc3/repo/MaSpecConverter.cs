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
    internal static class MaSpecConverter
    {
		internal static UmlClassSpec Convert(MaSpec maSpec)
		{
			var umlClassSpec = new UmlClassSpec
				{
					Name = maSpec.Name,
				};

			var associationSpecs = new List<UmlAssociationSpec>();
			foreach (var asmaSpec in maSpec.Asmas)
			{
				associationSpecs.Add(AsmaSpecConverter.Convert(asmaSpec));
			}
			umlClassSpec.Associations = associationSpecs;

			return umlClassSpec;
		}
	}
}

