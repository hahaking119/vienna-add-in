// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using EA;
using UPCCRepositoryInterface;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BIELibrary : ElementLibrary<IABIE, ABIE, ABIESpec>, IBIELibrary
    {
        public BIELibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        protected override ABIE CreateCCTSElement(Element element)
        {
            return new ABIE(repository, element);
        }
    }
}