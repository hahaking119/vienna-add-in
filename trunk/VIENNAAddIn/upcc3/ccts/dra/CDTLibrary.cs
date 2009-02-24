using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class CDTLibrary : BusinessLibrary, ICDTLibrary
    {
        public CDTLibrary(CCRepository repository, Package package, BusinessLibraryType libraryType)
            : base(repository, package, libraryType)
        {
        }

        #region ICDTLibrary Members

        public IEnumerable<ICDT> CDTs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    yield return new CDT(this, element);
                }
            }
        }

        #endregion
    }
}