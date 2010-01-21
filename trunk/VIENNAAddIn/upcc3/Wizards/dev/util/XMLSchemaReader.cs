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
    public static class XMLSchemaReader
    {
        private const int xsdAttributeWeight = 1;
        private const int xsdAnyWeight = 2;
        private const int xsdAnyAttributeWeight = 3;
        private const int xsdChoiceWeight = 4;
        private const int xsdRedefineWeight = 5;
        private const int xsdSubstitutionGroupWeight = 6;
        private const int xsiTypeWeight = 7;
        private const int xsdGroupWeight = 8;
        private const int xsdKeyWeight = 9;
        private const int xsdKeyRefWeight = 10;
        private const int xsdUnionWeight = 11;
        private const int xsdListWeight = 12;
        private const int xsdAttributeGroupWeight = 13;
        private const int xsdUniqueWeight = 14;
        private const int xsdComplexTypeWeight = 15;
        private const int xsdSimpleTypeWeight = 16;
        private const int xsdElementWeight = 17;
        private const int xsdSequenceWeight = 18;

        
        public static SchemaAnalyzerResults Read(string filename)
        {
            int xsdAttribute = 0;
            int xsdAny = 0;
            int xsdAnyAttribute = 0;
            int xsdChoice = 0;
            int xsdRedefine = 0;
            int xsdSubstitutionGroup = 0;
            int xsiType = 0;
            int xsdGroup = 0;
            int xsdKey = 0;
            int xsdKeyRef = 0;
            int xsdUnion = 0;
            int xsdList = 0;
            int xsdAttributeGroup = 0;
            int xsdUnique = 0;
            int xsdComplexType = 0;
            int xsdSimpleType = 0;
            int xsdElement = 0;
            int xsdSequence = 0;

            var results = new SchemaAnalyzerResults();


            XmlReader reader = XmlReader.Create(new StreamReader(filename));
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
                            xsdElement++;
                            break;
                        case "xs:complexType":
                            xsdComplexType++;
                            break;
                        case "xs:simpleType":
                            xsdSimpleType++;
                            break;
                        case "xs:choice":
                            xsdChoice++;
                            break;
                        case "xs:key":
                            xsdKey++;
                            break;
                        case "xs:any":
                            xsdAny++;
                            break;
                        case "xs:group":
                            xsdGroup++;
                            break;
                        case "xs:sequence":
                            xsdSequence++;
                            break;
                        case "xs:redefine":
                            xsdRedefine++;
                            break;
                        case "xs:substitutionGroup":
                            xsdSubstitutionGroup++;
                            break;
                        case "xsi:type":
                            xsiType++;
                            break;
                        case "xsd:attribute":
                            xsdAttribute++;
                            break;
                        case "xsd:anyAttribute":
                            xsdAnyAttribute++;
                            break;
                        case "xsd:substitutionGroup":
                            xsdSubstitutionGroup++;
                            break;
                        case "xsd:keyRef":
                            xsdKeyRef++;
                            break;
                        case "xsd:union":
                            xsdUnion++;
                            break;
                        case "xsd:list":
                            xsdList++;
                            break;
                        case "xsd:attributeGroup":
                            xsdAttributeGroup++;
                            break;
                        case "xsd:unique":
                            xsdUnique++;
                            break;
                    }
                }
            }

            results.Clear();
            
            Console.WriteLine("Schema features " + xsdElement + " Elements.");
            results.Add(new SchemaAnalyzerResult("Element", xsdElement, xsdElementWeight));
            
            Console.WriteLine("Schema features " + xsdSimpleType + " SimpleTypes.");
            results.Add(new SchemaAnalyzerResult("SimpleType", xsdSimpleType,xsdSimpleTypeWeight));
            
            Console.WriteLine("Schema features " + xsdComplexType + " ComplexTypes.");
            results.Add(new SchemaAnalyzerResult("ComplexType", xsdComplexType,xsdComplexTypeWeight));
            
            Console.WriteLine("Schema features " + xsdChoice + " Choices.");
            results.Add(new SchemaAnalyzerResult("Choice", xsdChoice, xsdChoiceWeight));
            
            Console.WriteLine("Schema features " + xsdKey + " xsKey.");
            results.Add(new SchemaAnalyzerResult("Key", xsdKey, xsdKeyWeight));
            
            Console.WriteLine("Schema features " + xsdAny + " xsAny.");
            results.Add(new SchemaAnalyzerResult("Any", xsdAny, xsdAnyWeight));
            
            Console.WriteLine("Schema features " + xsdGroup + " Groups.");
            results.Add(new SchemaAnalyzerResult("Group", xsdGroup, xsdGroupWeight));
            
            Console.WriteLine("Schema features " + xsdSequence + " Sequences.");
            results.Add(new SchemaAnalyzerResult("Sequence", xsdSequence, xsdSequenceWeight));
            
            Console.WriteLine("Schema features " + xsdSubstitutionGroup + " SubstitutionGroups.");
            results.Add(new SchemaAnalyzerResult("SubstitutionGroup", xsdSubstitutionGroup, xsdSubstitutionGroupWeight));
            
            Console.WriteLine("Schema features " + xsdRedefine + " Redefines.");
            results.Add(new SchemaAnalyzerResult("Redefine", xsdRedefine, xsdRedefineWeight));
            
            Console.WriteLine("Schema features " + xsiType + " xsi:Types.");
            results.Add(new SchemaAnalyzerResult("xsi:Type", xsiType, xsiTypeWeight));
            
            Console.WriteLine("Schema features " + xsdAttribute + " Attributes.");
            results.Add(new SchemaAnalyzerResult("Attribute", xsdAttribute, xsdAttributeWeight));
            
            Console.WriteLine("Schema features " + xsdAnyAttribute + " AnyAttributes.");
            results.Add(new SchemaAnalyzerResult("AnyAttribute", xsdAnyAttribute, xsdAnyAttributeWeight));

            Console.WriteLine("Schema features " + xsdKeyRef + " KeyRefs.");
            results.Add(new SchemaAnalyzerResult("KeyRef", xsdKeyRef, xsdKeyRefWeight));

            Console.WriteLine("Schema features " + xsdUnion + " Unions.");
            results.Add(new SchemaAnalyzerResult("Union", xsdUnion, xsdUnionWeight));

            Console.WriteLine("Schema features " + xsdList + " Lists.");
            results.Add(new SchemaAnalyzerResult("List", xsdList, xsdListWeight));

            Console.WriteLine("Schema features " + xsdAttributeGroup + " AttributeGroups.");
            results.Add(new SchemaAnalyzerResult("AttributeGroup", xsdAttributeGroup, xsdAttributeGroupWeight));

            Console.WriteLine("Schema features " + xsdUnique + " Uniques.");
            results.Add(new SchemaAnalyzerResult("Unique", xsdUnique, xsdUniqueWeight));

            results.Sort(new SchemaAnalyzerResultComparer());

            return results;
        }
    }
}
