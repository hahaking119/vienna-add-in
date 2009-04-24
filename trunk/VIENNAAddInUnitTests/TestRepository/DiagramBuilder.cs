namespace VIENNAAddInUnitTests.TestRepository
{
    public class DiagramBuilder
    {
        private readonly string diagramType;
        private readonly string name;

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