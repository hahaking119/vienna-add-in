using System;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class UpccTaggedValue : UpccElement
    {
        public string Value { get; private set; }

        public UpccTaggedValue(string name, string value) : base(name)
        {
            Value = value;
        }

        public override string Stereotype
        {
            get { throw new NotImplementedException(); }
        }
    }
}