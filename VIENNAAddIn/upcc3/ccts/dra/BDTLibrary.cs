using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BDTLibrary : BusinessLibrary, IBDTLibrary
    {
        public BDTLibrary(CCRepository repository, Package package)
            : base(repository, package, BusinessLibraryType.BDTLibrary)
        {
        }

        #region IBDTLibrary Members

        public IEnumerable<IBDT> BDTs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    yield return new BDT(repository, element);
                }
            }
        }

        #endregion
    }
}