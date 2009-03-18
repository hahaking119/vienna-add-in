// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using System.Linq;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class DOCLibrary : BusinessLibrary, IDOCLibrary
    {
        public DOCLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IDOCLibrary Members

        public IEnumerable<IBIE> BIEs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    yield return new ABIE(repository, element);
                }
            }
        }

        #endregion

        public ICCTSElement ElementByName(string name)
        {
            return BIEs.First(e => e.Name == name);
        }
    }
}