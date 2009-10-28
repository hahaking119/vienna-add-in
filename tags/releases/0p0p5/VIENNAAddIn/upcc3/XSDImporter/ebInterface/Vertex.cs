using System;
using System.Collections.Generic;
using System.IO;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class Vertex : IEquatable<Vertex>
    {
        public string Key { get; private set; }
        public IEnumerable<Edge> Edges { get; private set; }

        public Vertex(string key, IEnumerable<Edge> edges)
        {
            Key = key;
            Edges = new List<Edge>(edges);
        }

        public bool Equals(Vertex other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Key, Key) && other.Edges.IsEqualTo(Edges);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Vertex)) return false;
            return Equals((Vertex)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Key != null ? Key.GetHashCode() : 0) * 397) ^ (Edges != null ? Edges.GetHashCode() : 0);
            }
        }

        public void PrettyPrint(TextWriter writer, string indent)
        {
            writer.WriteLine(indent + "Vertex [" + Key + "]:");
            foreach (var edge in Edges)
            {
                edge.PrettyPrint(writer, indent + "  ");
            }
        }
    }
}