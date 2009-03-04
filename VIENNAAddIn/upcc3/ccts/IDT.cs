using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IDT : ICCTSElement, IHasUsageRules
    {
        IEnumerable<ISUP> SUPs { get; }
        ICON CON { get; }
    }
}