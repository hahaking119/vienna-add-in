using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
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
                yield return new Vertex((string)vertexElement.Attribute("vertexkey"), ImportEdges(vertexElement));
            }
        }

        private static IEnumerable<Edge> ImportEdges(XElement vertexElement)
        {
            var edgeElements = vertexElement.XPathSelectElements("edges/edge");
            foreach (var edgeElement in edgeElements)
            {
                yield return new Edge((string)edgeElement.Attribute("edgekey"), (string)edgeElement.Attribute("vertexkey"));
            }
        }

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

        private static Entry ImportComponentEntries(XElement component)
        {
            return ImportEntry(component.XPathSelectElement("data/root/entry"));
        }

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