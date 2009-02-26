using System.Collections.Generic;
using EA;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class Path
    {
        private readonly EARepository repository;
        private readonly List<string> parts = new List<string>();

        public Path(EARepository repository)
        {
            this.repository = repository;
        }

        public Path Append(string part)
        {
            parts.Add(part);
            return this;
        }

        public static Path operator /(Path lhs, string rhs)
        {
            return lhs.Append(rhs);
        }

        public Element Resolve()
        {
            return repository.ResolvePath(parts);
        }
    }
}