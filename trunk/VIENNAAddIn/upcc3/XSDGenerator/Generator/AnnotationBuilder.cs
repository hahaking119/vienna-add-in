using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    internal class AnnotationBuilder
    {
        private readonly XmlDocument xml = new XmlDocument();
        private readonly List<XmlNode> xmlNodes = new List<XmlNode>();

        public XmlSchemaAnnotation Annotation
        {
            get
            {
                var ann = new XmlSchemaAnnotation();
                ann.Items.Add(new XmlSchemaDocumentation {Language = "en", Markup = xmlNodes.ToArray()});
                return ann;
            }
        }

        public void addOptionalAnnotation(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                XmlNode node = xml.CreateElement("ccts", name,
                                                 "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:3");
                node.InnerText = value;
                xmlNodes.Add(node);
            }
        }
    } ;
}