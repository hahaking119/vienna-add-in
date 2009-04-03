using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.CCTS;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class RootSchemaGenerator
    {
        public static void GenerateXSD(GenerationContext context, IDOCLibrary docLibrary)
        {
            //private static string NSPREFIX_BDT = "bdt";
            //private static string NSPREFIX_TNS = "tns";
            //private static string SCHEMA_LOCATION_BDT = "bdts.xsd";
            //private static string SCHEMA_NAME_BIE = "bies.xsd";
            
            foreach (IABIE abie in docLibrary.RootElements)
            {
                var schema = new XmlSchema { TargetNamespace = context.TargetNamespace };
                schema.Namespaces.Add(context.NamespacePrefix, context.TargetNamespace);
                schema.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
                schema.Namespaces.Add("ccts",
                                      "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:2");
                

                schema.ElementFormDefault = XmlSchemaForm.Qualified;
                schema.AttributeFormDefault = XmlSchemaForm.Unqualified;
                schema.Version = docLibrary.VersionIdentifier;

                //TODO how do i now what schemas to include and what to import
                AddImports(schema, context, docLibrary);
                AddIncludes(schema, context, docLibrary);

                AddRootElemntDeclaration(schema, abie, context);
                AddRootTypeDefinition(schema, abie, context);
               
                AddGlobalElementDeclarations(schema, (new List<IABIE>(docLibrary.BIEs)), context);
                context.AddSchema(schema, "root.xsd");
            }
        }

        private static void AddGlobalElementDeclarations(XmlSchema schema, IEnumerable<IABIE> BIEs, GenerationContext context)
        {
            
            //TODO These are ABIEs which have not been defined on the BIE layer
        }

        private static void AddTypeDefinitions()
        {
            //TODO These are type defintions for new ABIEs not defined on the BIE Layer
        }

        private static void AddElementAnnotations()
        {
            //TODO Generate Element Annotations
        }

        private static void AddTypeAnnotations()
        {
            //TODO Generate Type Annotations
        }

        private static void AddRootElemntDeclaration(XmlSchema schema, IABIE abie, GenerationContext context)
        {
            XmlSchemaElement root = new XmlSchemaElement();
            root.Name = abie.Name;
            root.SchemaTypeName =
                        new XmlQualifiedName(context.NamespacePrefix + ":" + abie.Name + "Type");
            schema.Items.Add(root);
        }

        private static void AddRootTypeDefinition(XmlSchema schema, IABIE abie, GenerationContext context)
        {
            XmlSchemaComplexType roottype = new XmlSchemaComplexType();
            roottype.Name = context.NamespacePrefix + ":" + abie.Name + "Type";

            XmlSchemaSequence sequence = new XmlSchemaSequence();

            //TODO this is now only partially correct implemented, an algorithmn has to take links and instances of ASBIEs into account
            foreach (IASBIE asbie in abie.ASBIEs)
            {
                XmlSchemaElement element = new XmlSchemaElement();
                element.RefName = new XmlQualifiedName(asbie.AssociatedElement.Name);
                
                //TODO mapping of EA cardinalities to XML Schema
                element.MinOccursString = asbie.LowerBound;
                element.MaxOccursString = asbie.UpperBound;
                sequence.Items.Add(element);
            }
            
            schema.Items.Add(roottype);


        }

        private static void AddImports(XmlSchema schema, GenerationContext context, IDOCLibrary docLibrary)
        {
            //foreach (SchemaInfo si in context.Schemas)
           // {
                //XmlTextReader textReader = new XmlTextReader(si.FileName);
                XmlTextReader textReader = new XmlTextReader("C:\\XMLNDR_Documentation_3p0.xsd");
                XmlSchema importSchema = XmlSchema.Read(textReader, null);

                XmlSchemaImport import = new XmlSchemaImport();
                import.Schema = importSchema;
                import.Namespace = importSchema.TargetNamespace;
                import.SchemaLocation = "C:\\XMLNDR_Documentation_3p0.xsd";
                
                schema.Includes.Add(import);
           // }

        }
        private static void AddIncludes(XmlSchema schema, GenerationContext context, IDOCLibrary docLibrary)
        {
            foreach (SchemaInfo si in context.Schemas)
            {
                XmlTextReader textReader = new XmlTextReader(si.FileName);
                XmlSchema includeSchema = XmlSchema.Read(textReader, null);

                XmlSchemaInclude include = new XmlSchemaInclude();
                include.Schema = includeSchema;
                schema.Includes.Add(include);
            }
        }

    }
}