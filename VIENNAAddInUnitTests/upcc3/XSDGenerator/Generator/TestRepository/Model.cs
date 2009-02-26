using System;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class Model : Library
    {
        public Model(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { throw new NotImplementedException(); }
        }
    }
}