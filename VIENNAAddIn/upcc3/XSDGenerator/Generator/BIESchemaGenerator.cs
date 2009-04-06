using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    // Information in the CCTS model for BIEs also relevant for XML schema generation includes:
    // (refer to section 5.2.1 CCTS of the NDRs)
    //   a) Common Information: expressed in the annotation documentation in the XML schema
    //   b) Usage Rules: expressed in the annotation application information in the XML schema 
    //
    // Naming Rules (refer to section 5.4 Naming and Modeling Constraints, R 9956, R A781):
    //   a) Lower Camel Case: XML Schema attributes
    //   b) Upper Camel Case: XML Schema elements and types
    //
    // TODO: discuss correct file naming with Christian E. and Michi S. and incorporate the conclusion into the code
    // File Conventions (refer to section 5.7 XML Schema Files, R 92B8, R 8D58):
    //   a) File Name: <SchemaModuleName>_<Version>.xsd with spaces, periods or other separators removed
    //                 BIE XML Schema Files must be named 'Business Information Entity XML Schema File' (R 8252)
    //   b) Versioning: scheme versioning consists of stuats of the schema file, major version numer, minor version number and 
    //                  revision number (refer to section 5.7 Versioning Scheme, R BF17, R 84BE, R 9049, R A735, R AFA8, R BBD5, R 998B)
    //                  period in versions must be replaces by a lowercase p (e.g. 3p0)
    //   c) Location: schemaLocation must contain a resolvable path URL (refer to section 5.8 Schema Location, R 8F8D)
    // 
    // TODO: discuss CCTS Artifact Metadata with Christian E. and Michi S. (refer to section 7.1.4 CCTS Artifact Metadata)
    //
    // Documentation of generated Constructs
    //   a) all generated XML schema constructs will use the xsd:documentation and xsd:appInfo within xsd:annotation
    //      (refer to section 7.5 Annotation, R847A)


    ///<summary>
    ///</summary>
    public class BIESchemaGenerator
    {
        private static string NSPREFIX_BDT = "bdt";
        private static string NSPREFIX_TNS = "tns";
        private static string NSPREFIX_CCTS = "ccts";

        //private static string NS_CCTS_2 =
        //    "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:2";
        
        private static string NS_CCTS_3 =
            "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:3";


        private static string NSPREFIX_XSD = "xsd";
        private static string NS_XSD = "http://www.w3.org/2001/XMLSchema";
        private static string SCHEMA_LOCATION_BDT = "bdts.xsd";
        private static string SCHEMA_NAME_BIE = "bies.xsd";

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="bies"></param>
        public static void GenerateXSD(GenerationContext context, IEnumerable<IBIE> bies)
        {
            // TODO: check if there are duplicate namespaces being generated
            // Create XML schema file and prepare the XML schema header
            // R 88E2: all XML schema files must use UTF-8 encoding (refer to section 7.1.1 XML Schema Declaration)

            // R B387: every XML schema must have a declared target namespace
            var schema = new XmlSchema {TargetNamespace = context.TargetNamespace};
            schema.Namespaces.Add(context.NamespacePrefix, context.TargetNamespace);

            // TODO: discuss R A0E5 and R A9C5 with Christian E. and Michi S. since this is something that should be added to the context
            // R A0E5: all XML schemas must contain elementFormDefault and set it to qualified
            // (refer to section 7.1.3 Schema Declaration)            
            schema.ElementFormDefault = XmlSchemaForm.Qualified;

            // R A9C5: All XML schemas must contain attributeFormDefault and set it to unqualified
            // (refer to section 7.1.3 Schema Declaration)            
            schema.AttributeFormDefault = XmlSchemaForm.Unqualified;

            // R 9B18: all XML schemas must utilize the xsd prefix when referring to the W3C XML schema namespace
            // (refer to section 7.1.3 Schema Declaration)
            schema.Namespaces.Add(NSPREFIX_XSD, NS_XSD);

            schema.Namespaces.Add(NSPREFIX_CCTS, NS_CCTS_3);

            // add namespace to be able to utilize BDTs
            schema.Namespaces.Add(NSPREFIX_BDT, context.TargetNamespace);
            // add namespace to be able to utilize ABIEs 
            schema.Namespaces.Add(NSPREFIX_TNS, context.TargetNamespace);


            // Include the xml schema containing all the BDTs (refer to section 5.7 XML Schema Files)
            XmlSchemaInclude bdtInclude = new XmlSchemaInclude();
            // TODO: check with christian e. if we can retrieve the bdt schema file from the context
            bdtInclude.SchemaLocation = SCHEMA_LOCATION_BDT;
            schema.Includes.Add(bdtInclude);

            foreach (IABIE abie in bies)
            {
                // TODO: what about simple types? e.g. if the abie doesn't contain any bbies?? (see image on page 19 of the NDRs)

                // R A4CE: a complex type must be defined for each ABIE    
                // R AF95: every ABIE is defined as a complex type
                XmlSchemaComplexType complexTypeBIE = new XmlSchemaComplexType();

                // R 9D83: the name of the ABIE must be the DictionaryEntryName with all whitespace and separators 
                //         removed. The 'Details' suffix is replaced with 'Type'.
                complexTypeBIE.Name = abie.Name + "Type";

                if (context.Annotate)
                {
                    complexTypeBIE.Annotation = GetABIEAnnotation(abie);
                }

                // create the sequence for the BBIEs within the ABIE
                XmlSchemaSequence sequenceBBIEs = new XmlSchemaSequence();

                foreach (IBBIE bbie in abie.BBIEs)
                {
                    // R 89A6: for every BBIE a named element must be locally declared
                    XmlSchemaElement elementBBIE = new XmlSchemaElement();

                    // R 8B85: every BBIE type must be named the property term and qualifiers and the
                    //         representation term of the basic business information entity (BBIE) it represents
                    //         with the word 'Type' appended. 
                    // TODO: generate correct name
                    elementBBIE.Name = bbie.Name;
                    elementBBIE.SchemaTypeName = new XmlQualifiedName(NSPREFIX_BDT + ":" + bbie.Type.Name + "Type");
                    // TODO: check if these cardinalities make sensse
                    elementBBIE.MinOccurs = Convert.ToDecimal(bbie.LowerBound);
                    // TODO: upperbound maybe * --> which should be translated into unbounded
                    elementBBIE.MaxOccurs = Convert.ToDecimal(bbie.UpperBound);
                    
                    if (context.Annotate)
                    {
                        elementBBIE.Annotation = GetBBIEAnnotation(bbie);
                    }

                    // add the element created to the sequence
                    sequenceBBIEs.Items.Add(elementBBIE);
                }

                
                foreach (IASBIE asbie in abie.ASBIEs)
                {
                    XmlSchemaElement elementASBIE = new XmlSchemaElement();

                    elementASBIE.Name = asbie.Name;
                    elementASBIE.SchemaTypeName =
                        new XmlQualifiedName(NSPREFIX_TNS + ":" + asbie.AssociatedElement.Name + "Type");

                    ////TODO: check if upper bound is * .. also implement other "exceptions"
                    //Console.WriteLine("lower: " + asbie.LowerBound);
                    //Console.WriteLine("upper: " + asbie.UpperBound);
                    

                    //elementASBIE.MinOccurs = Convert.ToDecimal(asbie.LowerBound);
                    //elementASBIE.MaxOccurs = Convert.ToDecimal(asbie.UpperBound);

                    if (context.Annotate)
                    {
                        elementASBIE.Annotation = GetASBIEAnnotiation(asbie);
                    }

                    if (asbie.AggregationKind == AggregationKind.Shared)
                    {                         
                        // R 9241: for ASBIEs with AggregationKind = shared a global element must be declared.
                        XmlSchemaElement refASBIE = new XmlSchemaElement();
                        refASBIE.RefName = new XmlQualifiedName(NSPREFIX_TNS + ":" + asbie.Name);
                        sequenceBBIEs.Items.Add(refASBIE);

                        schema.Items.Add(elementASBIE);                        
                    }
                    else //if(asbie.AggregationKind == AggregationKind.Composite)
                    {                         
                        //R 9025: ASBIEs with Aggregation Kind = composite a local element for the
                        //        associated ABIE must be declared in the associating ABIE complex type.
                        sequenceBBIEs.Items.Add(elementASBIE);                        
                    }
                }

                // add the sequence created to the complex type
                complexTypeBIE.Particle = sequenceBBIEs;

                // finally add the complex type to the schema
                schema.Items.Add(complexTypeBIE);

                // R 9DA0: for each ABIE a named element must be globally declared
                // R 9A25: the name of the ABIE element must be the DictionaryEntryName with whitespace 
                //         and the 'Details' suffix removed
                // R B27B: every ABIE global element declaration must be of the complexType that represents
                //         the ABIE
                XmlSchemaElement elementBIE = new XmlSchemaElement();
                elementBIE.Name = abie.Name;
                elementBIE.SchemaTypeName = new XmlQualifiedName(NSPREFIX_TNS + ":" + abie.Name + "Type");
                schema.Items.Add(elementBIE);
            }

            // TODO generate correct schema file name
            // Add the xml schema and a file name that the schema will be saved as
            // to the context which will then later on be used to serialize the 
            // xml schema to a file.
            context.AddSchema(schema, SCHEMA_NAME_BIE);
        }

        ///<summary>
        ///</summary>
        ///<param name="bbie"></param>
        ///<returns></returns>
        public static XmlSchemaAnnotation GetBBIEAnnotation(IBBIE bbie)
        {
            // Contains all the documentation items such as DictionaryEntryName
            IList<XmlNode> documentation = new List<XmlNode>();

            AddDocumentation(documentation, "UniqueID", bbie.UniqueIdentifier);
            AddDocumentation(documentation, "VersionID", bbie.VersionIdentifier);
            AddDocumentation(documentation, "Cardinality", bbie.LowerBound + ".." + bbie.UpperBound);
            AddDocumentation(documentation, "SequencingKey", bbie.SequencingKey);
            AddDocumentation(documentation, "DictionaryEntryName", bbie.DictionaryEntryName);
            AddDocumentation(documentation, "Definition", bbie.Definition);
            AddDocumentation(documentation, "BusinessTermName", bbie.BusinessTerms);
            AddDocumentation(documentation, "PropertyTermName", bbie.Name);
            AddDocumentation(documentation, "RepresentationTermName", bbie.Type.Name);

            XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
            annotation.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = documentation.ToArray() });

            return annotation;
        }

        ///<summary>
        ///</summary>
        ///<param name="abie"></param>
        public static XmlSchemaAnnotation GetABIEAnnotation(IABIE abie)
        {
            // Contains all the documentation items such as DictionaryEntryName
            IList<XmlNode> documentation = new List<XmlNode>();

            AddDocumentation(documentation, "UniqueID", abie.UniqueIdentifier);
            AddDocumentation(documentation, "VersionID", abie.VersionIdentifier);
            // ObjectClassQualifierName could be extracted from the ObjectClassTermName (e.g. "My" in "My_Address") but is 
            // not implement at this point            
            AddDocumentation(documentation, "ObjectClassTermName", abie.Name);
            AddDocumentation(documentation, "DictionaryEntryName", abie.DictionaryEntryName);
            AddDocumentation(documentation, "Definition", abie.Definition);
            AddDocumentation(documentation, "BusinessTermName", abie.BusinessTerms);

            XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
            annotation.Items.Add(new XmlSchemaDocumentation {Language = "en", Markup = documentation.ToArray()});
            
            return annotation;
        }

        ///<summary>
        ///</summary>
        ///<param name="asbie"></param>
        ///<returns></returns>
        public static XmlSchemaAnnotation GetASBIEAnnotiation(IASBIE asbie)
        {
            // Contains all the documentation items such as DictionaryEntryName
            IList<XmlNode> documentation = new List<XmlNode>();

            AddDocumentation(documentation, "UniqueID", asbie.UniqueIdentifier);
            AddDocumentation(documentation, "VersionID", asbie.VersionIdentifier);
            AddDocumentation(documentation, "Cardinality", asbie.LowerBound + ".." + asbie.UpperBound);
            AddDocumentation(documentation, "SequencingKey", asbie.SequencingKey);
            AddDocumentation(documentation, "DictionaryEntryName", asbie.DictionaryEntryName);
            AddDocumentation(documentation, "Definition", asbie.Definition);
            AddDocumentation(documentation, "BusinessTermName", asbie.BusinessTerms);
            AddDocumentation(documentation, "AssociationType", asbie.AggregationKind.ToString());
            AddDocumentation(documentation, "PropertyTermName", asbie.Name);
            // PropertyQualifierName could be extracted from the PropertyTermName (e.g. "My" in 
            // "My_Address") but is not implement at this point 
            AddDocumentation(documentation, "PropertyQualifierName", "");            
            AddDocumentation(documentation, "AssociatedObjectClassTermName", asbie.AssociatedElement.Name);
            // AssociatedObjectClassQualifierTermName could be extracted from the AssociatedObjectClassTermName
            // (e.g. "My" in "My_Address") but is not implement at this point 


            XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
            annotation.Items.Add(new XmlSchemaDocumentation {Language = "en", Markup = documentation.ToArray()});

            return annotation;
        }

        private static void AddDocumentation(IList<XmlNode> doc, string name, string value)
        {
            // Dummy XML Document needed to create various XML schema elements (e.g. text nodes)
            XmlDocument dummyDoc = new XmlDocument();

            XmlElement documentationElement = dummyDoc.CreateElement(NSPREFIX_CCTS, name, NS_CCTS_3);
            documentationElement.InnerText = value;
            doc.Add(documentationElement);
        }

        private static void AddDocumentation(IList<XmlNode> doc, string name, IEnumerable<string> values)
        {
            // Dummy XML Document needed to create various XML schema elements (e.g. text nodes)
            XmlDocument dummyDoc = new XmlDocument();

            foreach (string value in values)
            {
                AddDocumentation(doc, name, value);
            }
        }
    }
}