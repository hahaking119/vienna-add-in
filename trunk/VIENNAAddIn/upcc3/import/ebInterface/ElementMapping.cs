using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public abstract class ElementMapping : AbstractMapping
    {
        protected override IEnumerable<ElementMapping> Children
        {
            get { yield break; }
        }
    }
}