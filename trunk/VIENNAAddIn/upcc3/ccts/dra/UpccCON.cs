using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    /// <summary>
    /// Note: This file cannot be named 'CON' after the class, because 'CON' is a reserved name for windows file systems.
    /// </summary>
    internal class CON : DTComponent, ICON
    {
        public CON(CCRepository repository, Attribute attribute, IDT dt) : base(repository, attribute, dt)
        {
        }
    }
}