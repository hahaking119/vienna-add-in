using System;
using System.Collections.Generic;
using System.Linq;
using EA;

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
                    yield return new ENUM(repository, element);
                }
            }
        }

        #endregion

        public IElement ElementByName(string name)
        {
            return ENUMs.First(e => e.Name == name);
        }
    }
}