using System;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public abstract class UpccElement
    {
        protected Element element;

        protected UpccElement(Element element, string stereotype)
        {
            if (element.Stereotype != stereotype)
            {
                throw new ArgumentException("invalid stereotype: " + element.Stereotype + ", expected: " + stereotype);
            }
            this.element = element;
        }
    }
}