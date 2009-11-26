using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemCDTLibrary
    {
        internal ICdtLibrary CDTLibrary { get; set; }

        internal List<ICdt> CDTsInLibrary { get; set; }

        internal CacheItemCDTLibrary(ICdtLibrary library)
        {
            CDTLibrary = library;
        }
    }
}