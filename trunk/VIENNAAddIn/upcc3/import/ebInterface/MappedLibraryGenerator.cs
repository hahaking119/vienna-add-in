using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using VIENNAAddIn.Utils;

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
        private readonly Dictionary<string, List<AsbieToGenerate>> asbiesToGenerate = new Dictionary<string, List<AsbieToGenerate>>();
        private readonly List<MaSpec> maSpecs = new List<MaSpec>();
        private readonly Dictionary<string, List<AsmaToGenerate>> asmasToGenerate = new Dictionary<string, List<AsmaToGenerate>>();
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
            bdtLibrary = bLibrary.CreateBdtLibrary(new BdtLibrarySpec
                                                   {
                                                       Name = bdtLibraryName
                                                   });
            bieLibrary = bLibrary.CreateBieLibrary(new BieLibrarySpec
                                                   {
                                                       Name = bieLibraryName
                                                   });
            docLibrary = bLibrary.CreateDocLibrary(new DocLibrarySpec
                                                   {
                                                       Name = docLibraryName
                                                   });
            GenerateBDTsAndABIEs();
            GenerateRootABIE();
        }

        private void GenerateRootABIE()
        {
            var rootElementMapping = schemaMapping.RootElementMapping;
            if (rootElementMapping is ASBIEMapping)
            {
                ComplexTypeMapping rootComplexTypeMapping = ((ASBIEMapping) rootElementMapping).TargetMapping;
                var ma = docLibrary.CreateMa(new MaSpec
                                             {
                                                 Name = qualifier + "_" + rootElementName,
                                             });
                ma.CreateAsma(new AsmaSpec
                              {
                                  Name = rootComplexTypeMapping.BIEName,
                                  AssociatedBieAggregator = new BieAggregator(bieLibrary.GetAbieByName(rootComplexTypeMapping.BIEName)),
                              });
            }
            else if (rootElementMapping is BCCMapping)
            {
                var bccMapping = (BCCMapping) rootElementMapping;
                var abie = bieLibrary.CreateAbie(new AbieSpec
                                         {
                                             BasedOn = bccMapping.ACC,
                                             Name = bccMapping.ACC.Name,
                                             Bbies = new List<BbieSpec>(GenerateBCCMappings(new List<BCCMapping>{bccMapping})),
                                         });
                var ma = docLibrary.CreateMa(new MaSpec
                                         {
                                             Name = qualifier + "_" + rootElementName,
                                         });
                ma.CreateAsma(new AsmaSpec
                {
                    Name = abie.Name,
                    AssociatedBieAggregator = new BieAggregator(abie),
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
            CreateAbies();
            CreateMas();
        }

        private void CreateAbies()
        {
            var abies = new List<IAbie>();
            foreach (var abieSpec in abieSpecs)
            {
                abies.Add(bieLibrary.CreateAbie(abieSpec));
            }
            foreach (var abie in abies)
            {
                foreach (var asbieSpec in GenerateAsbieSpecsForAbie(abie))
                {
                    abie.CreateAsbie(asbieSpec);
                }
            }
        }

        private List<AsbieSpec> GenerateAsbieSpecsForAbie(IAbie abie)
        {
            return asbiesToGenerate.GetAndCreate(abie.Name).ConvertAll(asbieToGenerate => asbieToGenerate.GenerateSpec());
        }

        private List<AsmaSpec> GenerateAsmaSpecsForMa(IMa ma)
        {
            return asmasToGenerate.GetAndCreate(ma.Name).ConvertAll(asmaToGenerate => asmaToGenerate.GenerateSpec());
        }

        private void CreateMas()
        {
            var mas = new List<IMa>();
            foreach (var maSpec in maSpecs)
            {
                mas.Add(docLibrary.CreateMa(maSpec));
            }
            foreach (var ma in mas)
            {
                foreach (var asmaSpec in GenerateAsmaSpecsForMa(ma))
                {
                    ma.CreateAsma(asmaSpec);
                }
            }
        }

        private void GenerateComplexTypeBIESpecs(ComplexTypeMapping abieMapping)
        {
            if (abieMapping.IsMappedToSingleACC)
            {
                GenerateABIESpec(abieMapping, abieMapping.TargetACCs.ElementAt(0), abieMapping.BIEName);
            }
            else
            {
                var maName = abieMapping.ComplexTypeName;
                maSpecs.Add(new MaSpec
                            {
                                Name = maName,
                            });
                var asmasToGenerateForThisMa = asmasToGenerate.GetAndCreate(maName);
                foreach (IAcc acc in abieMapping.TargetACCs)
                {
                    var accABIESpec = GenerateABIESpec(abieMapping, acc, abieMapping.ComplexTypeName + "_" + acc.Name);
                    asmasToGenerateForThisMa.Add(new AsmaToGenerate(bieLibrary, acc.Name, accABIESpec.Name));
                }
                foreach (var asbieMapping in abieMapping.ASBIEMappings)
                {
                    asmasToGenerateForThisMa.Add(new AsmaToGenerate(docLibrary, asbieMapping.BIEName, asbieMapping.TargetMapping.BIEName));
                }
            }
        }

        private AbieSpec GenerateABIESpec(ComplexTypeMapping abieMapping, IAcc acc, string name)
        {
            var abieSpec = new AbieSpec
                           {
                               BasedOn = acc,
                               Name = name,
                               Bbies = new List<BbieSpec>(GenerateBCCMappings(abieMapping.BCCMappings(acc))),
                           };
            abieSpecs.Add(abieSpec);
            asbiesToGenerate.GetAndCreate(name).AddRange(DetermineAsbiesToGenerate(abieMapping.ASCCMappings(acc)));
            return abieSpec;
        }

        private IEnumerable<AsbieToGenerate> DetermineAsbiesToGenerate(IEnumerable<ASCCMapping> asccMappings)
        {
            foreach (var asccMapping in asccMappings)
            {
                yield return new AsbieToGenerate(bieLibrary, asccMapping.ASCC, asccMapping.BIEName, asccMapping.TargetMapping.BIEName);
            }
        }

        private IEnumerable<BbieSpec> GenerateBCCMappings(IEnumerable<BCCMapping> bccMappings)
        {
            foreach (var bccMapping in bccMappings)
            {
                var bcc = bccMapping.BCC;
                var bbieSpec = BbieSpec.CloneBcc(bcc, GetBDT(bcc.Cdt));
                bbieSpec.Name = bccMapping.BIEName;
                yield return bbieSpec;
            }
        }
    }

    internal class AsmaToGenerate
    {
        private readonly string name;
        private readonly string associatedBieName;
        private readonly IBieLibrary bieLibrary;
        private readonly IDocLibrary docLibrary;

        public AsmaToGenerate(IBieLibrary bieLibrary, string name, string associatedAbieName)
        {
            this.bieLibrary = bieLibrary;
            this.name = name;
            this.associatedBieName = associatedAbieName;
        }

        public AsmaToGenerate(IDocLibrary docLibrary, string name, string associatedMaName)
        {
            this.docLibrary = docLibrary;
            this.name = name;
            this.associatedBieName = associatedMaName;
        }

        internal AsmaSpec GenerateSpec()
        {
            return new AsmaSpec
                   {
                       Name = name,
                       AssociatedBieAggregator = new BieAggregator(bieLibrary != null ? (object) bieLibrary.GetAbieByName(associatedBieName) : docLibrary.GetMaByName(associatedBieName)),
                   };
        }
    }

    internal class AsbieToGenerate
    {
        private readonly IBieLibrary bieLibrary;
        private readonly IAscc ascc;
        private readonly string asbieName;
        private readonly string associatedAbieName;

        public AsbieToGenerate(IBieLibrary bieLibrary, IAscc ascc, string asbieName, string associatedAbieName)
        {
            this.bieLibrary = bieLibrary;
            this.ascc = ascc;
            this.asbieName = asbieName;
            this.associatedAbieName = associatedAbieName;
        }

        internal AsbieSpec GenerateSpec()
        {
            return AsbieSpec.CloneAscc(ascc, asbieName, bieLibrary.GetAbieByName(associatedAbieName));
        }
    }

    internal class AsmaSpecWithAssociatedMaName : AsmaSpec
    {
        internal string AssociatedMaName { get; set; }
    }

    internal class AsmaSpecWithAssociatedAbieName : AsmaSpec
    {
        internal string AssociatedAbieName { get; set; }
    }
}