using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class CCLibrary : UpccPackage
    {
        public CCLibrary(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { return "CCLibrary"; }
        }

        public List<ACC> ACCs
        {
            set { AddClasses(value); }
        }
    }
}