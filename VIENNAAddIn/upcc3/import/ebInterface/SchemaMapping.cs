using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;

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

        public SchemaMapping(MapForceMapping mapForceMapping, XmlSchemaSet xmlSchemaSet, ICcLibrary ccLibrary)
        {
            sourceElementStore = new MapForceSourceElementTree(mapForceMapping, xmlSchemaSet);
            targetElementStore = new TargetElementStore(mapForceMapping, ccLibrary);
            
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

            RootElementMapping = MapElement(sourceElementStore.RootSourceElement);
        }

        public ElementMapping RootElementMapping { get; private set; }

        public SourceElement RootSourceElement
        {
            get { return sourceElementStore.RootSourceElement; }
        }

        /// <exception cref="MappingError">Simple typed element mapped to non-BCC CCTS element.</exception>
        private ElementMapping MapElement(SourceElement element)
        {
            if (element.HasSimpleType())
            {
                if (IsUnmapped(element))
                {
                    // ignore element
                    return ElementMapping.NullElementMapping;
                }
                if (IsMappedToBcc(element))
                {
                    SimpleTypeToCdtMapping simpleTypeToCdtMapping = MapSimpleType(element);
                    return new AttributeOrSimpleElementOrComplexElementToBccMapping(element, (IBcc) GetTargetElement(element), simpleTypeToCdtMapping);
                }
                if (IsMappedToSup(element))
                {
                    return new AttributeOrSimpleElementToSupMapping(element, (ICdtSup) GetTargetElement(element));
                }
                if (IsMappedToSplitFunction(element))
                {
                    return new SplitMapping(element, GetMappingFunction(element).TargetCcs);
                }
                throw new MappingError("Simple typed element mapped to non-BCC CCTS element.");
            }
            if (element.HasComplexType())
            {
                ComplexTypeMapping complexTypeMapping = MapComplexType(element);
                if (IsUnmapped(element))
                {
                    return new AsmaMapping(element.Name, complexTypeMapping);
                }
                if (IsMappedToAscc(element))
                {
                    if (complexTypeMapping.IsMappedToSingleACC)
                    {
                        IAcc complexTypeACC = complexTypeMapping.TargetACCs.ElementAt(0);
                        IAscc targetAscc = (IAscc)GetTargetElement(element);
                        IAcc targetACC = targetAscc.AssociatedAcc;
                        if (complexTypeACC.Id == targetACC.Id)
                        {
                            return new ComplexElementToAsccMapping(element, targetAscc, complexTypeMapping);
                        }
                        throw new MappingError("Complex typed element mapped to ASCC with associated ACC other than the target ACC for the complex type.");
                    }
                    throw new MappingError("Complex typed element mapped to ASCC, but the complex type is not mapped to a single ACC.");
                }
                if (IsMappedToBcc(element))
                {
                    if (complexTypeMapping.IsMappedToCdt)
                    {
                        ICdt complexTypeCdt = complexTypeMapping.TargetCdt;
                        IBcc targetBcc = (IBcc)GetTargetElement(element);
                        ICdt targetCdt = targetBcc.Cdt;

                        if (complexTypeCdt.Id == targetCdt.Id)
                        {
                            return new AttributeOrSimpleElementOrComplexElementToBccMapping(element, targetBcc, complexTypeMapping);
                        }

                        throw new MappingError("Complex typed element mapped to BCC with CDT other than the target CDT for the complex type.");
                    }
                    throw new MappingError("Complex typed element mapped to BCC, but the complex type is not mapped to a CDT.");                    

                }
                throw new MappingError("Complex typed element mapped to non-ASCC CCTS element.");
            }
            throw new Exception("Source element " + element.Name + " has neither simple nor complex type.");
        }

        private SimpleTypeToCdtMapping MapSimpleType(SourceElement sourceElement)
        {
            var simpleTypeName = sourceElement.XsdTypeName;
            var cdt = ((IBcc)GetTargetElement(sourceElement)).Cdt;
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

        private ComplexTypeMapping MapComplexType(SourceElement sourceElement)
        {
            string complexTypeName = sourceElement.XsdTypeName;
            if (ComplexTypeIsUnmapped(complexTypeName))
            {
                IEnumerable<ElementMapping> childMappings = MapChildren(sourceElement);

                IAcc targetAcc = null;
                ICdt targetCdt = null;
                bool hasMultipleAccMappings = false;
                bool hasAsmaMapping = false;
                ComplexTypeMapping complexTypeMapping;

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

                bool hasCdtMapping = targetCdt != null;
                bool hasAccMapping = targetAcc != null;

                if (hasCdtMapping)
                {
                    if ((!hasAccMapping) && (!hasMultipleAccMappings) && (!hasAsmaMapping))
                    {
                        // CDT
                        complexTypeMapping = new ComplexTypeToCdtMapping(complexTypeName, childMappings);
                    }
                    else
                    {
                        throw new MappingError("Mapping Error #374.");
                    }
                }
                else
                {
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
                        throw new MappingError("Mapping Error #375.");
                    }
                }

                complexTypeMappings[complexTypeName] = complexTypeMapping;
                return complexTypeMapping;
            }
            return GetComplexTypeMapping(complexTypeName);
        }

        private IEnumerable<ElementMapping> MapChildren(SourceElement sourceElement)
        {
            foreach (var child in sourceElement.Children)
            {
                var childMapping = MapElement(child);
                if (childMapping != ElementMapping.NullElementMapping)
                {
                    yield return childMapping;
                }
            }
        }

        private bool ComplexTypeIsUnmapped(string complexTypeName)
        {
            return !complexTypeMappings.ContainsKey(complexTypeName);
        }

        private ComplexTypeMapping GetComplexTypeMapping(string complexTypeName)
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