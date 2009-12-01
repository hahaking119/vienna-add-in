using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemCdtLibrary
    {
        internal ICdtLibrary CDTLibrary { get; set; }

        internal List<ICdt> CDTsInLibrary { get; set; }

        internal CacheItemCdtLibrary(ICdtLibrary library)
        {
            CDTLibrary = library;
        }
    }
}