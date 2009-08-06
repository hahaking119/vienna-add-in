using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using System.Linq;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class MappingAdapter
    {
        private readonly MapForceMapping mapForceMapping;
        private readonly ICCRepository ccRepository;
        private readonly ICCLibrary ccl;
        private readonly Dictionary<string, SourceElement> sourceElementsByKey = new Dictionary<string, SourceElement>();
        private IBIELibrary bieLibrary;
        public SourceElement RootSourceElement { get; private set; }

        public List<Mapping> Mappings { get; private set; }

        public MappingAdapter(MapForceMapping mapForceMapping, ICCRepository ccRepository)
        {
            this.mapForceMapping = mapForceMapping;
            this.ccRepository = ccRepository;
            ccl = ccRepository.Libraries<ICCLibrary>().FirstOrDefault();
            if (ccl == null)
            {
                throw new Exception("No CCLibary found in repository.");
            }
            Mappings = new List<Mapping>();
        }

        private SourceElement CreateSourceElementTree(Entry entry)
        {
            var sourceElement = new SourceElement(entry.Name);
            AddToIndex(entry, sourceElement);
            foreach (var subEntry in entry.SubEntries)
            {
                sourceElement.AddChild(CreateSourceElementTree(subEntry));
            }
            return sourceElement;
        }

        private void AddToIndex(Entry entry, SourceElement sourceElement)
        {
            var key = entry.InputOutputKey.Value;
            if (key != null)
            {
                sourceElementsByKey[key] = sourceElement;
            }
        }

        public void GenerateMapping()
        {
            var rootSchemaComponent = mapForceMapping.GetRootSchemaComponent();
            RootSourceElement = CreateSourceElementTree(rootSchemaComponent.RootEntry);

            foreach (var vertex in mapForceMapping.Graph.Vertices)
            {
                foreach (var edge in vertex.Edges)
                {
                    var sourceKey = vertex.Key;
                    var targetKey = edge.TargetVertexKey;
                    var sourceElement = GetSourceElement(sourceKey);
                    if (sourceElement != null)
                    {
                        Mappings.Add(new Mapping(sourceElement, GetTargetElement(targetKey)));
                    }
                    else
                    {
                        // TODO error
                    }
                }
            }
        }

        private TargetCCElement GetTargetElement(string key)
        {
            // TODO search multiple target schema components
            var targetSchemaComponent = mapForceMapping.GetTargetSchemaComponent();
            var targetElementEntry = GetTargetElementEntry(targetSchemaComponent.RootEntry, key);
            IACC acc = GetACC(targetSchemaComponent);
            ICC reference;
            if (targetElementEntry == targetSchemaComponent.RootEntry)
            {
                reference = acc;
            }
            else
            {
                reference = GetBCCOrASCC(acc, targetElementEntry.Name);
            }
            return new TargetCCElement(targetElementEntry.Name, reference);
        }

        private ICC GetBCCOrASCC(IACC acc, string name)
        {
            var bccsAndASCCsByName = new Dictionary<string, ICC>();
            foreach (var bcc in acc.BCCs)
            {
                bccsAndASCCsByName[NDR.GenerateBCCName(bcc)] = bcc;
            }
            foreach (var ascc in acc.ASCCs)
            {
                bccsAndASCCsByName[NDR.GenerateASCCName(ascc)] = ascc;
            }
            return bccsAndASCCsByName[name];
        }

        private IACC GetACC(SchemaComponent component)
        {
            return ccl.ElementByName(component.RootEntry.Name);
        }

        private static Entry GetTargetElementEntry(Entry entry, string key)
        {
            if (entry.HasKey(key))
            {
                return entry;
            }
            foreach (var subEntry in entry.SubEntries)
            {
                var targetElementEntry = GetTargetElementEntry(subEntry, key);
                if (targetElementEntry != null)
                {
                    return targetElementEntry;
                }
            }
            return null;
        }

        private SourceElement GetSourceElement(string key)
        {
            SourceElement element;
            sourceElementsByKey.TryGetValue(key, out element);
            return element;
        }

        public void GenerateBIELibrary(string name, string bdtLibraryName)
        {
            var bLibrary = GetBLibrary();
            var bdtLibrary = bLibrary.CreateBDTLibrary(new LibrarySpec
                                                       {
                                                           Name = bdtLibraryName,
                                                       });
            bieLibrary = bLibrary.CreateBIELibrary(new LibrarySpec
                                                   {
                                                       Name = name,
                                                   });
            var accMappings = GetACCMappings();
            foreach (var accMapping in accMappings)
            {
                var acc = (IACC) accMapping.TargetCC.Reference;
                var bbies = new List<BBIESpec>();

                foreach (var bccMapping in GetBCCMappings())
                {
                    var bcc = (IBCC) bccMapping.TargetCC.Reference;
                    var bdtName = bcc.Type.Name;
                    var bdt = bdtLibrary.ElementByName(bdtName);
                    if (bdt == null)
                    {
                        var bdtSpec = BDTSpec.CloneCDT(bcc.Type, bdtName);
                        bdt = bdtLibrary.CreateElement(bdtSpec);
                    }
                    var bbieSpec = BBIESpec.CloneBCC(bcc, bdt);
                    bbies.Add(bbieSpec);
                }

                var abieSpec = new ABIESpec
                               {
                                   BasedOn = acc,
                                   Name = acc.Name,
                                   BBIEs = bbies,
                               };
                accMapping.TargetBIE = new TargetBIEElement(abieSpec.Name, bieLibrary.CreateElement(abieSpec));
            }
        }

        private IBLibrary GetBLibrary()
        {
            return (IBLibrary) ccl.Parent;
        }

        private IEnumerable<Mapping> GetACCMappings()
        {
            foreach (var mapping in Mappings)
            {
                if (mapping.TargetCC.IsACC)
                {
                    yield return mapping;
                }
            }
        }
        private IEnumerable<Mapping> GetBCCMappings()
        {
            foreach (var mapping in Mappings)
            {
                if (mapping.TargetCC.IsBCC)
                {
                    yield return mapping;
                }
            }
        }

        public void GenerateDOCLibrary(string docLibraryName)
        {
            var bLibrary = GetBLibrary();
            var docLibrary = bLibrary.CreateDOCLibrary(new LibrarySpec
            {
                Name = docLibraryName,
            });
            var asbies = new List<ASBIESpec>();
            foreach (var childElement in RootSourceElement.Children)
            {
                var abie = childElement.Mapping.TargetBIE.Reference as IABIE;
                if (abie == null)
                {
                    // TODO error
                }
                else
                {
                    asbies.Add(new ASBIESpec
                               {
                                   AggregationKind = EAAggregationKind.Composite,
                                   AssociatedABIEId = abie.Id,
                                   Name = "specified_" + abie.Name,
                               });
                }
            }
            var abieSpec = new ABIESpec
            {
                Name = RootSourceElement.Name,
                ASBIEs = asbies,
            };
            docLibrary.CreateElement(abieSpec);
        }
    }
}