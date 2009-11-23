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
    internal class ENUMLibrary : ElementLibrary<IENUM, ENUM, ENUMSpec>, IENUMLibrary
    {
        public ENUMLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        protected override ENUM WrapEaElement(Element element)
        {
            return new ENUM(repository, element);
        }
    }
}