using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public class AttributeBuilder : EAObjectBuilder<AttributeBuilder>
    {
        private readonly Path pathToType;
        private string lowerBound = "1";
        private string upperBound = "1";

        public AttributeBuilder(string name, string stereotype, Path pathToType) : base(name, stereotype)
        {
            this.pathToType = pathToType;
        }

        public AttributeBuilder LowerBound(string lowerBound)
        {
            this.lowerBound = lowerBound;
            return this;
        }

        public AttributeBuilder UpperBound(string upperBound)
        {
            this.upperBound = upperBound;
            return this;
        }

        public Path GetPathToType()
        {
            return pathToType;
        }

        public string GetLowerBound()
        {
            return lowerBound;
        }

        public string GetUpperBound()
        {
            return upperBound;
        }
    }
}