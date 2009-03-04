using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IHasUsageRules
    {
        IEnumerable<string> UsageRules { get; }
    }
}