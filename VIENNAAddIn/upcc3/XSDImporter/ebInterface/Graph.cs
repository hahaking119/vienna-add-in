using System;
using System.Collections.Generic;
using System.IO;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class Graph : IEquatable<Graph>
    {
        public IEnumerable<Vertex> Vertices { get; private set; }

        public Graph(IEnumerable<Vertex> vertices)
        {
            Vertices = new List<Vertex>(vertices);
        }

        public void PrettyPrint(TextWriter writer, string indent)
        {
            writer.WriteLine(indent + "Graph:");
            foreach (var vertex in Vertices)
            {
                vertex.PrettyPrint(writer, indent + "  ");
            }
        }

        public bool Equals(Graph other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Vertices.IsEqualTo(Vertices);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Graph)) return false;
            return Equals((Graph)obj);
        }

        public override int GetHashCode()
        {
            return (Vertices != null ? Vertices.GetHashCode() : 0);
        }
    }
}