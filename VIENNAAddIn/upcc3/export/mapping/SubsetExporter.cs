using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.export.mapping
{
    public class SubsetExporter
    {
        private Dictionary<string, List<string>> modelDiff;
        private readonly XmlSchemaSet xmlSchemaSet;

        public SubsetExporter(IDocLibrary docLibraryComplete, IDocLibrary docLibrarySubset, string schemaFileComplete, string schemaFileSubset)
        {
            modelDiff = new UpccModelDiff(docLibraryComplete, docLibrarySubset).CalculateDiff();
            
            xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(schemaFileComplete), null));
            xmlSchemaSet.Compile();

            foreach (XmlSchema xmlSchema in xmlSchemaSet.Schemas())
            {
                xmlSchema.Write(Console.Out);
                Console.Out.WriteLine("-----------------------------------------");

                XmlSchemaObjectTable xmlSchemaObjectTable = xmlSchema.SchemaTypes;

                foreach (XmlSchemaObject type in xmlSchemaObjectTable.Values)
                {
                    if (type is XmlSchemaComplexType)
                    {
                        string complexTypeName = ((XmlSchemaComplexType)type).QualifiedName.Name;

                        if (modelDiff.ContainsKey(complexTypeName))
                        {
                            RemoveElementsAndAttributesFromComplexType((XmlSchemaComplexType) type, modelDiff[complexTypeName]);
                        }                        
                    }
                }

                // TODO: remove unused element declarations

                xmlSchema.Write(Console.Out);                
            }
        }

        private static void RemoveElementsAndAttributesFromComplexType(XmlSchemaComplexType complexType, List<string> itemsToRemove)
        {
            if (complexType.Particle is XmlSchemaGroupBase)
            {
                RemoveElementsAndAttributesFromXsdGroup((XmlSchemaGroupBase) complexType.Particle, itemsToRemove);
            }

            //if (complexType.ContentModel is XmlSchemaSimpleContent)
            //{
            //    XmlSchemaSimpleContent contentModel = (XmlSchemaSimpleContent) complexType.ContentModel;

            //    if (contentModel.Content is XmlSchemaSimpleContentExtension)
            //    {
            //        foreach (XmlSchemaObject attribute in ((XmlSchemaSimpleContentExtension) contentModel.Content).Attributes)
            //        {
            //            myChildren.Add(attribute);
            //        }
            //    }
            //    else if (contentModel.Content is XmlSchemaSimpleContentRestriction)
            //    {
            //        foreach (
            //            XmlSchemaObject attribute in
            //                ((XmlSchemaSimpleContentRestriction) contentModel.Content).Attributes)
            //        {
            //            myChildren.Add(attribute);
            //        }
            //    }
            //}

            //foreach (XmlSchemaAttribute attribute in complexType.Attributes)
            //{
            //    myChildren.Add(attribute);
            //}
        }

        private static void RemoveElementsAndAttributesFromXsdGroup(XmlSchemaGroupBase xsdGroup, List<string> itemsToRemove)
        {
            XmlSchemaObject[] originalItems = new XmlSchemaObject[xsdGroup.Items.Count];
            xsdGroup.Items.CopyTo(originalItems, 0);

            foreach (XmlSchemaObject item in originalItems)
            {
                if (item is XmlSchemaElement)
                {
                    if (itemsToRemove.Contains(((XmlSchemaElement) item).QualifiedName.Name))
                    {
                        xsdGroup.Items.Remove(item);
                    }                    
                }
                else if (item is XmlSchemaGroupBase)
                {
                    RemoveElementsAndAttributesFromXsdGroup((XmlSchemaGroupBase) item, itemsToRemove);
                }
            }
        }

        public void ExportSubset()
        {
            //throw new NotImplementedException();
        }
    }
}
