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
    internal static partial class AsmaSpecConverter
    {
		internal static UmlAssociationSpec Convert(AsmaSpec asmaSpec)
		{
			var associatingClassifierType = ((UpccMa) asmaSpec.AssociatingMa).UmlClass;
			IUmlClassifier associatedClassifierType;
			var multiType = asmaSpec.AssociatedBieAggregator;
            if (multiType.IsAbie)
            {
                associatedClassifierType = ((UpccAbie) multiType.Abie).UmlClass;
			}
			else
            if (multiType.IsMa)
            {
                associatedClassifierType = ((UpccMa) multiType.Ma).UmlClass;
			}
			else
			{
				associatedClassifierType = null;
			}
			var umlAssociationSpec = new UmlAssociationSpec
				{
					Stereotype = "ASMA",
					Name = asmaSpec.Name,
					UpperBound = asmaSpec.UpperBound,
					LowerBound = asmaSpec.LowerBound,
					AggregationKind = AggregationKind.Shared,
					AssociatingClassifier = associatingClassifierType,
					AssociatedClassifier = associatedClassifierType,
				};

			return umlAssociationSpec;
		}
	}
}

