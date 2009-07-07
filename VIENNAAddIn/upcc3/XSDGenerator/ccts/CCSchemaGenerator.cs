﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDGenerator.ccts
{
    class CCSchemaGenerator
    {
        private const string NSPREFIX_CDT = "cdt";
        private const string NSPREFIX_DOC = "ccts";

        private const string NS_DOC = "urn:un:unece:uncefact:documentation:standard:XMLNDRDocumentation:3";

        private const string NSPREFIX_XSD = "xsd";
        private const string NS_XSD = "http://www.w3.org/2001/XMLSchema";

        private static List<String> globalASCCs = new List<String>();
   

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="bies"></param>
        public static void GenerateXSD(GeneratorContext context, IEnumerable<IACC> accs)
        {
            // Create XML schema file and prepare the XML schema header
            // R 88E2: all XML schema files must use UTF-8 encoding
            // R B387: every XML schema must have a declared target namespace


            var schema = new XmlSchema { TargetNamespace = context.TargetNamespace };
            schema.Namespaces.Add(context.NamespacePrefix, context.TargetNamespace);
            schema.Version = context.DocLibrary.VersionIdentifier.DefaultTo("1");
            
            // TODO: discuss R A0E5 and R A9C5 with Christian E. and Michi S. since this is something that should be added to the context
            // R A0E5: all XML schemas must contain elementFormDefault and set it to qualified     
            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            // R A9C5: All XML schemas must contain attributeFormDefault and set it to unqualified
            schema.AttributeFormDefault = XmlSchemaForm.Unqualified;

            // R 9B18: all XML schemas must utilize the xsd prefix when referring to the W3C XML schema namespace            
            schema.Namespaces.Add(NSPREFIX_XSD, NS_XSD);

            schema.Namespaces.Add(NSPREFIX_DOC, NS_DOC);

            // add namespace to be able to utilize BDTs
            schema.Namespaces.Add(NSPREFIX_CDT, context.TargetNamespace);

            // R 8FE2: include BDT XML schema file
            // TODO: check with christian e. if we can retrieve the bdt schema file from the context
            XmlSchemaInclude cdtInclude = new XmlSchemaInclude();
            cdtInclude.SchemaLocation = "CoreDataType_" + schema.Version + ".xsd";
            schema.Includes.Add(cdtInclude);

            foreach (IACC acc in accs)
            {
                // finally add the complex type to the schema
                schema.Items.Add(GenerateComplexTypeACC(context, schema, acc));

                // R 9DA0: for each ABIE a named element must be globally declared
                // R 9A25: the name of the ABIE element must be the DictionaryEntryName with whitespace 
                //         and the 'Details' suffix removed
                // R B27B: every ABIE global element declaration must be of the complexType that represents
                //         the ABIE
                XmlSchemaElement elementACC = new XmlSchemaElement();
                elementACC.Name = acc.Name;
                elementACC.SchemaTypeName = new XmlQualifiedName(context.NamespacePrefix + ":" + acc.Name + "Type");
                schema.Items.Add(elementACC);
            }

            context.AddSchema(schema, "CoreComponent_" + schema.Version + ".xsd");
        }

        internal static XmlSchemaComplexType GenerateComplexTypeACC(GeneratorContext context, XmlSchema schema, IACC acc)
        {
            return GenerateComplexTypeABIE(context, schema, acc, context.NamespacePrefix);
        }

        internal static XmlSchemaComplexType GenerateComplexTypeABIE(GeneratorContext context, XmlSchema schema, IACC acc, string accPrefix)
        {
            // R A4CE, R AF95: a complex type must be defined for each ABIE   
            XmlSchemaComplexType complexTypeACC = new XmlSchemaComplexType();

            // R 9D83: the name of the ABIE must be the DictionaryEntryName with all whitespace and separators 
            //         removed. The 'Details' suffix is replaced with 'Type'.
            complexTypeACC.Name = acc.Name + "Type";

            if (context.Annotate)
            {
                complexTypeACC.Annotation = GetACCAnnotation(acc);
            }

            // create the sequence for the BBIEs within the ABIE
            XmlSchemaSequence sequenceBCCs = new XmlSchemaSequence();

            foreach (IBCC bcc in acc.BCCs)
            {
                // R 89A6: for every BBIE a named element must be locally declared
                XmlSchemaElement elementBCC = new XmlSchemaElement();

                // R AEFE, R 96D9, R9A40, R A34A are implemented in GenerateBBIEName(...)
                elementBCC.Name = GenerateBCCName(bcc.Name, bcc.Type.Name);

                // R 8B85: every BBIE type must be named the property term and qualifiers and the
                //         representation term of the basic business information entity (BBIE) it represents
                //         with the word 'Type' appended. 
                elementBCC.SchemaTypeName =
                    new XmlQualifiedName(NSPREFIX_CDT + ":" + bcc.Type.Name + bcc.Type.CON.BasicType.Name + "Type");


                // R 90F9: cardinality of elements within the ABIE
                elementBCC.MinOccursString = AdjustLowerBound(bcc.LowerBound);
                elementBCC.MaxOccursString = AdjustUpperBound(bcc.UpperBound);

                if (context.Annotate)
                {
                    elementBCC.Annotation = GetBCCAnnotation(bcc);
                }

                // add the element created to the sequence
                sequenceBCCs.Items.Add(elementBCC);
            }


            foreach (IASCC ascc in acc.ASCCs)
            {
                XmlSchemaElement elementASCC = new XmlSchemaElement();

                // R A08A: name of the ASBIE
                elementASCC.Name = ascc.Name + ascc.AssociatedElement.Name;
                elementASCC.SchemaTypeName =
                    new XmlQualifiedName(accPrefix + ":" + ascc.AssociatedElement.Name + "Type");

                if (context.Annotate)
                {
                    elementASCC.Annotation = GetASCCAnnotiation(ascc);
                }

                if (ascc.AggregationKind == EAAggregationKind.Shared)
                {
                    if (!globalASCCs.Contains(ascc.Name + ascc.AssociatedElement.Name))
                    {
                        // R 9241: for ASBIEs with AggregationKind = shared a global element must be declared.
                        XmlSchemaElement refASCC = new XmlSchemaElement();
                        refASCC.RefName = new XmlQualifiedName(accPrefix + ":" + ascc.Name + ascc.AssociatedElement.Name);
                        sequenceBCCs.Items.Add(refASCC);
                        schema.Items.Add(elementASCC);
                        globalASCCs.Add(ascc.Name + ascc.AssociatedElement.Name);
                    }
                }
                else
                {
                    //R 9025: ASBIEs with Aggregation Kind = composite a local element for the
                    //        associated ABIE must be declared in the associating ABIE complex type.
                    sequenceBCCs.Items.Add(elementASCC);
                }
            }

            // add the sequence created to the complex type
            complexTypeACC.Particle = sequenceBCCs;
            return complexTypeACC;
        }

        ///<summary>
        ///</summary>
        ///<param name="bbie"></param>
        ///<returns></returns>
        public static XmlSchemaAnnotation GetBCCAnnotation(IBCC bcc)
        {
            // Contains all the documentation items such as DictionaryEntryName
            IList<XmlNode> documentation = new List<XmlNode>();

            AddDocumentation(documentation, "UniqueID", bcc.UniqueIdentifier);
            AddDocumentation(documentation, "VersionID", bcc.VersionIdentifier);
            AddDocumentation(documentation, "Cardinality", bcc.LowerBound + ".." + bcc.UpperBound);
            AddDocumentation(documentation, "SequencingKey", bcc.SequencingKey);
            AddDocumentation(documentation, "DictionaryEntryName", bcc.DictionaryEntryName);
            AddDocumentation(documentation, "Definition", bcc.Definition);
            AddDocumentation(documentation, "BusinessTermName", bcc.BusinessTerms);
            AddDocumentation(documentation, "PropertyTermName", bcc.Name);
            AddDocumentation(documentation, "RepresentationTermName", bcc.Type.Name);
            AddDocumentation(documentation, "AcronymCode", "BCC");

            XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
            annotation.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = documentation.ToArray() });

            return annotation;
        }

        ///<summary>
        ///</summary>
        ///<param name="abie"></param>
        public static XmlSchemaAnnotation GetACCAnnotation(IACC acc)
        {
            // Contains all the documentation items such as DictionaryEntryName
            IList<XmlNode> documentation = new List<XmlNode>();

            AddDocumentation(documentation, "UniqueID", acc.UniqueIdentifier);
            AddDocumentation(documentation, "VersionID", acc.VersionIdentifier);
            AddDocumentation(documentation, "ObjectClassQualifierName", getObjectClassQualifier(acc.Name));
            AddDocumentation(documentation, "ObjectClassTermName", getObjectClassTerm(acc.Name));
            AddDocumentation(documentation, "DictionaryEntryName", acc.DictionaryEntryName);
            AddDocumentation(documentation, "Definition", acc.Definition);
            AddDocumentation(documentation, "BusinessTermName", acc.BusinessTerms);
            AddDocumentation(documentation, "AcronymCode", "ACC");

            XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
            annotation.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = documentation.ToArray() });

            return annotation;
        }

        ///<summary>
        ///</summary>
        ///<param name="asbie"></param>
        ///<returns></returns>
        public static XmlSchemaAnnotation GetASCCAnnotiation(IASCC ascc)
        {
            // Contains all the documentation items such as DictionaryEntryName
            IList<XmlNode> documentation = new List<XmlNode>();

            AddDocumentation(documentation, "UniqueID", ascc.UniqueIdentifier);
            AddDocumentation(documentation, "VersionID", ascc.VersionIdentifier);
            AddDocumentation(documentation, "Cardinality", ascc.LowerBound + ".." + ascc.UpperBound);
            AddDocumentation(documentation, "SequencingKey", ascc.SequencingKey);
            AddDocumentation(documentation, "DictionaryEntryName", ascc.DictionaryEntryName);
            AddDocumentation(documentation, "Definition", ascc.Definition);
            AddDocumentation(documentation, "BusinessTermName", ascc.BusinessTerms);
            AddDocumentation(documentation, "AssociationType", ascc.AggregationKind.ToString());
            AddDocumentation(documentation, "PropertyTermName", ascc.Name);
            // PropertyQualifierName could be extracted from the PropertyTermName (e.g. "My" in 
            // "My_Address") but is not implement at this point 
            AddDocumentation(documentation, "PropertyQualifierName", "");
            AddDocumentation(documentation, "AssociatedObjectClassTermName", ascc.AssociatedElement.Name);
            // AssociatedObjectClassQualifierTermName could be extracted from the AssociatedObjectClassTermName
            // (e.g. "My" in "My_Address") but is not implement at this point 
            AddDocumentation(documentation, "AcronymCode", "ASBIE");


            XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
            annotation.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = documentation.ToArray() });

            return annotation;
        }

        public static void AddDocumentation(IList<XmlNode> doc, string name, string value)
        {
            // Dummy XML Document needed to create various XML schema elements (e.g. text nodes)
            XmlDocument dummyDoc = new XmlDocument();

            XmlElement documentationElement = dummyDoc.CreateElement(NSPREFIX_DOC, name, NS_DOC);
            documentationElement.InnerText = value;
            doc.Add(documentationElement);
        }

        public static void AddDocumentation(IList<XmlNode> doc, string name, IEnumerable<string> values)
        {
            int countBusinessTerms = 0;

            foreach (string value in values)
            {
                AddDocumentation(doc, name, value);
                countBusinessTerms++;
            }

            if (countBusinessTerms < 1)
            {
                AddDocumentation(doc, name, "");
            }
        }

        private static string AdjustLowerBound(string lb)
        {
            return AdjustBound(lb);
        }

        private static string AdjustUpperBound(string ub)
        {
            return AdjustBound(ub);
        }

        private static string AdjustBound(string bound)
        {
            if (bound.Equals(""))
            {
                return "1";
            }

            if (bound.Equals("*"))
            {
                return "unbounded";
            }

            return bound;
        }

        private static string GenerateBCCName(string bccName, string bccType)
        {
            if ((bccName.EndsWith("Identification")) && (bccType.Equals("Identifier")))
            {
                return bccName.Remove(bccName.Length - 14) + "Identifer";
            }

            if ((bccName.EndsWith("Indication")) && (bccType.Equals("Indicator")))
            {
                return bccName.Remove(bccName.Length - 10) + "Indicator";
            }

            if (bccType.Equals("Text"))
            {
                return bccName;
            }

            return bccName + bccType;
        }

        public static string getObjectClassTerm(string name)
        {
            if (name.LastIndexOf('_') != -1)
            {
                return name.Substring(name.LastIndexOf('_') + 1);
            }
            return name;
        }

        public static string getObjectClassQualifier(string name)
        {
            if (name.LastIndexOf('_') != -1)
            {
                return name.Substring(0, name.LastIndexOf('_'));
            }
            return name;
        }
    }
}
