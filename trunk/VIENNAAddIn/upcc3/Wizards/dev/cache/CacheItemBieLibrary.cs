using System.Collections.Generic;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemBieLibrary
    {
        internal IBieLibrary bieLibrary { get; set; }

        internal List<IAbie> abiesInLibrary { get; set; }

        internal CacheItemBieLibrary(IBieLibrary library)
        {
            bieLibrary = library;
        }
    }
}