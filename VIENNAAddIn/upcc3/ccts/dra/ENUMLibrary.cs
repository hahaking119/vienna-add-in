// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ENUMLibrary : BusinessLibrary, IENUMLibrary
    {
        public ENUMLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IENUMLibrary Members

        public IEnumerable<IENUM> ENUMs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    if (element.IsENUM())
                    {
                        yield return new ENUM(repository, element);
                    }
                }
            }
        }

        #endregion

        public ICCTSElement ElementByName(string name)
        {
            return ENUMs.First(e => e.Name == name);
        }
    }
}