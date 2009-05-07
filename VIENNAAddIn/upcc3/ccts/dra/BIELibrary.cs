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
    internal class BIELibrary : BusinessLibrary, IBIELibrary
    {
        public BIELibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IBIELibrary Members

        public IEnumerable<IABIE> BIEs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    if (element.IsABIE())
                    {
                        yield return new ABIE(repository, element);
                    }
                }
            }
        }

        public IABIE CreateABIE(ABIESpec spec)
        {
            var element = (Element) package.Elements.AddNew(spec.Name, "Class");
            element.Stereotype = util.Stereotype.ABIE;
            element.PackageID = Id;
            package.Elements.Refresh();
            AddElementToDiagram(element);
            var abie = new ABIE(repository, element);
            abie.Update(spec);
            return abie;
        }

        public IABIE UpdateABIE(IABIE abie, ABIESpec spec)
        {
            ((ABIE) abie).Update(spec);
            return abie;
        }

        public ICCTSElement ElementByName(string name)
        {
            return BIEs.First(e => e.Name == name);
        }

        #endregion
    }
}