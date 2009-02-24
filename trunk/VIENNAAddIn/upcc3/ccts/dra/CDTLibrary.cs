using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    class CDTLibrary : BusinessLibrary, ICDTLibrary
    {
        public CDTLibrary(CCRepository repository, Package package, BusinessLibraryType libraryType) : base(repository, package, libraryType)
        {
        }

        public IList<ICDT> CDTs
        {
            get
            {
                var elements = new List<ICDT>();
                foreach (Element element in package.Elements)
                {
                    elements.Add(repository.GetCDT(element));
                }
                return elements;
            }
        }
    }
}