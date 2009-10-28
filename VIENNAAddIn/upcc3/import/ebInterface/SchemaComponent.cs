using System;
using System.Collections.Generic;
using System.IO;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class SchemaComponent : IEquatable<SchemaComponent>
    {
        public SchemaComponent(string schema, IEnumerable<Namespace> namespaces, Entry rootEntry)
        {
            Schema = schema;
            Namespaces = new List<Namespace>(namespaces);
            RootEntry = rootEntry;
        }

        public string Schema { get; private set; }
        public List<Namespace> Namespaces { get; private set; }
        public Entry RootEntry { get; private set; }

        #region IEquatable<SchemaComponent> Members

        public bool Equals(SchemaComponent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Schema, Schema) && other.Namespaces.IsEqualTo(Namespaces) && Equals(other.RootEntry, RootEntry);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(SchemaComponent) && Equals((SchemaComponent)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Schema != null ? Schema.GetHashCode() : 0) * 397) ^ (RootEntry != null ? RootEntry.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return String.Format("Schema: {0}", Schema);
        }

        public void PrettyPrint(TextWriter writer, string indent)
        {
            writer.WriteLine(indent + "Schema: " + Schema);
            writer.WriteLine(indent + "  Namespaces: ");
            foreach (Namespace ns in Namespaces)
            {
                writer.WriteLine(indent + "    " + ns.ID);
            }
            writer.WriteLine(indent + "  Entries: ");
            RootEntry.PrettyPrint(writer, indent + "    ");
        }

        /// <summary>
        /// Takes a SchemaComponent as input and returns true if this SchemaComponent is an Input Schema, i.e. a 
        /// source Schema, and false otherwise.
        /// </summary>
        /// <returns></returns>
        public bool IsInputSchema()
        {
            return IsInput(RootEntry);
        }

        /// <summary>
        /// Takes an Entry as input and returns true if this Entry or its child Entries have an OutputKey specified,
        /// false otherwise.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static bool IsInput(Entry entry)
        {
            if (entry.IsInput)
            {
                return true;
            }
            if (entry.IsOutput)
            {
                return false;
            }
            foreach (Entry subEntry in entry.SubEntries)
            {
                var isInput = IsInput(subEntry);
                if (isInput)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Takes a SchemaComponent as input and returns true if this SchemaComponent is an Output Schema, i.e. a 
        /// target Schema, and false otherwise.
        /// </summary>
        /// <returns></returns>
        public bool IsOutputSchema()
        {
            return IsOutput(RootEntry);
        }

        /// <summary>
        /// Takes an Entry as input and returns true if this Entry or its child Entries have an InputKey specified,
        /// false otherwise.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static bool IsOutput(Entry entry)
        {
            if (entry.IsInput)
            {
                return false;
            }
            if (entry.IsOutput)
            {
                return true;
            }
            foreach (Entry subEntry in entry.SubEntries)
            {
                var isOutput = IsOutput(subEntry);
                if (isOutput)
                {
                    return true;
                }
            }
            return false;
        }
    }
}