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
    internal class PRIMLibrary : BusinessLibrary, IPRIMLibrary
    {
        public PRIMLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IPRIMLibrary Members

        public IEnumerable<IPRIM> PRIMs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    yield return new PRIM(repository, element);
                }
            }
        }

        public ICCTSElement ElementByName(string name)
        {
            return PRIMs.First(e => e.Name == name);
        }

        #endregion
    }
}