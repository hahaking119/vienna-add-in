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
    public class CDTLibrary : ElementLibrary<ICDT, CDT, CDTSpec>, ICDTLibrary
    {
        public CDTLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region ICDTLibrary Members

        public IEnumerable<ICDT> CDTs
        {
            get { return GetCCTSElements(); }
        }

        #endregion

        protected override CDT CreateCCTSElement(Element element)
        {
            return new CDT(repository, element);
        }
    }
}