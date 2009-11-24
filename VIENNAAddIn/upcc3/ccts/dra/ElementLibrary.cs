using System.Collections.Generic;
using CctsRepository;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public abstract class ElementLibrary : BusinessLibrary
    {
        protected ElementLibrary(CCRepository repository, Package package) : base(repository, package)
        {
        }
    }
}