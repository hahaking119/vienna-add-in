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
    internal class DOCLibrary : BusinessLibrary, IDOCLibrary
    {
        public DOCLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IDOCLibrary Members

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

        public IEnumerable<IABIE> RootElements
        {
            get
            {
                var abies = new List<IABIE>(BIEs);
                // collect ASBIES
                var asbies = new List<IASBIE>();
                foreach (var abie in BIEs)
                {
                    asbies.AddRange(abie.ASBIEs);
                }
                // remove all abies that are associated via an ASBIE
                foreach (var asbie in asbies)
                {
                    var associatedABIE = asbie.AssociatedElement;
                    abies.Remove(associatedABIE);
                }
                return abies;
            }
        }

        #endregion

        public ICCTSElement ElementByName(string name)
        {
            return BIEs.First(e => e.Name == name);
        }
    }
}