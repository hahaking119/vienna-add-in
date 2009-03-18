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
    public class CCLibrary : BusinessLibrary, ICCLibrary
    {
        public CCLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region ICCLibrary Members

        public IEnumerable<IACC> ACCs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    yield return new ACC(repository, element);
                }
            }
        }

        #endregion

        public ICCTSElement ElementByName(string name)
        {
            return ACCs.First(e => e.Name == name);
        }
    }
}