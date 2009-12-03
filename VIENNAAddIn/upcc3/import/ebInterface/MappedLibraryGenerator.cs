using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.bLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    /// <summary>
    /// This Class manages as a last step of the generic XML Schema importer the generation of proper 
    /// BIE and BIE Doc elements in the corresponding UPCC libraries.
    /// </summary>
    public class MappedLibraryGenerator
    {
        private readonly IBLibrary bLibrary;
        private readonly string docLibraryName;
        private readonly string bieLibraryName;
        private readonly string bdtLibraryName;
        private readonly string qualifier;
        private readonly string rootElementName;
        private readonly SchemaMapping schemaMapping;
        private readonly List<AbieSpec> abieSpecs = new List<AbieSpec>();
        private readonly List<MaSpec> maSpecs = new List<MaSpec>();
        private IBdtLibrary bdtLibrary;
        private IBieLibrary bieLibrary;
        private IDocLibrary docLibrary;

        public MappedLibraryGenerator(SchemaMapping schemaMapping, IBLibrary bLibrary, string docLibraryName, string bieLibraryName, string bdtLibraryName, string qualifier, string rootElementName)
        {
            this.bLibrary = bLibrary;
            this.docLibraryName = docLibraryName;
            this.bieLibraryName = bieLibraryName;
            this.bdtLibraryName = bdtLibraryName;
            this.qualifier = qualifier;
            this.rootElementName = rootElementName;
            this.schemaMapping = schemaMapping;
        }

        /// <summary>
        /// Retrieves the BDT based on the given CDT from the BDT library. If the BDT does not yet exist, it is created.
        /// </summary>
        /// <param name="cdt"></param>
        /// <returns></returns>
        private IBdt GetBDT(ICdt cdt)
        {
            var bdtName = qualifier + "_" + cdt.Name;
            var bdt = bdtLibrary.GetBdtByName(bdtName);
            if (bdt == null)
            {
                var bdtSpec = BdtSpec.CloneCdt(cdt, bdtName);
                bdt = bdtLibrary.CreateBdt(bdtSpec);
            }
            return bdt;
        }

        /// <summary>
        /// Takes the names of the libraries to be created as well as a qualifier as input and creates the
        /// libraries.
        /// </summary>
        public void GenerateLibraries()
        {
            bdtLibrary = bLibrary.CreateBDTLibrary(new BdtLibrarySpec
                                                   {
                                                       Name = bdtLibraryName
                                                   });
            bieLibrary = bLibrary.CreateBIELibrary(new BieLibrarySpec
                                                   {
                                                       Name = bieLibraryName
                                                   });
            docLibrary = bLibrary.CreateDOCLibrary(new DocLibrarySpec
                                                   {
                                                       Name = docLibraryName
                                                   });
            GenerateBDTsAndABIEs();
            GenerateRootABIE();
        }

        private void GenerateRootABIE()
        {
            throw new NotImplementedException();
//            var rootElementMapping = schemaMapping.RootElementMapping;
//            if (rootElementMapping is ASBIEMapping)
//            {
//                ComplexTypeMapping rootComplexTypeMapping = ((ASBIEMapping) rootElementMapping).TargetMapping;
//                docLibrary.CreateAbie(new AbieSpec
//                                         {
//                                             Name = qualifier + "_" + rootElementName,
//                                             Asbies = new List<AsbieSpec>
//                                                      {
//                                                          new AsbieSpec
//                                                          {
//                                                              AggregationKind = AsbieAggregationKind.Composite,
//                                                              Name = rootComplexTypeMapping.BIEName,
//                                                              ResolveAssociatedABIE = DeferredABIEResolver(rootComplexTypeMapping),
//                                                          }
//                                                      },
//                                         });
//            }
//            else if (rootElementMapping is BCCMapping)
//            {
//                var bccMapping = (BCCMapping) rootElementMapping;
//                docLibrary.CreateAbie(new AbieSpec
//                                         {
//                                             BasedOn = bccMapping.ACC,
//                                             Name = qualifier + "_" + rootElementName,
//                                             Bbies = GenerateBCCMappings(new List<BCCMapping>{bccMapping}),
//                                         });
//            }
//            else
//            {
//                throw new MappingError("Root element can only be mapped to BCC, but is mapped to something else.");
//            }
        }

        private void GenerateBDTsAndABIEs()
        {
            foreach (var complexTypeMapping in schemaMapping.GetComplexTypeMappings())
            {
                GenerateComplexTypeBIESpecs(complexTypeMapping);
            }
            CreateAbies(abieSpecs);
            CreateMas(maSpecs);
        }

        private void CreateAbies(IEnumerable<AbieSpec> abieSpecs)
        {
            // need two passes:
            //  (1) create the ABIEs
            //  (2) create the ASBIEs
            var abies = new Dictionary<string, IAbie>();
            foreach (AbieSpec spec in abieSpecs)
            {
                var specWithoutASBIEs = new AbieSpec
                                        {
                                            BusinessTerms = spec.BusinessTerms,
                                            Definition = spec.Definition,
                                            DictionaryEntryName = spec.DictionaryEntryName,
                                            IsEquivalentTo = spec.IsEquivalentTo,
                                            LanguageCode = spec.LanguageCode,
                                            Name = spec.Name,
                                            UniqueIdentifier = spec.UniqueIdentifier,
                                            UsageRules = spec.UsageRules,
                                            VersionIdentifier = spec.VersionIdentifier
                                        };
                foreach (BbieSpec bbieSpec in spec.Bbies)
                {
                    specWithoutASBIEs.AddBbie(bbieSpec);
                }
                abies[spec.Name] = bieLibrary.CreateAbie(specWithoutASBIEs);
            }
            foreach (AbieSpec spec in abieSpecs)
            {
                var abie = abies[spec.Name];
                bieLibrary.UpdateAbie(abie, spec);
            }
        }

        private void CreateMas(IEnumerable<MaSpec> abieSpecs)
        {
            throw new NotImplementedException();
//            // need two passes:
//            //  (1) create the ABIEs
//            //  (2) create the ASBIEs
//            var abies = new Dictionary<string, IAbie>();
//            foreach (AbieSpec spec in abieSpecs)
//            {
//                var specWithoutASBIEs = new AbieSpec
//                                        {
//                                            BusinessTerms = spec.BusinessTerms,
//                                            Definition = spec.Definition,
//                                            DictionaryEntryName = spec.DictionaryEntryName,
//                                            IsEquivalentTo = spec.IsEquivalentTo,
//                                            LanguageCode = spec.LanguageCode,
//                                            Name = spec.Name,
//                                            UniqueIdentifier = spec.UniqueIdentifier,
//                                            UsageRules = spec.UsageRules,
//                                            VersionIdentifier = spec.VersionIdentifier
//                                        };
//                foreach (BbieSpec bbieSpec in spec.Bbies)
//                {
//                    specWithoutASBIEs.AddBbie(bbieSpec);
//                }
//                abies[spec.Name] = docLibrary.CreateAbie(specWithoutASBIEs);
//            }
//            foreach (AbieSpec spec in abieSpecs)
//            {
//                var abie = abies[spec.Name];
//                docLibrary.UpdateAbie(abie, spec);
//            }
        }

        private void GenerateComplexTypeBIESpecs(ComplexTypeMapping abieMapping)
        {
            if (abieMapping.IsMappedToSingleACC)
            {
                GenerateBIELibraryABIESpec(abieMapping, abieMapping.TargetACCs.ElementAt(0), abieMapping.BIEName);
            }
            else
            {
                var asmaSpecs = new List<AsmaSpec>();
                foreach (IAcc acc in abieMapping.TargetACCs)
                {
                    var accABIESpec = GenerateBIELibraryABIESpec(abieMapping, acc, abieMapping.ComplexTypeName + "_" + acc.Name);
                    asmaSpecs.Add(new AsmaSpec
                                   {
                                       Name = acc.Name,
                                       ResolveAssociatedAbie = DeferredAbieResolver(accABIESpec.Name),
                                   });
                }
                foreach (var asbieMapping in abieMapping.ASBIEMappings)
                {
                    asmaSpecs.Add(new AsmaSpec
                                   {
                                       Name = asbieMapping.BIEName,
                                       ResolveAssociatedMa = DeferredMaResolver(asbieMapping.TargetMapping.BIEName),
                                   });
                }
                maSpecs.Add(new MaSpec
                                 {
                                     Name = abieMapping.ComplexTypeName,
                                     Asmas = asmaSpecs,
                                 });
            }
        }

        private AbieSpec GenerateBIELibraryABIESpec(ComplexTypeMapping abieMapping, IAcc acc, string name)
        {
            var abieSpec = new AbieSpec
                           {
                               BasedOn = acc,
                               Name = name,
                               Bbies = GenerateBCCMappings(abieMapping.BCCMappings(acc)),
                               Asbies = GenerateASCCMappings(abieMapping.ASCCMappings(acc)),
                           };
            abieSpecs.Add(abieSpec);
            return abieSpec;
        }

        private IEnumerable<AsbieSpec> GenerateASCCMappings(IEnumerable<ASCCMapping> asccMappings)
        {
            foreach (var asccMapping in asccMappings)
            {
                var ascc = asccMapping.ASCC;
                var targetMapping = asccMapping.TargetMapping;
                var asbieSpec = AsbieSpec.CloneASCC(ascc, asccMapping.BIEName, DeferredAbieResolver(targetMapping.BIEName));
                yield return asbieSpec;
            }
        }

        private IEnumerable<BbieSpec> GenerateBCCMappings(IEnumerable<BCCMapping> bccMappings)
        {
            foreach (var bccMapping in bccMappings)
            {
                var bcc = bccMapping.BCC;
                var bbieSpec = BbieSpec.CloneBCC(bcc, GetBDT(bcc.Cdt));
                bbieSpec.Name = bccMapping.BIEName;
                yield return bbieSpec;
            }
        }

        private Func<IAbie> DeferredAbieResolver(string abieName)
        {
            return () => bieLibrary.GetAbieByName(abieName);
        }

        private Func<IMa> DeferredMaResolver(string maName)
        {
            return () => docLibrary.GetMaByName(maName);
        }
    }
}