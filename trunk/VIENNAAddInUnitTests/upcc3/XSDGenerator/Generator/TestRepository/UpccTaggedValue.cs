using System;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class UpccTaggedValue : UpccElement
    {
        private readonly string value;

        public UpccTaggedValue(string name, string value) : base(name)
        {
            this.value = value;
        }

        public string GetValue()
        {
            return value;
        }

        public override string GetStereotype()
        {
            throw new NotImplementedException();
        }
    }
}