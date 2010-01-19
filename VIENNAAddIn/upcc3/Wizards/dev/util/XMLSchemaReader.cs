// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.IO;
using System.Xml;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class XMLSchemaReader
    {
        public XMLSchemaReader(string filename)
        {
            int xselement = 0;
            int xscomplextype=0;
            int xssimpletype = 0;
            int xschoice = 0;
            int xsall = 0;
            int xsany = 0;
            int xsgroup = 0;
            int xssequence = 0;
            int xsredefine = 0;
            int xssubstitutionGroup = 0;
            int xsitype = 0;

            int complexContent = 0;
            int simpleContent = 0;

            int extension = 0;
            int attribute = 0;

            var results = new SchemaAnalyzerResults();


            XmlReader reader = XmlReader.Create(new StreamReader(filename));
            // read (pull) the next node in document order
            while (reader.Read())
            {
                if (reader.HasAttributes)
                {
                    Console.WriteLine(reader.ReadOuterXml());
                }
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "xs:element":
                            xselement++;
                            break;
                        case "xs:complexType":
                            xscomplextype++;
                            break;
                        case "xs:simpleType":
                            xssimpletype++;
                            break;
                        case "xs:choice":
                            xschoice++;
                            break;
                        case "xs:all":
                            xsall++;
                            break;
                        case "xs:any":
                            xsany++;
                            break;
                        case "xs:group":
                            xsgroup++;
                            break;
                        case "xs:sequence":
                            xssequence++;
                            break;
                        case "xs:redefine":
                            xsredefine++;
                            break;
                        case "xs:substitutionGroup":
                            xssubstitutionGroup++;
                            break;
                        case "xsi:type":
                            xsitype++;
                            break;
                    }
                }
                // get the current node's (namespace) name
            }
            
            Console.WriteLine("Schema features " + xselement + " Elements.");
            Console.WriteLine("Schema features " + xssimpletype + " SimpleTypes.");
            results.Add(new SchemaAnalyzerResult("SimpleType", xssimpletype));
            Console.WriteLine("Schema features " + xscomplextype + " ComplexTypes.");
            results.Add(new SchemaAnalyzerResult("ComplexType", xscomplextype));
            Console.WriteLine("Schema features " + xschoice + " Choices.");
            results.Add(new SchemaAnalyzerResult("Choice", xschoice));
            Console.WriteLine("Schema features " + xsall + " xsAll.");
            Console.WriteLine("Schema features " + xsany + " xsAny.");
            Console.WriteLine("Schema features " + xsgroup + " Groups.");
            Console.WriteLine("Schema features " + xsgroup + " Sequences.");
            Console.WriteLine("Schema features " + xssubstitutionGroup + " SubstitutionGroups.");
            Console.WriteLine("Schema features " + xsredefine + " Redefines.");
            Console.WriteLine("Schema features " + xsitype + " xsi:Types.");
        }
    }
}
