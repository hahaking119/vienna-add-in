using System.Collections.Generic;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemBDTLibrary
    {
        internal IBdtLibrary BDTLibrary { get; set; }

        internal List<IBdt> BDTsInLibrary { get; set; }

        internal CacheItemBDTLibrary(IBdtLibrary library)
        {
            BDTLibrary = library;
        }
    }
}