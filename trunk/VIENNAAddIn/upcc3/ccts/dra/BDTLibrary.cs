using System;
using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    class BDTLibrary : BusinessLibrary, IBDTLibrary
    {
        public BDTLibrary(CCRepository repository, Package package, BusinessLibraryType libraryType) : base(repository, package, libraryType)
        {
        }

        public IList<IBDT> BDTs
        {
            get
            {
                var elements = new List<IBDT>();
                foreach (Element element in package.Elements)
                {
                    elements.Add(repository.GetBDT(element));
                }
                return elements;
            }
        }
    }
}