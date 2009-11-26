// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class  PRIMLibrary : ElementLibrary<IPRIM, PRIM, PRIMSpec>, IPRIMLibrary
    {
        public PRIMLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        protected override PRIM CreateCCTSElement(Element element)
        {
            return new PRIM(repository, element);
        }
    }
}