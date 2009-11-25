using System.Collections.Generic;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemBIELibrary
    {
        internal IBIELibrary bieLibrary { get; set; }

        internal List<IABIE> abiesInLibrary { get; set; }

        internal CacheItemBIELibrary(IBIELibrary library)
        {
            bieLibrary = library;
        }
    }
}