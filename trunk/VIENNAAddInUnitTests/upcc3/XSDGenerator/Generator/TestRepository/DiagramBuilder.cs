using System;
using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public class DiagramBuilder
    {
        private readonly string name;
        private readonly string diagramType;

        public DiagramBuilder(string name, string diagramType)
        {
            this.name = name;
            this.diagramType = diagramType;
        }

        public string GetName()
        {
            return name;
        }

        public string GetDiagramType()
        {
            return diagramType;
        }
    }
}