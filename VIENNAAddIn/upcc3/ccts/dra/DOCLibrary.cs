using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class DOCLibrary : BusinessLibrary, IDOCLibrary
    {
        public DOCLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IDOCLibrary Members

        public IEnumerable<IBIE> BIEs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    yield return new ABIE(repository, element);
                }
            }
        }

        #endregion
    }
}