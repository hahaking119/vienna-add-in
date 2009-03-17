using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public class PackageBuilder : EAObjectBuilder<PackageBuilder>
    {
        private readonly List<ElementBuilder> elements = new List<ElementBuilder>();
        private readonly List<PackageBuilder> packages = new List<PackageBuilder>();

        public PackageBuilder(string name, string stereotype) : base(name, stereotype)
        {
        }

        public PackageBuilder Packages(params PackageBuilder[] packages)
        {
            this.packages.AddRange(packages);
            return this;
        }

        public PackageBuilder Elements(params ElementBuilder[] elements)
        {
            this.elements.AddRange(elements);
            return this;
        }

        public IEnumerable<PackageBuilder> GetPackages()
        {
            return packages;
        }

        public IEnumerable<ElementBuilder> GetElements()
        {
            return elements;
        }
    }
}