using System;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class InputOutputKey : IEquatable<InputOutputKey>
    {
        #region KeyType enum

        public enum KeyType
        {
            Input,
            Output,
            None
        }

        #endregion

        private InputOutputKey(KeyType type, string value)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; private set; }
        public KeyType Type { get; private set; }

        public bool IsInputKey
        {
            get { return KeyType.Input == Type; }
        }

        public static InputOutputKey None
        {
            get { return new InputOutputKey(KeyType.None, ""); }
        }

        public bool IsOutputKey
        {
            get { return KeyType.Output == Type; }
        }

        #region IEquatable<InputOutputKey> Members

        public bool Equals(InputOutputKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Value, Value) && Equals(other.Type, Type);
        }

        #endregion

        public static InputOutputKey Input(string value)
        {
            return new InputOutputKey(KeyType.Input, value);
        }

        public static InputOutputKey Output(string value)
        {
            return new InputOutputKey(KeyType.Output, value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(InputOutputKey)) return false;
            return Equals((InputOutputKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Value != null ? Value.GetHashCode() : 0) * 397) ^ Type.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", Type, Value);
        }
    }
}