using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemCcLibrary
    {
        internal ICcLibrary CCLibrary { get; set; }

        internal List<IAcc> CCsInLibrary { get; set; }

        internal CacheItemCcLibrary(ICcLibrary library)
        {
            CCLibrary = library;
        }
    }
}