using System;
using System.Collections.Generic;
using System.IO;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
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
            return string.Format("Schema: {0}", Schema);
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
    }
}