using System.Collections.Generic;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemBIELibrary
    {
        internal IBieLibrary bieLibrary { get; set; }

        internal List<IAbie> abiesInLibrary { get; set; }

        internal CacheItemBIELibrary(IBieLibrary library)
        {
            bieLibrary = library;
        }
    }
}