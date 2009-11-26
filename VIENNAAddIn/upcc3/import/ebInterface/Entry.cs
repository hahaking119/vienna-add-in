using System;
using System.Collections.Generic;
using System.IO;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    /// <summary>
    /// Represents a MapForce mapping element of the form:
    /// <entry name="Invoice" expanded="1">
    /// </summary>
    public class Entry : IEquatable<Entry>
    {
        private readonly InputOutputKey inputOutputKey;

        public Entry(string name, InputOutputKey inputOutputKey)
            : this(name, inputOutputKey, new Entry[0])
        {
        }

        public Entry(string name, InputOutputKey inputOutputKey, IEnumerable<Entry> subEntries)
        {
            this.inputOutputKey = inputOutputKey;
            Name = name;
            SubEntries = new List<Entry>(subEntries);
        }

        public InputOutputKey InputOutputKey
        {
            get { return inputOutputKey; }
        }

        public string Name { get; private set; }
        public IEnumerable<Entry> SubEntries { get; private set; }

        /// <summary>
        /// Determines whether this object is a mapping input element, i.e. a source element.
        /// </summary>
        public bool IsInput
        {
            get { return inputOutputKey.IsOutputKey; }
        }

        /// <summary>
        /// Determines whether this object is a mapping output element, i.e. a target element.
        /// </summary>
        public bool IsOutput
        {
            get { return inputOutputKey.IsInputKey; }
        }

        #region IEquatable<Entry> Members

        public bool Equals(Entry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && other.SubEntries.IsEqualTo(SubEntries);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(Entry) && Equals((Entry)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (SubEntries != null ? SubEntries.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, SubEntries: {1}", Name, SubEntries);
        }

        public void PrettyPrint(TextWriter writer, string indent)
        {
            writer.WriteLine(indent + "Entry: " + Name + " [" + inputOutputKey + "]");
            foreach (Entry subEntry in SubEntries)
            {
                subEntry.PrettyPrint(writer, indent + "  ");
            }
        }

        public bool HasKey(string key)
        {
            return inputOutputKey.Value == key;
        }
    }
}