using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemBDTLibrary
    {
        internal BDTLibrary BDTLibrary { get; set; }

        internal List<BDT> BDTsInLibrary { get; set; }

        internal CacheItemBDTLibrary(BDTLibrary library)
        {
            BDTLibrary = library;
        }
    }
}