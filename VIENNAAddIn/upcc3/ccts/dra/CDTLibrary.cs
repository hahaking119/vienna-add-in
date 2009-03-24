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
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class CDTLibrary : BusinessLibrary, ICDTLibrary
    {
        public CDTLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region ICDTLibrary Members

        public IEnumerable<ICDT> CDTs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    if (element.IsCDT())
                    {
                        yield return new CDT(repository, element);
                    }
                }
            }
        }

        #endregion

        public ICCTSElement ElementByName(string name)
        {
            return CDTs.First(e => e.Name == name);
        }
    }
}