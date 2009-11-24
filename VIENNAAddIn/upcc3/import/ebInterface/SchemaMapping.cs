using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository.cc;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class SchemaMapping
    {
        private readonly Dictionary<string, ComplexTypeMapping> complexTypeMappings = new Dictionary<string, ComplexTypeMapping>();
        private readonly Dictionary<string, string> edges = new Dictionary<string, string>();
        private readonly MapForceSourceElementTree sourceElementStore;
        private readonly TargetElementStore targetElementStore;

        public SchemaMapping(MapForceMapping mapForceMapping, ICCLibrary ccLibrary)
        {
            sourceElementStore = new MapForceSourceElementTree(mapForceMapping);
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
                    return Mapping.NullElementMapping;
                }
                if (IsMappedToBCC(element))
                {
                    return new BCCMapping(element, GetTargetElement(element));
                }
                throw new MappingError("Simple typed element mapped to non-BCC CCTS element.");
            }
            if (element.HasComplexType())
            {
                ComplexTypeMapping complexTypeMapping = MapComplexType(element);
                if (IsUnmapped(element))
                {
                    return new ASBIEMapping(element.Name, complexTypeMapping);
                }
                if (IsMappedToASCC(element))
                {
                    if (complexTypeMapping.IsMappedToSingleACC)
                    {
                        IACC complexTypeACC = complexTypeMapping.TargetACCs.ElementAt(0);
                        IACC targetACC = ((IASCC) GetTargetElement(element).Reference).AssociatedElement;
                        if (complexTypeACC.Id == targetACC.Id)
                        {
                            return new ASCCMapping(element, GetTargetElement(element), complexTypeMapping);
                        }
                        throw new MappingError("Complex typed element mapped to ASCC with associated ACC other than the target ACC for the complex type.");
                    }
                    throw new MappingError("Complex typed element mapped to ASCC, but the complex type is not mapped to a single ACC.");
                }
                throw new MappingError("Complex typed element mapped to non-ASCC CCTS element.");
            }
            throw new Exception("Source element has neither simple nor complex type.");
        }

        private ComplexTypeMapping MapComplexType(SourceElement sourceElement)
        {
            string complexTypeName = sourceElement.GetComplexTypeName();
            if (ComplexTypeIsUnmapped(complexTypeName))
            {
                var complexTypeMapping = new ComplexTypeMapping(complexTypeName, MapChildren(sourceElement));
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
                if (childMapping != Mapping.NullElementMapping)
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
            return Mapping.NullComplexTypeMapping;
        }

        private bool IsMappedToASCC(SourceElement element)
        {
            TargetCCElement targetElement = GetTargetElement(element);
            return targetElement != null && targetElement.IsASCC;
        }

        private bool IsMappedToBCC(SourceElement element)
        {
            TargetCCElement targetElement = GetTargetElement(element);
            return targetElement != null && targetElement.IsBCC;
        }

        private TargetCCElement GetTargetElement(SourceElement element)
        {
            string targetElementKey;
            if (edges.TryGetValue(element.Key, out targetElementKey))
            {
                return targetElementStore.GetTargetElement(targetElementKey);
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
    }
}