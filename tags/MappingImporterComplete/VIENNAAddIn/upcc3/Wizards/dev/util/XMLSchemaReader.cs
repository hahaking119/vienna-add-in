﻿// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Xml;
using System.Xml.Schema;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class XMLSchemaReader
    {
        private const double xsdAttributeWeight = 1;
        private const double xsdAnyWeight = 0.5;
        private const double xsdAnyAttributeWeight = 0;
        private const double xsdChoiceWeight = 0.5;
        private const double xsdRedefineWeight = 0.5;
        private const double xsdSubstitutionGroupWeight = 0.5;
        private const double xsiTypeWeight = 0;
        private const double xsdGroupWeight = 0;
        private const double xsdKeyWeight = 0;
        private const double xsdKeyRefWeight = 0;
        private const double xsdUnionWeight = 0;
        private const double xsdListWeight = 0;
        private const double xsdAttributeGroupWeight = 0.5;
        private const double xsdUniqueWeight = 0;
        private const double xsdComplexTypeWeight = 1;
        private const double xsdSimpleTypeWeight = 1;
        private const double xsdElementWeight = 1;
        private const double xsdSequenceWeight = 1;
        private const double xsdAllWeight = 0.5;
        private const double xsdRestrictionWeight = 0;
        private const double xsdExtensionWeight = 0;

        private int xsdAttribute;
        private int xsdAny;
        private int xsdAll;
        private int xsdAnyAttribute;
        private int xsdChoice;
        private int xsdRedefine;
        private int xsdSubstitutionGroup;
        private int xsiType;
        private int xsdGroup;
        private int xsdKey;
        private int xsdKeyRef;
        private int xsdUnion;
        private int xsdList;
        private int xsdAttributeGroup;
        private int xsdUnique;
        private int xsdComplexType;
        private int xsdSimpleType;
        private int xsdElement;
        private int xsdSequence;
        private int xsdRestriction;
        private int xsdExtension;


        public SchemaAnalyzerResults Read(string filename)
        {
            var results = new SchemaAnalyzerResults();
            
            var xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(filename),null));
           
            foreach (XmlSchema schema in xmlSchemaSet.Schemas())
            {
                //Console.WriteLine(schema.SourceUri);
                countXsdElements(schema.Items);
            }

            results.Clear();

            //Console.WriteLine("Schema features " + xsdAll + " All.");
            results.Add(new SchemaAnalyzerResult("All", xsdAll, xsdAllWeight));

            //Console.WriteLine("Schema features " + xsdAny + " xsAny.");
            results.Add(new SchemaAnalyzerResult("Any", xsdAny, xsdAnyWeight));

            //Console.WriteLine("Schema features " + xsdAnyAttribute + " AnyAttributes.");
            results.Add(new SchemaAnalyzerResult("AnyAttribute", xsdAnyAttribute, xsdAnyAttributeWeight));

            //Console.WriteLine("Schema features " + xsdAttribute + " Attributes.");
            results.Add(new SchemaAnalyzerResult("Attribute", xsdAttribute, xsdAttributeWeight));

            //Console.WriteLine("Schema features " + xsdAttributeGroup + " AttributeGroups.");
            results.Add(new SchemaAnalyzerResult("AttributeGroup", xsdAttributeGroup, xsdAttributeGroupWeight));

            //Console.WriteLine("Schema features " + xsdChoice + " Choices.");
            results.Add(new SchemaAnalyzerResult("Choice", xsdChoice, xsdChoiceWeight));

            //Console.WriteLine("Schema features " + xsdComplexType + " ComplexTypes.");
            results.Add(new SchemaAnalyzerResult("ComplexType", xsdComplexType, xsdComplexTypeWeight));

            //Console.WriteLine("Schema features " + xsdElement + " Elements.");
            results.Add(new SchemaAnalyzerResult("Element", xsdElement, xsdElementWeight));

            //Console.WriteLine("Schema features " + xsdExtension + " Extensions.");
            results.Add(new SchemaAnalyzerResult("Extension", xsdExtension, xsdExtensionWeight));

            //Console.WriteLine("Schema features " + xsdGroup + " Groups.");
            results.Add(new SchemaAnalyzerResult("Group", xsdGroup, xsdGroupWeight));

            //Console.WriteLine("Schema features " + xsdKey + " xsKey.");
            results.Add(new SchemaAnalyzerResult("Key", xsdKey, xsdKeyWeight));

            //Console.WriteLine("Schema features " + xsdKeyRef + " KeyRefs.");
            results.Add(new SchemaAnalyzerResult("KeyRef", xsdKeyRef, xsdKeyRefWeight));

            //Console.WriteLine("Schema features " + xsdList + " Lists.");
            results.Add(new SchemaAnalyzerResult("List", xsdList, xsdListWeight));

            //Console.WriteLine("Schema features " + xsdRedefine + " Redefines.");
            results.Add(new SchemaAnalyzerResult("Redefine", xsdRedefine, xsdRedefineWeight));

            //Console.WriteLine("Schema features " + xsdRestriction + " Restrictions.");
            results.Add(new SchemaAnalyzerResult("Restriction",xsdRestriction,xsdRestrictionWeight));

            //Console.WriteLine("Schema features " + xsdSequence + " Sequences.");
            results.Add(new SchemaAnalyzerResult("Sequence", xsdSequence, xsdSequenceWeight));

            //Console.WriteLine("Schema features " + xsdSimpleType + " SimpleTypes.");
            results.Add(new SchemaAnalyzerResult("SimpleType", xsdSimpleType, xsdSimpleTypeWeight));

            //Console.WriteLine("Schema features " + xsdSubstitutionGroup + " SubstitutionGroups.");
            results.Add(new SchemaAnalyzerResult("SubstitutionGroup", xsdSubstitutionGroup, xsdSubstitutionGroupWeight));

            //Console.WriteLine("Schema features " + xsdUnion + " Unions.");
            results.Add(new SchemaAnalyzerResult("Union", xsdUnion, xsdUnionWeight));

            //Console.WriteLine("Schema features " + xsdUnique + " Uniques.");
            results.Add(new SchemaAnalyzerResult("Unique", xsdUnique, xsdUniqueWeight));

            //Console.WriteLine("Schema features " + xsiType + " xsi:Types.");
            results.Add(new SchemaAnalyzerResult("xsi:Type", xsiType, xsiTypeWeight));


            results.Sort(new SchemaAnalyzerResultComparer());

            return results;
        }
        private void countXsdElements(XmlSchemaObjectCollection items)
        {
            foreach (var item in items)
            {
                if (item is XmlSchemaComplexType)
                {
                    xsdComplexType++;
                    var complexType = (XmlSchemaComplexType)item;

                    if (complexType.Particle is XmlSchemaAll)
                    {
                        xsdAll++;
                    }
                    if (complexType.Particle is XmlSchemaChoice)
                    {
                        xsdChoice++;
                    }
                    if (complexType.Particle is XmlSchemaSequence)
                    {
                        xsdSequence++;
                    }
                    countXsdElements(new XmlSchemaObjectCollection(complexType.Particle));
                    countXsdElements(complexType.Attributes);

                    var obj = (XmlSchemaObject) complexType.ContentModel;
                    if (obj != null)
                    {
                        var list = new XmlSchemaObjectCollection();
                        list.Add(obj);
                        countXsdElements(list);
                    }
                }
                if(item is XmlSchemaSimpleContentExtension)
                {
                    xsdExtension++;
                    var extension = (XmlSchemaSimpleContentExtension)item;
                    countXsdElements(extension.Attributes);
                }
                if(item is XmlSchemaSimpleContentRestriction)
                {
                    xsdRestriction++;
                    var restriction = (XmlSchemaSimpleContentRestriction) item;
                    countXsdElements(restriction.Attributes);
                    countXsdElements(restriction.Facets);
                }
                if (item is XmlSchemaSimpleContent)
                {
                    var xmlSchemaSimpleContent = (XmlSchemaSimpleContent) item;

                    var obj = (XmlSchemaObject) xmlSchemaSimpleContent.Content;
                    if (obj != null)
                    {
                        var list = new XmlSchemaObjectCollection();
                        list.Add(obj);
                        countXsdElements(list);
                    }
                }
                if(item is XmlSchemaAttributeGroup)
                {
                    xsdAttributeGroup++;
                    var xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup) item;
                    countXsdElements(xmlSchemaAttributeGroup.Attributes);
                }
                if(item is XmlSchemaSimpleTypeUnion)
                {
                    var xmlSchemaSimpleTypeUnion = (XmlSchemaSimpleTypeUnion) item;
                    if(xmlSchemaSimpleTypeUnion.MemberTypes.Length > 0)
                    {
                        xsiType++;
                    }
                    xsdUnion++;
                    countXsdElements(new XmlSchemaObjectCollection(xmlSchemaSimpleTypeUnion));
                }
                if(item is XmlSchemaSimpleTypeList)
                {
                    xsdList++;
                }
                if(item is XmlSchemaSimpleTypeRestriction)
                {
                    xsdRestriction++;
                    var xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction) item;
                    countXsdElements(xmlSchemaSimpleTypeRestriction.Facets);
                }
                if (item is XmlSchemaSimpleType)
                {
                    var xmlSchemaSimpleType = (XmlSchemaSimpleType) item;
                    xsdSimpleType++;

                    var obj = (XmlSchemaObject)xmlSchemaSimpleType.Content;
                    if (obj != null)
                    {
                        var list = new XmlSchemaObjectCollection();
                        list.Add(obj);
                        countXsdElements(list);
                    }
                }
                if(item is XmlSchemaAny)
                {
                    xsdAny++;
                }
                if(item is XmlSchemaAnyAttribute)
                {
                    xsdAnyAttribute++;
                }
                if(item is XmlSchemaRedefine)
                {
                    xsdRedefine++;
                    var xmlSchemaRedefine = (XmlSchemaRedefine) item;
                    countXsdElements(xmlSchemaRedefine.Items);
                }
                if(item is XmlSchemaUnique)
                {
                    xsdUnique++;
                    var xmlSchemaUnique = (XmlSchemaUnique) item;
                    countXsdElements(xmlSchemaUnique.Fields);
                }
                if(item is XmlSchemaKey)
                {
                    xsdKey++;
                    var xmlSchemaKey = (XmlSchemaKey) item;
                    countXsdElements(xmlSchemaKey.Fields);
                }
                if(item is XmlSchemaKeyref)
                {
                    xsdKeyRef++;
                    var xmlSchemaKeyref = (XmlSchemaKeyref)item;
                    countXsdElements(xmlSchemaKeyref.Fields);
                }
                if (item is XmlSchemaElement)
                {
                    var element = (XmlSchemaElement)item;
                    if (element.RefName.IsEmpty)
                    {
                        xsdElement++;
                    }
                    if(!element.SubstitutionGroup.IsEmpty)
                    {
                        xsdSubstitutionGroup++;
                    }
                    if(element.ElementSchemaType is XmlSchemaComplexType) {
                        var xmlSchemaComplexType = element.ElementSchemaType as XmlSchemaComplexType;

                        var obj = (XmlSchemaObject)xmlSchemaComplexType;
                        if (obj != null)
                        {
                            var list = new XmlSchemaObjectCollection();
                            list.Add(obj);
                            countXsdElements(list);
                        }
                    }
                    if (element.ElementSchemaType is XmlSchemaSimpleType)
                    {
                        var xmlSchemaSimpleType = element.ElementSchemaType as XmlSchemaSimpleType;

                        var obj = (XmlSchemaObject)xmlSchemaSimpleType;
                        if (obj != null)
                        {
                            var list = new XmlSchemaObjectCollection();
                            list.Add(obj);
                            countXsdElements(list);
                        }
                    }
                }
                if (item is XmlSchemaAttribute)
                {
                    var attribute = (XmlSchemaAttribute)item;
                    if (attribute.RefName.IsEmpty)
                    {
                        xsdAttribute++;
                    }
                    var obj = (XmlSchemaObject)attribute.AttributeSchemaType;
                    if (obj != null)
                    {
                        var list = new XmlSchemaObjectCollection();
                        list.Add(obj);
                        countXsdElements(list);
                    }
                    var obj2 = (XmlSchemaObject) attribute.SchemaType;
                    if (obj2 != null)
                    {
                        var list = new XmlSchemaObjectCollection();
                        list.Add(obj2);
                        countXsdElements(list);
                    }
                }
                if (item is XmlSchemaGroup)
                {
                    xsdGroup++;
                    var xmlSchemaGroup = (XmlSchemaGroup) item;
                    countXsdElements(new XmlSchemaObjectCollection(xmlSchemaGroup.Particle));
                }
                #region SAXImplementation:old
                //switch (element.Name)
                // {
                //     case "xs:element":
                //         xsdElement++;
                //         break;
                //     case "xs:complexType":
                //         xsdComplexType++;
                //         break;
                //     case "xs:simpleType":
                //         xsdSimpleType++;
                //         break;
                //     case "xs:choice":
                //         xsdChoice++;
                //         break;
                //     case "xs:key":
                //         xsdKey++;
                //         break;
                //     case "xs:any":
                //         xsdAny++;
                //         break;
                //     case "xs:group":
                //         xsdGroup++;
                //         break;
                //     case "xs:sequence":
                //         xsdSequence++;
                //         break;
                //     case "xs:redefine":
                //         xsdRedefine++;
                //         break;
                //     case "xs:substitutionGroup":
                //         xsdSubstitutionGroup++;
                //         break;
                //     case "xsi:type":
                //         xsiType++;
                //         break;
                //     case "xs:attribute":
                //         xsdAttribute++;
                //         break;
                //     case "xs:anyAttribute":
                //         xsdAnyAttribute++;
                //         break;
                //     case "xs:keyRef":
                //         xsdKeyRef++;
                //         break;
                //     case "xs:union":
                //         xsdUnion++;
                //         break;
                //     case "xs:list":
                //         xsdList++;
                //         break;
                //     case "xs:attributeGroup":
                //         xsdAttributeGroup++;
                //         break;
                //     case "xs:unique":
                //         xsdUnique++;
                //         break;
                // }
                #endregion
            }
        }
    }
}