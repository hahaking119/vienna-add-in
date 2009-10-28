using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    /// <summary>
    /// This class manages the data originating from the MapForce XML mapping
    /// file. In order to do so it uses <see cref="System.Linq"/> to extract the data
    /// and put it into a <see cref="MapForceMapping"/> object.
    /// </summary>
    public static class LinqToXmlMapForceMappingImporter
    {
        /// <summary>
        /// Takes a MapForce mapping file as input and returns a <see cref="MapForceMapping"/> object.
        /// </summary>
        /// <param name="mappingFiles"></param>
        /// <returns></returns>
        public static MapForceMapping ImportFromFiles(params string[] mappingFiles)
        {
            return new MapForceMapping(ImportSchemaComponents(mappingFiles), ImportConstantComponents(mappingFiles), ImportGraph(mappingFiles));
        }

        /// <summary>
        /// Takes an <see cref="XDocument"/> as input and returns a Graph containing all the MapForce Mappings consisting
        /// of Vertices and Edges.
        /// </summary>
        /// <param name="mappingFiles"></param>
        /// <returns></returns>
        private static Graph ImportGraph(string[] mappingFiles)
        {
            return new Graph(ImportVertices(mappingFiles));
        }

        private static IEnumerable<Vertex> ImportVertices(string[] mappingFiles)
        {
            foreach (var mappingFile in mappingFiles)
            {
                var mappingDocument = XDocument.Load(mappingFile);
                var graphElement = mappingDocument.XPathSelectElement("mapping/component[@name='defaultmap1']/structure/graph");
                var vertexElements = graphElement.XPathSelectElements("vertices/vertex");
                foreach (var vertexElement in vertexElements)
                {
                    yield return new Vertex((string)vertexElement.Attribute("vertexkey"), ImportEdges(vertexElement, mappingFile), mappingFile);
                }
            }
        }

        /// <summary>
        /// Takes an <see cref="XElement"/> representing an individual Vertex as input and returns a set
        /// of associated Edges for this specific Vertex.
        /// </summary>
        /// <param name="vertexElement"></param>
        /// <returns></returns>
        private static IEnumerable<Edge> ImportEdges(XElement vertexElement, string mappingFile)
        {
            var edgeElements = vertexElement.XPathSelectElements("edges/edge");
            foreach (var edgeElement in edgeElements)
            {
                yield return new Edge((string)edgeElement.Attribute("edgekey"), (string)edgeElement.Attribute("vertexkey"), mappingFile);
            }
        }

        /// <summary>
        /// Takes an <see cref="XDocument"/> representing the whole MapForce mapping model as input and returns a set
        /// of ConstantComponents.
        /// </summary>
        /// <param name="mappingFiles"></param>
        /// <returns></returns>
        private static IEnumerable<ConstantComponent> ImportConstantComponents(string[] mappingFiles)
        {
            foreach (var mappingFile in mappingFiles)
            {
                var mappingDocument = XDocument.Load(mappingFile);
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
        }

        /// <summary>
        /// Takes an <see cref="XDocument"/> representing the whole MapForce mapping model as input and returns
        /// a set of <see cref="SchemaComponent"/> objects representing real XSD document models.
        /// </summary>
        /// <param name="mappingFiles"></param>
        /// <returns></returns>
        private static IEnumerable<SchemaComponent> ImportSchemaComponents(IEnumerable<string> mappingFiles)
        {
            foreach (var mappingFile in mappingFiles)
            {
                var mappingDocument = XDocument.Load(mappingFile);
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
                        yield return new SchemaComponent((string) document.Attribute("schema"), namespaces, ImportComponentEntries(component, mappingFile));
                    }
                }
            }
        }

        /// <summary>
        /// Takes an <see cref="XElement"/> representing a document component as input and returns the associated namespaces.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Takes an <see cref="XElement"/> representing a document component as input and returns the root Entry object of that
        /// component with all sub-entries in a tree.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="mappingFile"></param>
        /// <returns></returns>
        private static Entry ImportComponentEntries(XElement component, string mappingFile)
        {
            return ImportEntry(component.XPathSelectElement("data/root/entry"), mappingFile);
        }

        /// <summary>
        /// Takes an <see cref="XElement"/> representing the root entry as input and returns the root Entry object with all its children
        /// by recursively calling itself.
        /// </summary>
        /// <param name="entryElement"></param>
        /// <param name="mappingFile"></param>
        /// <returns></returns>
        private static Entry ImportEntry(XElement entryElement, string mappingFile)
        {
            InputOutputKey inputOutputKey;
            var inputKey = (string) entryElement.Attribute("inpkey");
            if (string.IsNullOrEmpty(inputKey))
            {
                var outputKey = (string) entryElement.Attribute("outkey");
                inputOutputKey = string.IsNullOrEmpty(outputKey) ? InputOutputKey.None : InputOutputKey.Output(mappingFile, outputKey);
            }
            else
            {
                inputOutputKey = InputOutputKey.Input(mappingFile, inputKey);
            }
            return new Entry((string) entryElement.Attribute("name"), inputOutputKey, from childElement in entryElement.Elements("entry") select ImportEntry(childElement, mappingFile));
        }
    }
}