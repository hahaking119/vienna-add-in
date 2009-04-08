using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.CCTS;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    ///<summary>
    ///</summary>
    public class RootSchemaGenerator
    {
        private static string NSPREFIX_BDT = "bdt";
        private static string NSPREFIX_BIE = "bie";

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="docLibrary"></param>
        public static void GenerateXSD(GenerationContext context, IDOCLibrary docLibrary)
        {
            //private static string SCHEMA_LOCATION_BDT = "bdts.xsd";
            //private static string SCHEMA_NAME_BIE = "bies.xsd";

            foreach (IABIE abie in docLibrary.RootElements)
            {
                var schema = new XmlSchema { TargetNamespace = context.TargetNamespace };
                schema.Namespaces.Add(context.NamespacePrefix, context.TargetNamespace);
                schema.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
                schema.Namespaces.Add("ccts",
                                      "urn:un:unece:uncefact:documentation:standard:XMLNDRDocumentation:3");
                schema.Namespaces.Add(NSPREFIX_BDT, context.TargetNamespace);
                schema.Namespaces.Add(NSPREFIX_BIE, context.TargetNamespace);

                schema.ElementFormDefault = XmlSchemaForm.Qualified;
                schema.AttributeFormDefault = XmlSchemaForm.Unqualified;
                schema.Version = docLibrary.VersionIdentifier.DefaultTo("1");

                //TODO how do i now what schemas to include and what to import
                AddImports(schema, context, docLibrary);
                AddIncludes(schema, context, docLibrary);

                AddRootElemntDeclaration(schema, abie, context);
                AddRootTypeDefinition(schema, abie, context, docLibrary);

                AddGlobalElementDeclarations(schema, removeRootElements(docLibrary.BIEs, docLibrary.RootElements),
                                             context);
                AddGlobalTypeDefinitions(schema, removeRootElements(docLibrary.BIEs, docLibrary.RootElements),
                                             context);
                context.AddSchema(schema, "root.xsd");
            }
        }

        private static void AddGlobalElementDeclarations(XmlSchema schema, IEnumerable<IABIE> abies,
                                                         GenerationContext context)
        {
            foreach (IABIE abie in abies)
            {
                XmlSchemaElement element = new XmlSchemaElement
                                               {
                                                   Name = abie.Name,
                                                   SchemaTypeName =
                                                       new XmlQualifiedName(context.NamespacePrefix + ":" + abie.Name +
                                                                            "Type")
                                               };
                if (context.Annotate)
                    element.Annotation = BIESchemaGenerator.GetABIEAnnotation(abie);
                schema.Items.Add(element);
            }
        }

        private static void AddGlobalTypeDefinitions(XmlSchema schema, IEnumerable<IABIE> abies, GenerationContext context)
        {
            foreach (IABIE abie in abies)
            {
                schema.Items.Add(BIESchemaGenerator.GenerateComplexTypeABIE(context, schema, abie, context.NamespacePrefix));
            }

        }

        private static IList<IABIE> removeRootElements(IEnumerable<IABIE> abies, IEnumerable<IABIE> rootelements)
        {
            IList<IABIE> temp = new List<IABIE>(abies);
            foreach (IABIE rootelement in rootelements)
            {
                temp.Remove(rootelement);
            }
            return temp;
        }

        private static void AddRootElemntDeclaration(XmlSchema schema, IABIE abie, GenerationContext context)
        {
            XmlSchemaElement root = new XmlSchemaElement
                                        {
                                            Name = abie.Name,
                                            SchemaTypeName =
                                                new XmlQualifiedName(context.NamespacePrefix + ":" + abie.Name + "Type")
                                        };
            if (context.Annotate)
                root.Annotation = GetRootAnnotation(abie);
            schema.Items.Add(root);
        }

        private static void AddRootTypeDefinition(XmlSchema schema, IABIE abie, GenerationContext context, IDOCLibrary docLibrary)
        {
            XmlSchemaComplexType type = BIESchemaGenerator.GenerateComplexTypeABIE(context, schema, abie, NSPREFIX_BIE);
            XmlSchemaElement element;
            schema.Items.Add(type);
            IList<IABIE> temp = new List<IABIE>(docLibrary.BIEs);
            XmlSchemaSequence sequence = type.Particle as XmlSchemaSequence;

            if (sequence != null)
                foreach (XmlSchemaObject item in sequence.Items)
                {
                    if (item is XmlSchemaElement)
                    {
                        element = item as XmlSchemaElement;

                        foreach (IASBIE asbie in abie.ASBIEs)
                        {
                            // this condition makes sure only docLibraryLevel ABIEs get selected
                            if (temp.Contains(asbie.AssociatedElement))
                            {
                                if (element.RefName != null && element.RefName.ToString().Contains(NSPREFIX_BIE + ":" + asbie.Name + asbie.AssociatedElement.Name))
                                {
                                    String s = element.RefName.ToString();
                                    s = s.Replace(NSPREFIX_BIE, context.NamespacePrefix);
                                    element.RefName = new XmlQualifiedName(s);
                                    UpdateElementTypePrefix(schema, context, element.RefName.ToString());
                                    break;
                                }
// ReSharper disable RedundantIfElseBlock
                                else if (element.Name != null && element.Name.ToString().Contains(asbie.Name + asbie.AssociatedElement.Name))
// ReSharper restore RedundantIfElseBlock
                                {
                                    String s = element.SchemaTypeName.ToString();
                                    s = s.Replace(NSPREFIX_BIE, context.NamespacePrefix);
                                    element.SchemaTypeName = new XmlQualifiedName(s);
                                    UpdateElementTypePrefix(schema, context, element.SchemaTypeName.ToString());
                                    break;
                                }
                            }
                        }
                    }
                }

            //TODO this is now only partially correct implemented, an algorithmn has to take links and instances of ASBIEs into account

        }


        private static void UpdateElementTypePrefix(XmlSchema schema, GenerationContext context, String name)
        {
            String temp = "";
            XmlSchemaElement element;
            foreach (XmlSchemaObject item in schema.Items)
            {
                if (item is XmlSchemaElement){
                    element = item as XmlSchemaElement;
                   if(name.Contains(element.Name))
                   {
                       temp = element.SchemaTypeName.ToString();
                       temp = temp.Replace(NSPREFIX_BIE, context.NamespacePrefix);
                       element.SchemaTypeName = new XmlQualifiedName(temp);
                       return;
                   }
                }
            }
        }

        private static void AddASBIE(XmlSchema schema, XmlSchemaSequence sequence, IABIE abie, GenerationContext context, IDOCLibrary docLibrary)
        {
            IList<IABIE> temp = new List<IABIE>(docLibrary.BIEs);

            foreach (IASBIE asbie in abie.ASBIEs)
            {
                XmlSchemaElement elementASBIE = new XmlSchemaElement();

                elementASBIE.Name = asbie.Name;
                elementASBIE.SchemaTypeName = new XmlQualifiedName(context.NamespacePrefix + ":" + asbie.AssociatedElement.Name + "Type");

                ////TODO: check if upper bound is * .. also implement other "exceptions"
                //Console.WriteLine("lower: " + asbie.LowerBound);
                //Console.WriteLine("upper: " + asbie.UpperBound);


                //elementASBIE.MinOccurs = Convert.ToDecimal(asbie.LowerBound);
                //elementASBIE.MaxOccurs = Convert.ToDecimal(asbie.UpperBound);


                if (context.Annotate)
                {
                    //TODO Annotations for ASBIEs
                    //elementASBIE.Annotation = GetASBIEAnnotiation(asbie);
                }

                if (asbie.AggregationKind == AggregationKind.Shared)
                {
                    // R 9241: for ASBIEs with AggregationKind = shared a global element must be declared.
                    XmlSchemaElement refASBIE = new XmlSchemaElement();
                    refASBIE.RefName = temp.Contains(asbie.AssociatedElement) ? new XmlQualifiedName(context.NamespacePrefix + ":" + asbie.Name + asbie.AssociatedElement.Name) : new XmlQualifiedName(NSPREFIX_BIE + ":" + asbie.Name + asbie.AssociatedElement.Name);
                    refASBIE.MinOccursString = asbie.LowerBound;
                    refASBIE.MaxOccursString = asbie.UpperBound;
                    sequence.Items.Add(refASBIE);
                    schema.Items.Add(elementASBIE);
                }
                else if (asbie.AggregationKind == AggregationKind.Composite)
                {
                    //R 9025: ASBIEs with Aggregation Kind = composite a local element for the
                    //        associated ABIE must be declared in the associating ABIE complex type.
                    //TODO mapping of EA cardinalities to XML Schema
                    elementASBIE.MinOccursString = asbie.LowerBound;
                    elementASBIE.MaxOccursString = asbie.UpperBound;
                    sequence.Items.Add(elementASBIE);
                }
            }

        }

        private static void AddImports(XmlSchema schema, GenerationContext context, IDOCLibrary docLibrary)
        {
            //foreach (SchemaInfo si in context.Schemas)
            // {
            //XmlTextReader textReader = new XmlTextReader(si.FileName);
            XmlTextReader textReader =
                new XmlTextReader("C:\\dump\\documentation\\standard\\XMLNDR_Documentation_3p0.xsd");
            XmlSchema importSchema = XmlSchema.Read(textReader, null);

            XmlSchemaImport import = new XmlSchemaImport();
            import.Schema = importSchema;
            import.Namespace = importSchema.TargetNamespace;
            import.SchemaLocation = "documentation/standard/XMLNDR_Documentation_3p0.xsd";

            schema.Includes.Add(import);
            // }
        }

        private static void AddIncludes(XmlSchema schema, GenerationContext context, IDOCLibrary docLibrary)
        {
            foreach (SchemaInfo si in context.Schemas)
            {
                //XmlTextReader textReader = new XmlTextReader(context.OutputDirectory + si.FileName);
                //XmlSchema includeSchema = XmlSchema.Read(textReader, null);
                XmlSchemaInclude include = new XmlSchemaInclude();
                //include.Schema = includeSchema;
                include.SchemaLocation = si.FileName;
                schema.Includes.Add(include);
            }
        }

        private static XmlSchemaAnnotation GetRootAnnotation(IABIE abie)
        {
            // Contains all the documentation items such as DictionaryEntryName
            IList<XmlNode> documentation = new List<XmlNode>();

            BIESchemaGenerator.AddDocumentation(documentation, "UniqueID", abie.UniqueIdentifier);
            BIESchemaGenerator.AddDocumentation(documentation, "VersionID", abie.VersionIdentifier);
            BIESchemaGenerator.AddDocumentation(documentation, "ObjectClassQualifierName", BIESchemaGenerator.getObjectClassQualifier(abie.Name));
            BIESchemaGenerator.AddDocumentation(documentation, "ObjectClassTermName", BIESchemaGenerator.getObjectClassTerm(abie.Name));
            BIESchemaGenerator.AddDocumentation(documentation, "DictionaryEntryName", abie.DictionaryEntryName);
            BIESchemaGenerator.AddDocumentation(documentation, "Definition", abie.Definition);
            BIESchemaGenerator.AddDocumentation(documentation, "BusinessTermName", abie.BusinessTerms);

            XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
            annotation.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = documentation.ToArray() });

            return annotation;
        }

        
    }
}