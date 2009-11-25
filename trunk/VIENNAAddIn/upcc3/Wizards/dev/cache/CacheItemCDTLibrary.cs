using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemCDTLibrary
    {
        internal ICDTLibrary CDTLibrary { get; set; }

        internal List<ICDT> CDTsInLibrary { get; set; }

        internal CacheItemCDTLibrary(ICDTLibrary library)
        {
            CDTLibrary = library;
        }
    }
}