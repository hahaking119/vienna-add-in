using System.Collections.Generic;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemBDTLibrary
    {
        internal IBDTLibrary BDTLibrary { get; set; }

        internal List<IBDT> BDTsInLibrary { get; set; }

        internal CacheItemBDTLibrary(IBDTLibrary library)
        {
            BDTLibrary = library;
        }
    }
}