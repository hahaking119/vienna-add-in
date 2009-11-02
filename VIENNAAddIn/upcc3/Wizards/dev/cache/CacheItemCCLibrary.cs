using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemCCLibrary
    {
        internal CCLibrary CCLibrary { get; set; }

        internal List<ACC> CCsInLibrary { get; set; }

        internal CacheItemCCLibrary(CCLibrary library)
        {
            CCLibrary = library;
        }
    }
}