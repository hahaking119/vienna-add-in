using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class CDTLibraryGenerator : AbstractLibraryGenerator<ICDTLibrary>
    {
        private const string NS_PREFIX = "cdt";

        public CDTLibraryGenerator(GenerationContext context) : base(context)
        {
        }

        public override XmlSchema GenerateXSD(ICDTLibrary library)
        {
            var schema = new XmlSchema
                             {
                                 ElementFormDefault = XmlSchemaForm.Qualified,
                                 AttributeFormDefault = XmlSchemaForm.Unqualified,
                                 Version = library.VersionIdentifier.DefaultTo("notSpecified")
                             };
            AddNameSpaces(schema, library);
            library.CDTs.ForEach(cdt => schema.Items.Add(GetSchemaElement(cdt)));
            return schema;
        }

        private XmlSchemaObject GetSchemaElement(ICDT cdt)
        {
            var complexType = new XmlSchemaComplexType {Name = (cdt.Name.ToXmlName() + "Type")};

            if (Context.Annotate)
            {
                complexType.Annotation = GetCDTAnnotation(cdt);
            }

            var simpleContent = new XmlSchemaSimpleContent();
            var simpleContent_extension = new XmlSchemaSimpleContentExtension
                                              {
                                                  BaseTypeName =
                                                      new XmlQualifiedName(cdt.CON.Type,
                                                                           "http://www.w3.org/2001/XMLSchema")
                                              };

            foreach (IDTComponent sup in cdt.SUPs)
            {
                var att = new XmlSchemaAttribute();
                if (Context.Annotate)
                    att.Annotation = getAttributeAnnotation(sup);
                att.Name = sup.Name;
                att.SchemaTypeName = getXSDType(sup);
                att.Use = sup.IsOptional() ? XmlSchemaUse.Optional : XmlSchemaUse.Required;
                simpleContent_extension.Attributes.Add(att);
            }

            simpleContent.Content = simpleContent_extension;
            complexType.ContentModel = simpleContent;

            return complexType;
        }

        private XmlQualifiedName getXSDType(IDTComponent dtComponent)
        {
            string type = dtComponent.Type.ToXSDType();
            if (type == null)
            {
                Context.appendWarnMessage(
                    "No built-in datatype found for the attribute " + dtComponent.Name + " in element " +
                    dtComponent.DT.Name +
                    ". Using xsd:string instead.", dtComponent.DT.Library.Name);
                type = "string";
            }
            return new XmlQualifiedName(type, "http://www.w3.org/2001/XMLSchema");
        }

        private static XmlSchemaAnnotation GetCDTAnnotation(ICDT cdt)
        {
            var annotationBuilder = new AnnotationBuilder();
            annotationBuilder.addOptionalAnnotation("UniqueID", cdt.UniqueIdentifier);
            // TODO what's the acronym?
            annotationBuilder.addOptionalAnnotation("Acronym", "CCT");
            annotationBuilder.addOptionalAnnotation("DictionaryEntryName",
                                                    cdt.DictionaryEntryName);
            annotationBuilder.addOptionalAnnotation("Version", cdt.VersionIdentifier);
            annotationBuilder.addOptionalAnnotation("Definition", cdt.Definition);
            var usageRules = new List<string>(cdt.UsageRules);
            if (usageRules.Count > 0)
            {
                // TODO how to annotate multiple usage rules
                annotationBuilder.addOptionalAnnotation("UsageRule", usageRules[0]);
            }
            var businessTerms = new List<string>(cdt.BusinessTerms);
            if (businessTerms.Count > 0)
            {
                // TODO how to annotate multiple business terms
                annotationBuilder.addOptionalAnnotation("BusinessTerm",
                                                        businessTerms[0]);
            }
            return annotationBuilder.Annotation;
        }

        /// <sUMM2ary>
        /// Returns the annoation for an attribute
        /// </sUMM2ary>
        /// <param name="sup"></param>
        /// <returns></returns>
        private static XmlSchemaAnnotation getAttributeAnnotation(IDTComponent sup)
        {
            var annotationBuilder = new AnnotationBuilder();
            annotationBuilder.addOptionalAnnotation("Acronym", "SC");
            annotationBuilder.addOptionalAnnotation("Name", sup.Name);
            annotationBuilder.addOptionalAnnotation("Definition", sup.Definition);
            annotationBuilder.addOptionalAnnotation("Cardinality", sup.LowerBound + ".." + sup.UpperBound);
            // TODO get complete list of annotations
            return annotationBuilder.Annotation;
        }

        private void AddNameSpaces(XmlSchema schema, IBusinessLibrary cdtLibrary)
        {
            schema.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            schema.Namespaces.Add("ccts",
                                  "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:3");

            string schemaNamespace = cdtLibrary.BaseURN;
            if (string.IsNullOrEmpty(schemaNamespace))
            {
                throw new Exception(
                    "Please fill the 'baseURN' tagged value of the package. 'baseURN' tagged value can not be empty.");
            }

            schema.Namespaces.Add(Context.GetNextNamespacePrefix(NS_PREFIX), schemaNamespace);
            schema.TargetNamespace = schemaNamespace;
        }
    }
}