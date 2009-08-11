using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class Mappings
    {
        public Mappings(MapForceMapping mapForceMapping, ICCLibrary ccLibrary)
        {
            var sourceElementStore = new SourceElementStore(mapForceMapping);
            var targetElementStore = new TargetElementStore(mapForceMapping, ccLibrary);

            MappingList = new List<Mapping>();
            foreach (var vertex in mapForceMapping.Graph.Vertices)
            {
                foreach (var edge in vertex.Edges)
                {
                    var sourceKey = vertex.Key;
                    var targetKey = edge.TargetVertexKey;
                    var sourceElement = sourceElementStore.GetSourceElement(sourceKey);
                    var targetElement = targetElementStore.GetTargetElement(targetKey);
                    if (sourceElement == null || targetElement == null)
                    {
                        // TODO error
                    }
                    else
                    {
                        MappingList.Add(new Mapping(sourceElement, targetElement));
                    }
                }
            }

            RootSourceElement = sourceElementStore.RootSourceElement;
        }

        public List<Mapping> MappingList { get; private set; }

        public SourceElement RootSourceElement { get; private set; }

        public IEnumerable<Mapping> GetACCMappings()
        {
            foreach (var mapping in MappingList)
            {
                if (mapping.TargetCC.IsACC)
                {
                    yield return mapping;
                }
            }
        }
    }
}