using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemCCLibrary
    {
        internal ICCLibrary CCLibrary { get; set; }

        internal List<IACC> CCsInLibrary { get; set; }

        internal CacheItemCCLibrary(ICCLibrary library)
        {
            CCLibrary = library;
        }
    }
}