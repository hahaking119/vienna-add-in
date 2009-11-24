using System;
using System.Collections.Generic;
using CctsRepository;
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
        private readonly List<ABIESpec> bieABIESpecs = new List<ABIESpec>();
        private readonly List<ABIESpec> docABIESpecs = new List<ABIESpec>();
        private IBDTLibrary bdtLibrary;
        private IBIELibrary bieLibrary;
        private IDOCLibrary docLibrary;

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

        private static LibrarySpec SpecifyLibraryNamed(string name)
        {
            return new LibrarySpec
                   {
                       Name = name
                   };
        }

        /// <summary>
        /// Retrieves the BDT based on the given CDT from the BDT library. If the BDT does not yet exist, it is created.
        /// </summary>
        /// <param name="cdt"></param>
        /// <returns></returns>
        private IBDT GetBDT(ICDT cdt)
        {
            var bdtName = qualifier + "_" + cdt.Name;
            var bdt = bdtLibrary.ElementByName(bdtName);
            if (bdt == null)
            {
                var bdtSpec = BDTSpec.CloneCDT(cdt, bdtName);
                bdt = bdtLibrary.CreateElement(bdtSpec);
            }
            return bdt;
        }

        /// <summary>
        /// Takes the names of the libraries to be created as well as a qualifier as input and creates the
        /// libraries.
        /// </summary>
        public void GenerateLibraries()
        {
            bdtLibrary = bLibrary.CreateBDTLibrary(SpecifyLibraryNamed(bdtLibraryName));
            bieLibrary = bLibrary.CreateBIELibrary(SpecifyLibraryNamed(bieLibraryName));
            docLibrary = bLibrary.CreateDOCLibrary(SpecifyLibraryNamed(docLibraryName));
            GenerateBDTsAndABIEs();
            GenerateRootABIE();
        }

        private void GenerateRootABIE()
        {
            var rootElementMapping = schemaMapping.RootElementMapping;
            if (rootElementMapping is ASBIEMapping)
            {
                ComplexTypeMapping rootComplexTypeMapping = ((ASBIEMapping) rootElementMapping).TargetMapping;
                docLibrary.CreateElement(new ABIESpec
                                         {
                                             Name = qualifier + "_" + rootElementName,
                                             ASBIEs = new List<ASBIESpec>
                                                      {
                                                          new ASBIESpec
                                                          {
                                                              AggregationKind = AggregationKind.Composite,
                                                              Name = rootComplexTypeMapping.BIEName,
                                                              ResolveAssociatedABIE = DeferredABIEResolver(rootComplexTypeMapping),
                                                          }
                                                      },
                                         });
            }
            else if (rootElementMapping is BCCMapping)
            {
                var bccMapping = (BCCMapping) rootElementMapping;
                docLibrary.CreateElement(new ABIESpec
                                         {
                                             BasedOn = bccMapping.ACC,
                                             Name = qualifier + "_" + rootElementName,
                                             BBIEs = GenerateBCCMappings(new List<BCCMapping>{bccMapping}),
                                         });
            }
            else
            {
                throw new MappingError("Root element can only be mapped to BCC, but is mapped to something else.");
            }
        }

        private void GenerateBDTsAndABIEs()
        {
            foreach (var complexTypeMapping in schemaMapping.GetComplexTypeMappings())
            {
                GenerateComplexTypeBIESpecs(complexTypeMapping);
            }
            CreateBIEABIEs(bieABIESpecs);
            CreateDOCABIEs(docABIESpecs);
        }

        private void CreateBIEABIEs(IEnumerable<ABIESpec> abieSpecs)
        {
            // need two passes:
            //  (1) create the ABIEs
            //  (2) create the ASBIEs
            var abies = new Dictionary<string, IABIE>();
            foreach (ABIESpec spec in abieSpecs)
            {
                var specWithoutASBIEs = new ABIESpec
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
                foreach (BBIESpec bbieSpec in spec.BBIEs)
                {
                    specWithoutASBIEs.AddBBIE(bbieSpec);
                }
                abies[spec.Name] = bieLibrary.CreateElement(specWithoutASBIEs);
            }
            foreach (ABIESpec spec in abieSpecs)
            {
                var abie = abies[spec.Name];
                bieLibrary.UpdateElement(abie, spec);
            }
        }

        private void CreateDOCABIEs(IEnumerable<ABIESpec> abieSpecs)
        {
            // need two passes:
            //  (1) create the ABIEs
            //  (2) create the ASBIEs
            var abies = new Dictionary<string, IABIE>();
            foreach (ABIESpec spec in abieSpecs)
            {
                var specWithoutASBIEs = new ABIESpec
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
                foreach (BBIESpec bbieSpec in spec.BBIEs)
                {
                    specWithoutASBIEs.AddBBIE(bbieSpec);
                }
                abies[spec.Name] = docLibrary.CreateElement(specWithoutASBIEs);
            }
            foreach (ABIESpec spec in abieSpecs)
            {
                var abie = abies[spec.Name];
                docLibrary.UpdateElement(abie, spec);
            }
        }

        private void GenerateComplexTypeBIESpecs(ComplexTypeMapping abieMapping)
        {
            if (abieMapping.IsMappedToSingleACC)
            {
                GenerateBIELibraryABIESpec(abieMapping, abieMapping.TargetACCs.ElementAt(0), abieMapping.BIEName);
            }
            else
            {
                var asbieSpecs = new List<ASBIESpec>();
                foreach (IACC acc in abieMapping.TargetACCs)
                {
                    var accABIESpec = GenerateBIELibraryABIESpec(abieMapping, acc, abieMapping.ComplexTypeName + "_" + acc.Name);
                    asbieSpecs.Add(new ASBIESpec
                                   {
                                       Name = acc.Name,
                                       ResolveAssociatedABIE = DeferredABIEResolver(bieLibrary, accABIESpec.Name),
                                   });
                }
                foreach (var asbieMapping in abieMapping.ASBIEMappings)
                {
                    asbieSpecs.Add(new ASBIESpec
                                   {
                                       Name = asbieMapping.BIEName,
                                       ResolveAssociatedABIE = DeferredABIEResolver(asbieMapping.TargetMapping),
                                   });
                }
                docABIESpecs.Add(new ABIESpec
                                 {
                                     Name = abieMapping.ComplexTypeName,
                                     ASBIEs = asbieSpecs,
                                 });
            }
        }

        private ABIESpec GenerateBIELibraryABIESpec(ComplexTypeMapping abieMapping, IACC acc, string name)
        {
            var abieSpec = new ABIESpec
                           {
                               BasedOn = acc,
                               Name = name,
                               BBIEs = GenerateBCCMappings(abieMapping.BCCMappings(acc)),
                               ASBIEs = GenerateASCCMappings(abieMapping.ASCCMappings(acc)),
                           };
            bieABIESpecs.Add(abieSpec);
            return abieSpec;
        }

        private IEnumerable<ASBIESpec> GenerateASCCMappings(IEnumerable<ASCCMapping> asccMappings)
        {
            foreach (var asccMapping in asccMappings)
            {
                var ascc = asccMapping.ASCC;
                var targetMapping = asccMapping.TargetMapping;
                var asbieSpec = ASBIESpec.CloneASCC(ascc, asccMapping.BIEName, DeferredABIEResolver(targetMapping));
                yield return asbieSpec;
            }
        }

        private IEnumerable<BBIESpec> GenerateBCCMappings(IEnumerable<BCCMapping> bccMappings)
        {
            foreach (var bccMapping in bccMappings)
            {
                var bcc = bccMapping.BCC;
                var bbieSpec = BBIESpec.CloneBCC(bcc, GetBDT(bcc.Type));
                bbieSpec.Name = bccMapping.BIEName;
                yield return bbieSpec;
            }
        }

        private static Func<IABIE> DeferredABIEResolver(IBIELibrary bieLibrary, string abieName)
        {
            return () => bieLibrary.ElementByName(abieName);
        }

        private static Func<IABIE> DeferredABIEResolver(IDOCLibrary docLibrary, string abieName)
        {
            return () => docLibrary.ElementByName(abieName);
        }

        private Func<IABIE> DeferredABIEResolver(ComplexTypeMapping complexTypeMapping)
        {
            if (complexTypeMapping.Library == ComplexTypeMapping.LibraryType.BIE)
            {
                return DeferredABIEResolver(bieLibrary, complexTypeMapping.BIEName);
            }
            else
            {
                return DeferredABIEResolver(docLibrary, complexTypeMapping.BIEName);
            }
        }
    }
}