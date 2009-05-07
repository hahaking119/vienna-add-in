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
    public class CCLibrary : ElementLibrary<IACC, ACC, ACCSpec>, ICCLibrary
    {
        public CCLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region ICCLibrary Members

        public IEnumerable<IACC> ACCs
        {
            get { return GetCCTSElements(); }
        }

        #endregion

        protected override ACC CreateCCTSElement(Element element)
        {
            return new ACC(repository, element);
        }
    }
}