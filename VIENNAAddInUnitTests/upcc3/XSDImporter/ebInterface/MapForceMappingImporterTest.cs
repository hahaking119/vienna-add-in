using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.XSDImporter.ebInterface
{
    [TestFixture]
    public class MapForceMappingImporterTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            expectedMapping = new MapForceMapping(new List<SchemaComponent>
                                                  {
                                                      new SchemaComponent("CoreComponent_1.xsd", new Namespace[0],
                                                                          new Entry("Address", InputOutputKey.Input("106374552"),
                                                                                    new[]
                                                                                    {
                                                                                        new Entry("CityName", InputOutputKey.Input("109158960")),
                                                                                    })
                                                          ),
                                                      new SchemaComponent("Invoice.xsd", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                          new Entry("Address", InputOutputKey.Output("105780248"),
                                                                                    new[]
                                                                                    {
                                                                                        new Entry("Town", InputOutputKey.Output("105906872")),
                                                                                    })
                                                          ),
                                                  },
                                                  new List<ConstantComponent>
                                                  {
                                                      new ConstantComponent("Root: Invoice"),
                                                  },
                                                  new Graph(new[]
                                                            {
                                                                new Vertex("105780248", new[]
                                                                                        {
                                                                                            new Edge("84250472", "106374552"),
                                                                                        }),
                                                                new Vertex("105906872", new[]
                                                                                        {
                                                                                            new Edge("105919872", "109158960"),
                                                                                        }),
                                                            }));
        }

        #endregion

        private MapForceMapping expectedMapping;

        [Test]
        public void TestImportSchemaComponents()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd");

            MapForceMapping mapping = LinqToXmlMapForceMappingImporter.ImportFromFile(mappingFile);

            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("Expected mapping: ");
            expectedMapping.PrettyPrint(Console.Out, "  ");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("Imported mapping: ");
            mapping.PrettyPrint(Console.Out, "  ");
            Console.WriteLine("----------------------------------------------------");

            Assert.AreEqual(expectedMapping, mapping);
        }
    }

    public class Edge : IEquatable<Edge>
    {
        public string EdgeKey { get; private set; }
        public string TargetVertexKey { get; private set; }

        public Edge(string edgeKey, string targetVertexKey)
        {
            EdgeKey = edgeKey;
            TargetVertexKey = targetVertexKey;
        }

        public bool Equals(Edge other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.EdgeKey, EdgeKey) && Equals(other.TargetVertexKey, TargetVertexKey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Edge)) return false;
            return Equals((Edge) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((EdgeKey != null ? EdgeKey.GetHashCode() : 0)*397) ^ (TargetVertexKey != null ? TargetVertexKey.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("Edge [Key: {0}, TargetVertexKey: {1}]", EdgeKey, TargetVertexKey);
        }

        public void PrettyPrint(TextWriter writer, string indent)
        {
            writer.WriteLine(indent + "Edge [" + EdgeKey + "] -> " + TargetVertexKey);
        }
    }

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
            if (obj.GetType() != typeof (Graph)) return false;
            return Equals((Graph) obj);
        }

        public override int GetHashCode()
        {
            return (Vertices != null ? Vertices.GetHashCode() : 0);
        }
    }

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
            if (obj.GetType() != typeof (Vertex)) return false;
            return Equals((Vertex) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Key != null ? Key.GetHashCode() : 0)*397) ^ (Edges != null ? Edges.GetHashCode() : 0);
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
            if (obj.GetType() != typeof (ConstantComponent)) return false;
            return Equals((ConstantComponent) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }

    public class InputOutputKey : IEquatable<InputOutputKey>
    {
        #region KeyType enum

        public enum KeyType
        {
            Input,
            Output,
        }

        #endregion

        private InputOutputKey(KeyType type, string value)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; private set; }
        public KeyType Type { get; private set; }

        public bool IsInput
        {
            get { return KeyType.Input == Type; }
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
            if (obj.GetType() != typeof (InputOutputKey)) return false;
            return Equals((InputOutputKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Value != null ? Value.GetHashCode() : 0)*397) ^ Type.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", Type, Value);
        }
    }

    public class MapForceMapping : IEquatable<MapForceMapping>
    {
        public Graph Graph { get; private set; }

        public MapForceMapping(IEnumerable<SchemaComponent> schemaComponents, IEnumerable<ConstantComponent> constantComponents, Graph graph)
        {
            Graph = graph;
            SchemaComponents = new List<SchemaComponent>(schemaComponents);
            ConstantComponents = new List<ConstantComponent>(constantComponents);
        }

        public IEnumerable<ConstantComponent> ConstantComponents { get; private set; }
        public IEnumerable<SchemaComponent> SchemaComponents { get; private set; }

        #region IEquatable<MapForceMapping> Members

        public bool Equals(MapForceMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || (other.SchemaComponents.IsEqualTo(SchemaComponents) && other.ConstantComponents.IsEqualTo(ConstantComponents));
        }

        #endregion

        public void PrettyPrint(TextWriter writer, string indent)
        {
            foreach (SchemaComponent schemaComponent in SchemaComponents)
            {
                schemaComponent.PrettyPrint(writer, indent);
            }
            foreach (var constantComponent in ConstantComponents)
            {
                constantComponent.PrettyPrint(writer, indent);
            }
            Graph.PrettyPrint(writer, indent);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (MapForceMapping)) return false;
            return Equals((MapForceMapping) obj);
        }

        public override int GetHashCode()
        {
            return (SchemaComponents != null ? SchemaComponents.GetHashCode() : 0);
        }
    }

    public static class LinqToXmlMapForceMappingImporter
    {
        public static MapForceMapping ImportFromFile(string mappingFile)
        {
            return ImportMapping(XDocument.Load(mappingFile));
        }

        private static MapForceMapping ImportMapping(XDocument mappingDocument)
        {
            return new MapForceMapping(ImportSchemaComponents(mappingDocument), ImportConstantComponents(mappingDocument), ImportGraph(mappingDocument));
        }

        private static Graph ImportGraph(XDocument document)
        {
            var graphElement = document.XPathSelectElement("mapping/component[@name='defaultmap1']/structure/graph");
            return new Graph(ImportVertices(graphElement));
        }

        private static IEnumerable<Vertex> ImportVertices(XElement graphElement)
        {
            var vertexElements = graphElement.XPathSelectElements("vertices/vertex");
            foreach (var vertexElement in vertexElements)
            {
                yield return new Vertex((string) vertexElement.Attribute("vertexkey"), ImportEdges(vertexElement));
            }
        }

        private static IEnumerable<Edge> ImportEdges(XElement vertexElement)
        {
            var edgeElements = vertexElement.XPathSelectElements("edges/edge");
            foreach (var edgeElement in edgeElements)
            {
                yield return new Edge((string) edgeElement.Attribute("edgekey"), (string) edgeElement.Attribute("vertexkey"));
            }
        }

        private static IEnumerable<ConstantComponent> ImportConstantComponents(XDocument mappingDocument)
        {
            IEnumerable<XElement> constantComponentElements = from c in mappingDocument.Descendants("component")
                                                              where (string) c.Attribute("name") == "constant"
                                                              select c;

            foreach (XElement constantComponentElement in constantComponentElements)
            {
                var constantElement = constantComponentElement.XPathSelectElement("data/constant");

                if (constantElement == null)
                {
                    // TODO report error
                }
                else
                {
                    yield return new ConstantComponent((string) constantElement.Attribute("value"));
                }
            }
        }

        private static IEnumerable<SchemaComponent> ImportSchemaComponents(XDocument mappingDocument)
        {
            IEnumerable<XElement> documentComponents = from c in mappingDocument.Descendants("component")
                                                       where (string) c.Attribute("name") == "document"
                                                       select c;

            foreach (XElement component in documentComponents)
            {
                var documents = new List<XElement>(from document in component.Descendants("document")
                                                   where document.Attribute("schema") != null
                                                   select document);
                if (documents.Count != 1)
                {
                    // TODO report error
                }
                else
                {
                    XElement document = documents[0];
                    IEnumerable<Namespace> namespaces = ImportComponentNamespaces(component);
                    yield return new SchemaComponent((string) document.Attribute("schema"), namespaces, ImportComponentEntries(component));
                }
            }
        }

        private static IEnumerable<Namespace> ImportComponentNamespaces(XElement component)
        {
            IEnumerable<XElement> namespaceElements = component.XPathSelectElements("data/root/header/namespaces/namespace");
            foreach (XElement namespaceElement in namespaceElements)
            {
                var id = (string) namespaceElement.Attribute("uid");
                if (!string.IsNullOrEmpty(id))
                {
                    yield return new Namespace(id);
                }
            }
        }

        private static Entry ImportComponentEntries(XElement component)
        {
            return ImportEntry(component.XPathSelectElement("data/root/entry"));
        }

        private static Entry ImportEntry(XElement entryElement)
        {
            InputOutputKey inputOutputKey;
            var inputKey = (string) entryElement.Attribute("inpkey");
            if (string.IsNullOrEmpty(inputKey))
            {
                var outputKey = (string) entryElement.Attribute("outkey");
                if (string.IsNullOrEmpty(outputKey))
                {
                    // TODO error
                }
                inputOutputKey = InputOutputKey.Output(outputKey);
            }
            else
            {
                inputOutputKey = InputOutputKey.Input(inputKey);
            }
            return new Entry((string) entryElement.Attribute("name"), inputOutputKey, from childElement in entryElement.Elements("entry") select ImportEntry(childElement));
        }
    }

    public class Namespace : IEquatable<Namespace>
    {
        public Namespace(string id)
        {
            ID = id;
        }

        public string ID { get; private set; }

        #region IEquatable<Namespace> Members

        public bool Equals(Namespace other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.ID, ID);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Namespace)) return false;
            return Equals((Namespace) obj);
        }

        public override int GetHashCode()
        {
            return (ID != null ? ID.GetHashCode() : 0);
        }
    }

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
            return obj.GetType() == typeof (SchemaComponent) && Equals((SchemaComponent) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Schema != null ? Schema.GetHashCode() : 0)*397) ^ (RootEntry != null ? RootEntry.GetHashCode() : 0);
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

    public class Entry : IEquatable<Entry>
    {
        private readonly InputOutputKey inputOutputKey;

        public Entry(string name, InputOutputKey inputOutputKey) : this(name, inputOutputKey, new Entry[0])
        {
        }

        public Entry(string name, InputOutputKey inputOutputKey, IEnumerable<Entry> subEntries)
        {
            this.inputOutputKey = inputOutputKey;
            Name = name;
            SubEntries = new List<Entry>(subEntries);
        }

        public string Name { get; private set; }
        public IEnumerable<Entry> SubEntries { get; private set; }

        public bool IsInput
        {
            get { return inputOutputKey.IsInput; }
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
            return obj.GetType() == typeof (Entry) && Equals((Entry) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (SubEntries != null ? SubEntries.GetHashCode() : 0);
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
    }
}