// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace VIENNAAddIn.upcc3.XSDImporter.util
{
    public class QualifiedName
    {
        public string Prefix { get; set; }
        public string Name { get; set; }

        public QualifiedName(string prefix, string name)
        {
            Prefix = prefix;
            Name = name;
        }
    }

    public class Include
    {
        public string SchemaLocation { get; set; }
    }

    public class ComplexType
    {
        public string Name { get; set; }

        public ComplexType()
        {
            Name = "";
        }

        //public ComplexType(string name)
        //{
        //    Name = name;
        //}
    }

    public class Element
    {
        public string Name { get; set; }
        public QualifiedName Ref { get; set; }
        public QualifiedName Type { get; set; }
        public string MinOccurs { get; set; }
        public string MaxOccurs { get; set; }      

        public Element()
        {
            Name = "";
            Ref = new QualifiedName("", "");
            Type = new QualifiedName("", "");
            MinOccurs = "";
            MaxOccurs = "";
        }

        //public Element(string name, QualifiedName type, string minOccurs, string maxOccurs)
        //{
        //    Name = name;
        //    Type = type;
        //    MinOccurs = minOccurs;
        //    MaxOccurs = maxOccurs;
        //}
    }

    public static class ExtensionMethods
    {
        public static Include ParseInclude(this XmlNodeReader reader)
        {
            Include include = new Include();

            if (reader.MoveToAttribute("schemaLocation"))
            {
                include.SchemaLocation = reader.ReadContentAsString();
            }

            return include;
        }

        public static ComplexType ParseComplexType(this XmlNodeReader reader)
        {
            ComplexType newComplexType = new ComplexType();

            if (reader.MoveToAttribute("name"))
            {
                newComplexType.Name = reader.ReadContentAsString();
            }

            return newComplexType;
        }
        
        public static Element ParseElement(this XmlNodeReader reader)
        {
            Element element = new Element();

            if (reader.MoveToAttribute("name"))
            {
                element.Name = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ref"))
            {
                string reference = reader.ReadContentAsString();
                int endPrefix = reference.IndexOf(':');

                element.Ref.Prefix = reference.Substring(0, endPrefix);
                element.Ref.Name = reference.Substring(endPrefix + 1);                
            }

            if (reader.MoveToAttribute("type"))
            {
                string type = reader.ReadContentAsString();
                int endPrefix = type.IndexOf(':');

                element.Type.Prefix = type.Substring(0, endPrefix);
                element.Type.Name = type.Substring(endPrefix + 1);
            }

            if (reader.MoveToAttribute("minOccurs"))
            {
                element.MinOccurs = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("maxOccurs"))
            {
                element.MaxOccurs = reader.ReadContentAsString();
            }

            return element;
        }

        public static void MoveToSchemaRoot(this XmlNodeReader reader)
        {
            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "xsd:schema"))
                {
                    return;
                }
            }
        }

        public static IDictionary<string, string> ParseSchemaHeader(this XmlNodeReader reader)
        {
            IDictionary<string, string> nsTable = new Dictionary<string, string>();

            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                
                if (reader.Name.Contains("xmlns:"))
                {
                    nsTable.Add(reader.Name.Substring(6), reader.ReadContentAsString());
                }
            }

            return nsTable;
        }
    }


    public class CustomSchemaReader
    {
        private XmlNodeReader reader;

        public IDictionary<string, string> NamespaceTable { get; private set; }
        public IList<object> Items { get; private set; }
        public IList<Include> Includes { get; private set; }
        
        public CustomSchemaReader(XmlDocument xmlDocument)
        {
            Items = new List<object>();
            Includes = new List<Include>();

            reader = new XmlNodeReader(xmlDocument);

            reader.MoveToSchemaRoot();

            NamespaceTable = reader.ParseSchemaHeader();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "xsd:element":
                            {
                                Element element = reader.ParseElement();

                                Items.Add(element);

                                break;
                            }
                        case "xsd:complexType":
                            {
                                ComplexType newComplexType = reader.ParseComplexType();

                                Items.Add(newComplexType);

                                break;
                            }
                        case "xsd:include":
                            {
                                Include include = reader.ParseInclude();

                                Includes.Add(include);
                                
                                break;
                            }
                    }
                }
            }
        }
    }
}