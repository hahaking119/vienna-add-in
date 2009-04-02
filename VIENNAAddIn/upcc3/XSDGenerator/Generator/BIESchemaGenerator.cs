using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    ///<summary>
    ///</summary>
    public class BIESchemaGenerator
    {
        // todo: finalize the ABIE schema generator by extending the implementation as specified in the NDRs
        private static string NSPREFIX_BDT = "bdt";
        private static string NSPREFIX_TNS = "tns";
        private static string SCHEMA_LOCATION_BDT = "bdts.xsd";
        private static string SCHEMA_NAME_BIE = "bies.xsd";

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="bies"></param>
        public static void GenerateXSD(GenerationContext context, IEnumerable<IBIE> bies)
        {
            // prepare xml schema file and add header
            var schema = new XmlSchema {TargetNamespace = context.TargetNamespace};
            schema.Namespaces.Add(context.NamespacePrefix, context.TargetNamespace);
            schema.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            schema.Namespaces.Add("ccts",
                                  "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:2");

            // add namespace to be able to utilize BDTs
            schema.Namespaces.Add(NSPREFIX_BDT, context.TargetNamespace);
            // add namespace to be able to utilize ABIEs 
            schema.Namespaces.Add(NSPREFIX_TNS, context.TargetNamespace);


            // include the xml schema containing all the BDTs
            XmlSchemaInclude bdtInclude = new XmlSchemaInclude();
            // todo: check with christian e. if we can retrieve the bdt schema file from the context ...
            //bdtInclude.SchemaLocation = "../../../VIENNAAddInUnitTests/testresources/XSDGeneratorTest/bdts.xsd";
            bdtInclude.SchemaLocation = SCHEMA_LOCATION_BDT;
            schema.Includes.Add(bdtInclude);

            foreach (IABIE abie in bies)
            {
                // create the complex type for the current ABIE
                XmlSchemaComplexType complexTypeBIE = new XmlSchemaComplexType();
                complexTypeBIE.Name = abie.Name + "Type";

                // todo: implement annotation
                //complexTypeBIE.Annotation

                // create the sequence for the BBIEs within the ABIE
                XmlSchemaSequence sequenceBBIEs = new XmlSchemaSequence();                

                foreach (IBBIE bbie in abie.BBIEs)
                {
                    // create the element representing a particular BBIE
                    XmlSchemaElement elementBBIE = new XmlSchemaElement();
                    elementBBIE.Name = bbie.Name;
                    elementBBIE.SchemaTypeName = new XmlQualifiedName(NSPREFIX_BDT + ":" + bbie.Type.Name + "Type");


                    // add the element created to the sequence
                    sequenceBBIEs.Items.Add(elementBBIE);
                }

                // process the ASBIEs
                foreach (IASBIE asbie in abie.ASBIEs)
                {
                    XmlSchemaElement elementASBIE = new XmlSchemaElement();
                    //elementASBIE.Name = asbie.AssociatedElement.Name;
                    elementASBIE.Name = asbie.Name;
                    elementASBIE.SchemaTypeName =
                        new XmlQualifiedName(NSPREFIX_TNS + ":" + asbie.AssociatedElement.Name + "Type");

                    sequenceBBIEs.Items.Add(elementASBIE);
                }

                // add the sequence created to the complex type
                complexTypeBIE.Particle = sequenceBBIEs;

                // finally add the complex type to the schema
                schema.Items.Add(complexTypeBIE);

                // also, for each ABIE representing a comlex type an element should be 
                // defined as well. 
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
    }
}