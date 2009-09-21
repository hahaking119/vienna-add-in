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
    ///<summary>
    ///</summary>
    public class CDTLibrary : ElementLibrary<ICDT, CDT, CDTSpec>, ICDTLibrary
    {
        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="package"></param>
        public CDTLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        protected override CDT CreateCCTSElement(Element element)
        {
            return new CDT(repository, element);
        }
    }
}