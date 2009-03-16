using System;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class CDT : DT
    {
        public override string GetStereotype()
        {
            return "CDT";
        }

        public string Definition
        {
            set { AddTaggedValue(TaggedValues.Definition.AsString(), value); }
        }
    }
}