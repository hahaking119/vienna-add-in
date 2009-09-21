// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class DOCLibrary : ElementLibrary<IABIE, ABIE, ABIESpec>, IDOCLibrary
    {
        public DOCLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IDOCLibrary Members

        public IEnumerable<IABIE> RootElements
        {
            get
            {
                var abies = new List<IABIE>(Elements);
                // collect ASBIES
                var asbies = new List<IASBIE>();
                foreach (IABIE abie in Elements)
                {
                    asbies.AddRange(abie.ASBIEs);
                }
                // remove all abies that are associated via an ASBIE
                foreach (IASBIE asbie in asbies)
                {
                    IABIE associatedABIE = asbie.AssociatedElement;
                    abies.Remove(associatedABIE);
                }
                return abies;
            }
        }

        #endregion

        protected override ABIE CreateCCTSElement(Element element)
        {
            return new ABIE(repository, element);
        }
    }
}