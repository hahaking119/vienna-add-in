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
using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccAsma : IAsma
    {
        public UpccAsma(IUmlAssociation umlAssociation, IMa associatingMa)
        {
            UmlAssociation = umlAssociation;
			AssociatingMa = associatingMa;
        }

        public IUmlAssociation UmlAssociation { get; private set; }

        #region IAsma Members

        public int Id
        {
            get { return UmlAssociation.Id; }
        }

        public string Name
        {
            get { return UmlAssociation.Name; }
        }

        public string UpperBound
		{
            get { return UmlAssociation.UpperBound; }
		}
		
        public string LowerBound
		{
            get { return UmlAssociation.LowerBound; }
		}
		
        public bool IsOptional()
        {
            int i;
            return Int32.TryParse(LowerBound, out i) && i == 0;
        }
		
        public IMa AssociatingMa { get; private set; }
		
		public BieAggregator AssociatedBieAggregator
		{
			get
			{
				var associatedClassifier = UmlAssociation.AssociatedClassifier;
                switch (associatedClassifier.Stereotype)
                {
                    case "ABIE":
                    	return new BieAggregator(new UpccAbie((IUmlClass) associatedClassifier));
                    case "MA":
                    	return new BieAggregator(new UpccMa((IUmlClass) associatedClassifier));
                    default:
                        return null;
                }
			}
		}

		#endregion
    }
}

