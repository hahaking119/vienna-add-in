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
    internal class BDTLibrary : ElementLibrary<IBDT, BDT, BDTSpec>, IBDTLibrary
    {
        public BDTLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IBDTLibrary Members

        public IEnumerable<IBDT> BDTs
        {
            get { return GetCCTSElements(); }
        }

        #endregion

        protected override BDT CreateCCTSElement(Element element)
        {
            return new BDT(repository, element);
        }
    }
}