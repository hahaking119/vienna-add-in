using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    internal class CacheItemCDTLibrary
    {
        internal CDTLibrary CDTLibrary { get; set; }

        internal List<CDT> CDTsInLibrary { get; set; }

        internal CacheItemCDTLibrary(CDTLibrary library)
        {
            CDTLibrary = library;
        }
    }
}