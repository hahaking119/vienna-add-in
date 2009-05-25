using System;
using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public static class EADiagramExtensions
    {
        public static Diagram With(this Diagram diagram, Action<Diagram> doSomethingWith)
        {
            doSomethingWith(diagram);
            diagram.Update();
            return diagram;
        }
    }
}