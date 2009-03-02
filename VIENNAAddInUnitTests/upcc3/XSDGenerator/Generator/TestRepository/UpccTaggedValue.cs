using System;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public class UpccTaggedValue : UpccElement
    {
        public string Value { get; set; }

        public override string GetStereotype()
        {
            throw new NotImplementedException();
        }
    }
}