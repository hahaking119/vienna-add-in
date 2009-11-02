using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemBIELibrary
    {
        internal BIELibrary bieLibrary { get; set; }

        internal List<ABIE> abiesInLibrary { get; set; }

        internal CacheItemBIELibrary(BIELibrary library)
        {
            bieLibrary = library;
        }
    }
}