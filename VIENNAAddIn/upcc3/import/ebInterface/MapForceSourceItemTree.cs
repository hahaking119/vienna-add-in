using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class MapForceSourceItemTree
    {
        private readonly MapForceMapping mapForceMapping;
        
        /// <exception cref="ArgumentException">The MapForce mapping does not contain any input schema components.</exception>
        public MapForceSourceItemTree(MapForceMapping mapForceMapping, XmlSchemaSet xmlSchemaSet)
        {
            this.mapForceMapping = mapForceMapping;
            xmlSchemaSet.Compile();
            /// Retrieve the schema component containing the input schemas' root element.
            /// 
            /// If there is only one input schema component, then this component is the root schema component.
            /// 
            /// If there are more than one input schema components, we look for a constant component with value "Root: *", where "*" must
            /// be the name of the root XSD element of the input schemas. We then return the schema component containing this element as its root.

            var inputSchemaComponents = new List<SchemaComponent>(mapForceMapping.GetInputSchemaComponents());

            var schemaComponentTrees = new List<SourceItem>();
            foreach (SchemaComponent inputSchemaComponent in inputSchemaComponents)
            {
                XmlSchemaElement rootXsdElement = (XmlSchemaElement) xmlSchemaSet.GlobalElements[inputSchemaComponent.InstanceRoot];
                SourceItem tree = CreateSourceItemTree(inputSchemaComponent.RootEntry, rootXsdElement);
                schemaComponentTrees.Add(tree);
            }

            if (schemaComponentTrees.Count == 0)
            {
                throw new MappingError("The MapForce mapping does not contain any input schema components.");
            }
            else if (schemaComponentTrees.Count == 1)
            {
                RootSourceItem = schemaComponentTrees[0];                
            } 
            else
            {
                var rootElementName = mapForceMapping.GetConstant("Root");
                if (string.IsNullOrEmpty(rootElementName))
                {
                    throw new MappingError("The MapForce mapping does not specify the root source element name.");
                }
                var nonRootElementTrees = new List<SourceItem>();
                foreach (SourceItem tree in schemaComponentTrees)
                {
                    if (tree.Name == rootElementName)
                    {
                        RootSourceItem = tree;
                    }
                    else
                    {
                        nonRootElementTrees.Add(tree);
                    }
                }
                if (RootSourceItem == null)
                {
                    throw new ArgumentException("The MapForce mapping does not contain an input schema component with the specified root element: " + rootElementName);
                }
                foreach (var nonRootElementTree in nonRootElementTrees)
                {
                    // TODO
                    AttachSubTree(nonRootElementTree, RootSourceItem);
                }            
            }
        }

        public SourceItem RootSourceItem { get; private set; }

        /// <summary>
        /// Perform name matching between the sub-tree's root element name and element names in the sourceElementTree.
        /// 
        /// When a matching element is found, attach the sub-tree root's children to the element in the source tree.
        /// </summary>
        /// <param name="subTreeRoot"></param>
        /// <param name="sourceElementTree"></param>
        private static void AttachSubTree(SourceItem subTreeRoot, SourceItem sourceElementTree)
        {
            if (subTreeRoot.XsdTypeName == sourceElementTree.XsdTypeName)
            {
                sourceElementTree.MergeWith(subTreeRoot);
            }
            foreach (var child in sourceElementTree.Children)
            {
                AttachSubTree(subTreeRoot, child);
            }
        }


        private SourceItem CreateSourceItemTree(Entry mapForceEntry, XmlSchemaObject sourceXsdObject)
        {
            XmlSchemaType xsdType = GetXsdTypeForXsdObject(sourceXsdObject);

            var sourceItem = new SourceItem(mapForceEntry.Name, xsdType, mapForceEntry.XsdObjectType, mapForceMapping.GetMappingTargetKey(mapForceEntry.InputOutputKey.Value));

            foreach (XmlSchemaObject childOfXsdType in GetChildElementsAndAttributesDefinedByXsdType(sourceItem.XsdType))
            {
                Entry mapForceSubEntry;

                if (childOfXsdType is XmlSchemaElement)
                {
                    mapForceSubEntry = mapForceEntry.GetSubEntryForElement(((XmlSchemaElement)childOfXsdType).QualifiedName.Name);
                }
                else if (childOfXsdType is XmlSchemaAttribute)
                {
                    mapForceSubEntry = mapForceEntry.GetSubEntryForAttribute(((XmlSchemaAttribute)childOfXsdType).QualifiedName.Name);
                }
                else
                {
                    throw new Exception("Child of XSD Type is neither an XSD Element nor an XSD Attribute. The type of the Child is " + sourceXsdObject.GetType());
                }

                if (mapForceSubEntry != null)
                {
                    SourceItem sourceItemTreeForChild = CreateSourceItemTree(mapForceSubEntry, childOfXsdType);

                    sourceItem.AddChild(sourceItemTreeForChild);
                }
                else
                {
                    XmlSchemaType xsdTypeForChild = GetXsdTypeForXsdObject(childOfXsdType);

                    if (childOfXsdType is XmlSchemaElement)
                        sourceItem.AddChild(new SourceItem(((XmlSchemaElement)childOfXsdType).QualifiedName.Name, xsdTypeForChild, XsdObjectType.Element, null));
                        

                    if (childOfXsdType is XmlSchemaAttribute)
                        sourceItem.AddChild(new SourceItem(((XmlSchemaAttribute)childOfXsdType).QualifiedName.Name, xsdTypeForChild, XsdObjectType.Attribute, null));
                }
            }

            return sourceItem;
        }

        private static XmlSchemaType GetXsdTypeForXsdObject(XmlSchemaObject sourceXsdObject)
        {
            if (sourceXsdObject is XmlSchemaElement)
            {
                return ((XmlSchemaElement)sourceXsdObject).ElementSchemaType;    
            }

            if (sourceXsdObject is XmlSchemaAttribute)
            {
                return ((XmlSchemaAttribute)sourceXsdObject).AttributeSchemaType;
            }

            throw new ArgumentException("Source XSD Object is neither an XSD Element nor an XSD Attribute. The type of the XSD Object is " + sourceXsdObject.GetType());
        }

        private IEnumerable<XmlSchemaObject> GetChildElementsAndAttributesDefinedByXsdType(XmlSchemaType xmlSchemaType)
        {
           if (xmlSchemaType is XmlSchemaComplexType)
           {
               XmlSchemaComplexType complexType = (XmlSchemaComplexType) xmlSchemaType;

               if (complexType.Particle is XmlSchemaGroupBase)
               {
                   foreach (var child in GetChildElementsAndAttributesDefinedByXsdGroup((XmlSchemaGroupBase) complexType.Particle))
                   {
                       yield return child;
                   }
               }

               //if (complexType.ContentTypeParticle is XmlSchemaSequence)
               //{
               //    foreach (var child in GetChildElementsAndAttributesDefinedByXsdGroup((XmlSchemaGroupBase) complexType.ContentTypeParticle))
               //    {
               //        yield return child;
               //    }
               //}

               if (complexType.ContentModel is XmlSchemaSimpleContent)
               {
                   XmlSchemaSimpleContent contentModel = (XmlSchemaSimpleContent) complexType.ContentModel;
                   XmlSchemaSimpleContentExtension content = (XmlSchemaSimpleContentExtension) contentModel.Content;

                   foreach (XmlSchemaAttribute attribute in content.Attributes)
                   {
                       yield return attribute;
                   }
               }

               foreach (XmlSchemaAttribute attribute in complexType.Attributes)
               {
                   yield return attribute;
               }
           }

            if (xmlSchemaType.BaseXmlSchemaType != null)
           {
               foreach (var child in GetChildElementsAndAttributesDefinedByXsdType(xmlSchemaType.BaseXmlSchemaType))
               {
                   yield return child;
               }
           }
        }

        private IEnumerable<XmlSchemaObject> GetChildElementsAndAttributesDefinedByXsdGroup(XmlSchemaGroupBase xsdGroup)
        {
            foreach (XmlSchemaObject item in xsdGroup.Items)
            {
                if (item is XmlSchemaElement)
                {
                    yield return item;
                }
                else if (item is XmlSchemaGroupBase)
                {
                    foreach (var child in GetChildElementsAndAttributesDefinedByXsdGroup((XmlSchemaGroupBase)item))
                    {
                        yield return child;
                    }
                }
            }
        }
    }
}