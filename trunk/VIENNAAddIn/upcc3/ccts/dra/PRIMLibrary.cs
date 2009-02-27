using System.Collections.Generic;
using System.Linq;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class PRIMLibrary : BusinessLibrary, IPRIMLibrary
    {
        public PRIMLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IPRIMLibrary Members

        public IEnumerable<IPRIM> PRIMs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    yield return new PRIM(repository, element);
                }
            }
        }

        public IElement ElementByName(string name)
        {
            return PRIMs.First(e => e.Name == name);
        }

        #endregion
    }
}