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

namespace VIENNAAddIn.upcc3.repo.DocLibrary
{
    internal static partial class MaSpecConverter
    {
		internal static UmlClassSpec Convert(MaSpec maSpec)
		{
			var umlClassSpec = new UmlClassSpec
				{
					Stereotype = "MA",
					Name = maSpec.Name,
				};

			var associationSpecs = new List<UmlAssociationSpec>();
			foreach (var asmaSpec in maSpec.Asmas)
			{
				associationSpecs.Add(AsmaSpecConverter.Convert(asmaSpec, maSpec.Name));
			}
			umlClassSpec.Associations = associationSpecs;

			return umlClassSpec;
		}
	}
}

