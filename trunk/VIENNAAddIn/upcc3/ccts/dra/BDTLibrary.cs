using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BDTLibrary : BusinessLibrary, IBDTLibrary
    {
        public BDTLibrary(CCRepository repository, Package package, BusinessLibraryType libraryType)
            : base(repository, package, libraryType)
        {
        }

        #region IBDTLibrary Members

        public IEnumerable<IBDT> BDTs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    yield return new BDT(this, element);
                }
            }
        }

        #endregion
    }
}