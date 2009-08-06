using System;
using System.IO;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class ConstantComponent : IEquatable<ConstantComponent>
    {
        public void PrettyPrint(TextWriter writer, string indent)
        {
            writer.WriteLine(indent + "Constant: " + Value);
        }

        public string Value { get; private set; }

        public ConstantComponent(string value)
        {
            Value = value;
        }

        public bool Equals(ConstantComponent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Value, Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ConstantComponent)) return false;
            return Equals((ConstantComponent)obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}