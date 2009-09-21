using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    /// <summary>
    /// This class manages the data originating from the MapForce XML mapping file. In order to
    /// do so it uses Linq to extract the data and put it into a MapForceMapping object.
    /// </summary>
    public static class LinqToXmlMapForceMappingImporter
    {
        /// <summary>
        /// Takes a MapForce mapping file as input and returns a MapForceMapping object.
        /// </summary>
        /// <param name="mappingFile"></param>
        /// <returns></returns>
        public static MapForceMapping ImportFromFile(string mappingFile)
        {
            return ImportMapping(XDocument.Load(mappingFile));
        }

        /// <summary>
        /// Takes an XDocument as Input and returns a MapForceMapping file.
        /// </summary>
        /// <param name="mappingDocument"></param>
        /// <returns></returns>
        private static MapForceMapping ImportMapping(XDocument mappingDocument)
        {
            return new MapForceMapping(ImportSchemaComponents(mappingDocument), ImportConstantComponents(mappingDocument), ImportGraph(mappingDocument));
        }

        /// <summary>
        /// Takes an XDocument as input and returns a Graph containing all the MapForce Mappings consisting
        /// of Vertices and Edges.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private static Graph ImportGraph(XDocument document)
        {
            var graphElement = document.XPathSelectElement("mapping/component[@name='defaultmap1']/structure/graph");
            return new Graph(ImportVertices(graphElement));
        }

        /// <summary>
        /// Takes an XElement representing the Root of the MapForce mapping graph as input and returns
        /// a set of contained Vertices.
        /// </summary>
        /// <param name="graphElement"></param>
        /// <returns></returns>
        private static IEnumerable<Vertex> ImportVertices(XElement graphElement)
        {
            var vertexElements = graphElement.XPathSelectElements("vertices/vertex");
            foreach (var vertexElement in vertexElements)
            {
                yield return new Vertex((string)vertexElement.Attribute("vertexkey"), ImportEdges(vertexElement));
            }
        }

        /// <summary>
        /// Takes an XElement representing an individual Vertex as input and returns a set
        /// of associated Edges for this specific Vertex.
        /// </summary>
        /// <param name="vertexElement"></param>
        /// <returns></returns>
        private static IEnumerable<Edge> ImportEdges(XElement vertexElement)
        {
            var edgeElements = vertexElement.XPathSelectElements("edges/edge");
            foreach (var edgeElement in edgeElements)
            {
                yield return new Edge((string)edgeElement.Attribute("edgekey"), (string)edgeElement.Attribute("vertexkey"));
            }
        }

        /// <summary>
        /// Takes an XDocument representing the whole MapForce mapping model as input and returns a set
        /// of ConstantComponents.
        /// </summary>
        /// <param name="mappingDocument"></param>
        /// <returns></returns>
        private static IEnumerable<ConstantComponent> ImportConstantComponents(XDocument mappingDocument)
        {
            IEnumerable<XElement> constantComponentElements = from c in mappingDocument.Descendants("component")
                                                              where (string)c.Attribute("name") == "constant"
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
                    yield return new ConstantComponent((string)constantElement.Attribute("value"));
                }
            }
        }

        /// <summary>
        /// Takes an XDocument representing the whole MapForce mapping model as input and returns
        /// a set of SchemaComponent objects representing real XSD document models.
        /// </summary>
        /// <param name="mappingDocument"></param>
        /// <returns></returns>
        private static IEnumerable<SchemaComponent> ImportSchemaComponents(XDocument mappingDocument)
        {
            IEnumerable<XElement> documentComponents = from c in mappingDocument.Descendants("component")
                                                       where (string)c.Attribute("name") == "document"
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
                    yield return new SchemaComponent((string)document.Attribute("schema"), namespaces, ImportComponentEntries(component));
                }
            }
        }

        /// <summary>
        /// Takes an XElement representing a document component as input and returns the associated namespaces.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private static IEnumerable<Namespace> ImportComponentNamespaces(XElement component)
        {
            IEnumerable<XElement> namespaceElements = component.XPathSelectElements("data/root/header/namespaces/namespace");
            foreach (XElement namespaceElement in namespaceElements)
            {
                var id = (string)namespaceElement.Attribute("uid");
                if (!string.IsNullOrEmpty(id))
                {
                    yield return new Namespace(id);
                }
            }
        }

        /// <summary>
        /// Takes an XElement representing a document component as input and returns the root Entry object of that
        /// component with all subentries in a tree.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private static Entry ImportComponentEntries(XElement component)
        {
            return ImportEntry(component.XPathSelectElement("data/root/entry"));
        }

        /// <summary>
        /// Takes an XElement representing the root entry as input and returns the root Entry object with all its childs
        /// by recursively calling itself.
        /// </summary>
        /// <param name="entryElement"></param>
        /// <returns></returns>
        private static Entry ImportEntry(XElement entryElement)
        {
            InputOutputKey inputOutputKey;
            var inputKey = (string)entryElement.Attribute("inpkey");
            if (string.IsNullOrEmpty(inputKey))
            {
                var outputKey = (string)entryElement.Attribute("outkey");
                inputOutputKey = string.IsNullOrEmpty(outputKey) ? InputOutputKey.None : InputOutputKey.Output(outputKey);
            }
            else
            {
                inputOutputKey = InputOutputKey.Input(inputKey);
            }
            return new Entry((string)entryElement.Attribute("name"), inputOutputKey, from childElement in entryElement.Elements("entry") select ImportEntry(childElement));
        }
    }
}