// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using CctsRepository;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class BDTLibrary : ElementLibrary<IBDT, BDT, BDTSpec>, IBDTLibrary
    {
        public BDTLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        protected override BDT WrapEaElement(Element element)
        {
            return new BDT(repository, element);
        }
    }
}