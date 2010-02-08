using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class SchemaMapping
    {
        private readonly Dictionary<string, ComplexTypeMapping> complexTypeMappings = new Dictionary<string, ComplexTypeMapping>();
        private readonly List<SimpleTypeToCdtMapping> simpleTypeMappings = new List<SimpleTypeToCdtMapping>();
        private readonly Dictionary<string, string> edges = new Dictionary<string, string>();
        private readonly MapForceSourceElementTree sourceElementStore;
        private readonly TargetElementStore targetElementStore;
        private readonly MappingFunctionStore mappingFunctionStore;
        private readonly List<ElementMapping> elementMappings = new List<ElementMapping>();

        public SchemaMapping(MapForceMapping mapForceMapping, XmlSchemaSet xmlSchemaSet, ICcLibrary ccLibrary, ICctsRepository cctsRepository)
        {
            Console.Out.WriteLine("Building source tree:");
            sourceElementStore = new MapForceSourceElementTree(mapForceMapping, xmlSchemaSet);

            PrintSourceElementTree(sourceElementStore.RootSourceElement, "");
            Console.Out.WriteLine("Done.");

            Console.Out.WriteLine("Building target element store:");
            targetElementStore = new TargetElementStore(mapForceMapping, ccLibrary, cctsRepository);
            Console.Out.WriteLine("Done.");
            
            foreach (Vertex vertex in mapForceMapping.Graph.Vertices)
            {
                foreach (Edge edge in vertex.Edges)
                {
                    string sourceKey = vertex.Key;
                    string targetKey = edge.TargetVertexKey;
                    edges[sourceKey] = targetKey;
                }
            }
            mappingFunctionStore = new MappingFunctionStore(mapForceMapping, edges, targetElementStore);

            Console.Out.WriteLine("Deriving implicit mappings:");
            RootElementMapping = MapElement(sourceElementStore.RootSourceElement, "/" + sourceElementStore.RootSourceElement.Name, new Stack<XmlQualifiedName>());
            elementMappings.Add(RootElementMapping);
            Console.Out.WriteLine("Done.");

            elementMappings = new List<ElementMapping>(ResolveTypeMappings(elementMappings));

            int numberOfExplicitMappings = 0;
            foreach (ElementMapping em in elementMappings)
            {
                if ((em is AttributeOrSimpleElementOrComplexElementToBccMapping) || (em is AttributeOrSimpleElementToSupMapping) || (em is ComplexElementToAsccMapping) || (em is SplitMapping))
                {
                    numberOfExplicitMappings++;
                }
            }
            Console.Out.WriteLine("Number of Explicit Mappings: " + numberOfExplicitMappings);
        }

        private IEnumerable<ElementMapping> ResolveTypeMappings(IEnumerable<ElementMapping> unresolvedElementMappings)
        {
            foreach (ElementMapping elementMapping in unresolvedElementMappings)
            {
                if (elementMapping.ResolveTypeMapping(this))
                {
                    yield return elementMapping;
                }
            }
        }

        private static void PrintSourceElementTree(SourceElement element, string indent)
        {
            Console.Out.WriteLine(indent + element.Name);
            foreach (var child in element.Children)
            {
                PrintSourceElementTree(child, indent + "    ");
            }
        }

        public ElementMapping RootElementMapping { get; private set; }

        /// <exception cref="MappingError">Simple typed element mapped to non-BCC CCTS element.</exception>
        private ElementMapping MapElement(SourceElement sourceElement, string path, Stack<XmlQualifiedName> parentComplexTypeNames)
        {
            if (sourceElement.HasSimpleType())
            {
                if (IsUnmapped(sourceElement))
                {
                    // ignore element
                    return ElementMapping.NullElementMapping;
                }
                if (IsMappedToBcc(sourceElement))
                {
                    SimpleTypeToCdtMapping simpleTypeToCdtMapping = MapSimpleType(sourceElement);
                    return new AttributeOrSimpleElementOrComplexElementToBccMapping(sourceElement, (IBcc) GetTargetElement(sourceElement), simpleTypeToCdtMapping);
                }
                if (IsMappedToSup(sourceElement))
                {
                    return new AttributeOrSimpleElementToSupMapping(sourceElement, (ICdtSup) GetTargetElement(sourceElement));
                }
                if (IsMappedToSplitFunction(sourceElement))
                {
                    List<SimpleTypeToCdtMapping> simpleTypeToCdtMappings = new List<SimpleTypeToCdtMapping>();
                    MappingFunction splitFunction = GetMappingFunction(sourceElement);
                    foreach (IBcc targetBcc in splitFunction.TargetCcs)
                    {
                        simpleTypeToCdtMappings.Add(MapSimpleType(sourceElement, targetBcc));
                    }
                    return new SplitMapping(sourceElement, splitFunction.TargetCcs.Convert(cc => (IBcc) cc), simpleTypeToCdtMappings);
                }
                throw new MappingError("Simple typed element '" + path + "' mapped to non-BCC CCTS element.");
            }
            
            if (sourceElement.HasComplexType())
            {
                bool complexTypeIsMapped = MapComplexType(sourceElement, path, parentComplexTypeNames);

                if (!complexTypeIsMapped)
                {
                    // ignore element
                    return ElementMapping.NullElementMapping;
                }
                if (IsUnmapped(sourceElement))
                {
                    return new AsmaMapping(sourceElement);
                }
                if (IsMappedToAscc(sourceElement))
                {
                    IAscc targetAscc = (IAscc)GetTargetElement(sourceElement);
                    return new ComplexElementToAsccMapping(sourceElement, targetAscc);
                }
                if (IsMappedToBcc(sourceElement))
                {
                    IBcc targetBcc = (IBcc)GetTargetElement(sourceElement);
                    return new AttributeOrSimpleElementOrComplexElementToBccMapping(sourceElement, targetBcc, null);
                }

                throw new MappingError("Complex typed element '" + path + "' mapped to non-ASCC CCTS element.");
            }
            throw new Exception("Source element '" + path + "' has neither simple nor complex type.");
        }

        private SimpleTypeToCdtMapping MapSimpleType(SourceElement sourceElement)
        {
            return MapSimpleType(sourceElement, (IBcc)GetTargetElement(sourceElement));
        }

        private SimpleTypeToCdtMapping MapSimpleType(SourceElement sourceElement, IBcc targetBcc)
        {
            var simpleTypeName = sourceElement.XsdTypeName;
            var cdt = targetBcc.Cdt;
            foreach (SimpleTypeToCdtMapping simpleTypeMapping in simpleTypeMappings)
            {
                if (simpleTypeMapping.SimpleTypeName == simpleTypeName && simpleTypeMapping.TargetCDT.Id == cdt.Id)
                {
                    return simpleTypeMapping;
                }
            }
            SimpleTypeToCdtMapping newMapping = new SimpleTypeToCdtMapping(simpleTypeName, cdt);
            simpleTypeMappings.Add(newMapping);
            return newMapping;
        }

        private bool MapComplexType(SourceElement sourceElement, string path, Stack<XmlQualifiedName> parentComplexTypeNames)
        {
            XmlQualifiedName qualifiedComplexTypeName = sourceElement.XsdType.QualifiedName;
            if (parentComplexTypeNames.Contains(qualifiedComplexTypeName))
            {
                return true;
            }

            string complexTypeName = sourceElement.XsdTypeName;

            #region ComplexType => BCC Mapping

            if ((IsMappedToBcc(sourceElement)) && ComplexTypeIsUnmapped(complexTypeName + ((IBcc)GetTargetElement(sourceElement)).Cdt.Name))
            {
                ICdt targetCdt = null;
                ComplexTypeMapping complexTypeMapping;
                List<ElementMapping> childMappings = GetChildMappings(sourceElement, parentComplexTypeNames, qualifiedComplexTypeName, path);

                if (childMappings.Count() > 0)
                {
                    foreach (ElementMapping child in childMappings)
                    {
                        if (child is AttributeOrSimpleElementToSupMapping)
                        {
                            if (targetCdt == null)
                            {
                                targetCdt = ((AttributeOrSimpleElementToSupMapping)child).Cdt;
                            }
                            else
                            {
                                if (targetCdt.Id != ((AttributeOrSimpleElementToSupMapping)child).Cdt.Id)
                                {
                                    throw new MappingError("Complex type mapped to more than one CDTs");
                                }
                            }
                        }
                    }

                    complexTypeMapping = new ComplexTypeToCdtMapping(complexTypeName, childMappings);
                    complexTypeMappings[complexTypeName + ((IBcc)GetTargetElement(sourceElement)).Cdt.Name] = complexTypeMapping;

                    return true;
                }
                else
                {
                    if (sourceElement.HasSimpleContent())
                    {
                        targetCdt = ((IBcc)GetTargetElement(sourceElement)).Cdt;

                        complexTypeMapping = new ComplexTypeToCdtMapping(complexTypeName, childMappings) { TargetCdt = targetCdt };

                        complexTypeMappings[complexTypeName + ((IBcc)GetTargetElement(sourceElement)).Cdt.Name] = complexTypeMapping;

                        return true;                        
                    }
                }                
            }
            #endregion

            if (ComplexTypeIsUnmapped(complexTypeName))
            {
                IAcc targetAcc = null;
                ICdt targetCdt = null;
                bool hasMultipleAccMappings = false;
                bool hasAsmaMapping = false;
                ComplexTypeMapping complexTypeMapping;

                List<ElementMapping> childMappings = GetChildMappings(sourceElement, parentComplexTypeNames, qualifiedComplexTypeName, path);

                if (childMappings.Count() == 0)
                {
                    //if ((IsMappedToBcc(sourceElement)) && (sourceElement.HasSimpleContent()))
                    //{
                    //    targetCdt = ((IBcc)GetTargetElement(sourceElement)).Cdt;

                    //    complexTypeMapping = new ComplexTypeToCdtMapping(complexTypeName, childMappings) { TargetCdt = targetCdt };

                    //    complexTypeMappings[complexTypeName] = complexTypeMapping;

                    //    return true;
                    //}

                    // complex type not mapped
                    return false;
                }

                foreach (ElementMapping child in childMappings)
                {
                    //if (child is AttributeOrSimpleElementToSupMapping)
                    //{
                    //    if (targetCdt == null)
                    //    {
                    //        targetCdt = ((AttributeOrSimpleElementToSupMapping)child).Cdt;
                    //    }
                    //    else
                    //    {
                    //        if (targetCdt.Id != ((AttributeOrSimpleElementToSupMapping)child).Cdt.Id)
                    //        {
                    //            throw new MappingError("Complex type mapped to more than one CDTs");
                    //        }
                    //    }
                    //}

                    if (child is AttributeOrSimpleElementOrComplexElementToBccMapping)
                    {
                        if (targetAcc == null)
                        {
                            targetAcc = ((AttributeOrSimpleElementOrComplexElementToBccMapping) child).Acc;
                        }
                        else
                        {
                            if (targetAcc.Id != ((AttributeOrSimpleElementOrComplexElementToBccMapping) child).Acc.Id)
                            {
                                hasMultipleAccMappings = true;
                            }
                        }
                    }

                    if (child is ComplexElementToAsccMapping)
                    {
                        if (targetAcc == null)
                        {
                            targetAcc = ((ComplexElementToAsccMapping) child).Acc;
                        }
                        else
                        {
                            if (targetAcc.Id != ((ComplexElementToAsccMapping) child).Acc.Id)
                            {
                                hasMultipleAccMappings = true;
                            }
                        }
                    }

                    if (child is AsmaMapping)
                    {
                        hasAsmaMapping = true;
                    }
                }

                //bool hasCdtMapping = targetCdt != null;
                bool hasAccMapping = targetAcc != null;

                //if (hasCdtMapping)
                //{
                //    if ((!hasAccMapping) && (!hasMultipleAccMappings) && (!hasAsmaMapping))
                //    {
                //        // CDT
                //        complexTypeMapping = new ComplexTypeToCdtMapping(complexTypeName, childMappings);
                //    }
                //    else
                //    {
                //        throw new MappingError("Mapping Error #374.");
                //    }
                //}
                //else
                //{
                    if ((hasAccMapping) && (!hasMultipleAccMappings) && (!hasAsmaMapping))
                    {
                        // ACC
                        complexTypeMapping = new ComplexTypeToAccMapping(complexTypeName, childMappings);
                    }
                    else if ((!hasMultipleAccMappings && hasAsmaMapping) || ((hasAccMapping) && hasMultipleAccMappings))
                    {
                        // MA
                        complexTypeMapping = new ComplexTypeToMaMapping(complexTypeName, childMappings);
                    }
                    else
                    {
                        throw new MappingError("Mapping Error #375. Source Element: " + sourceElement.Name);
                    }
                //}

                complexTypeMappings[complexTypeName] = complexTypeMapping;
                return true;
            }
            return true;
        }

        private List<ElementMapping> GetChildMappings(SourceElement sourceElement, Stack<XmlQualifiedName> parentComplexTypeNames, XmlQualifiedName qualifiedComplexTypeName, string path)
        {
            List<ElementMapping> childMappings = new List<ElementMapping>();
            foreach (var child in sourceElement.Children)
            {
                parentComplexTypeNames.Push(qualifiedComplexTypeName);
                var childMapping = MapElement(child, path + "/" + child.Name, parentComplexTypeNames);
                parentComplexTypeNames.Pop();
                if (childMapping != ElementMapping.NullElementMapping)
                {
                    elementMappings.Add(childMapping);
                    childMappings.Add(childMapping);
                }
            }
            return childMappings;
        }

        private bool ComplexTypeIsUnmapped(string complexTypeName)
        {
            return !complexTypeMappings.ContainsKey(complexTypeName);
        }

        public ComplexTypeMapping GetComplexTypeMapping(XmlSchemaType complexType)
        {
            ComplexTypeMapping mapping;
            if (complexTypeMappings.TryGetValue(complexType.Name, out mapping))
            {
                return mapping;
            }
            return ComplexTypeMapping.NullComplexTypeMapping;
        }

        public ComplexTypeMapping GetComplexTypeToCdtMapping(string complexTypeName)
        {
            ComplexTypeMapping mapping;
            if (complexTypeMappings.TryGetValue(complexTypeName, out mapping))
            {
                return mapping;
            }
            return ComplexTypeMapping.NullComplexTypeMapping;
        }

        private bool IsMappedToAscc(SourceElement element)
        {
            object targetElement = GetTargetElement(element);
            return targetElement != null && targetElement is IAscc;
        }

        private bool IsMappedToBcc(SourceElement element)
        {
            object targetElement = GetTargetElement(element);
            return targetElement != null && targetElement is IBcc;
        }

        private bool IsMappedToSup(SourceElement element)
        {
            object targetElement = GetTargetElement(element);
            return targetElement != null && targetElement is ICdtSup;
        }


        private bool IsMappedToSplitFunction(SourceElement element)
        {
            MappingFunction mappingFunction = GetMappingFunction(element);
            return mappingFunction != null && mappingFunction.IsSplit;
        }

        private MappingFunction GetMappingFunction(SourceElement element)
        {
            string mappingFunctionKey;

            if (edges.TryGetValue(element.Key, out mappingFunctionKey))
            {
                return mappingFunctionStore.GetMappingFunction(mappingFunctionKey);
            }
            return null;
        }

        private object GetTargetElement(SourceElement element)
        {
            string targetElementKey;
            if (edges.TryGetValue(element.Key, out targetElementKey))
            {
                return targetElementStore.GetTargetCc(targetElementKey);
            }
            return null;
        }

        private bool IsUnmapped(SourceElement element)
        {
            return !edges.ContainsKey(element.Key);
        }

        public IEnumerable<ComplexTypeMapping> GetComplexTypeMappings()
        {
            return complexTypeMappings.Values;
        }

        public IEnumerable<SimpleTypeToCdtMapping> GetSimpleTypeMappings()
        {
            return simpleTypeMappings;
        }
    }
}