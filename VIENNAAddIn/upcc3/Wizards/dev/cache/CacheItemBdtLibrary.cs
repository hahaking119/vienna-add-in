using System.Collections.Generic;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemBdtLibrary
    {
        internal IBdtLibrary BDTLibrary { get; set; }

        internal List<IBdt> BDTsInLibrary { get; set; }

        internal CacheItemBdtLibrary(IBdtLibrary library)
        {
            BDTLibrary = library;
        }
    }
}