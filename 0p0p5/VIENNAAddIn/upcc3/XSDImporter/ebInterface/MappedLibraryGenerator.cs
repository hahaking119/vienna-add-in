using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    /// <summary>
    /// This Class manages as a last step of the generic XML Schema importer the generation of proper 
    /// BIE and BIE Doc elements in the corresponding UPCC libraries.
    /// </summary>
    public class MappedLibraryGenerator
    {
        private readonly IBLibrary bLibrary;
        private readonly Mappings mappings;

        public MappedLibraryGenerator(IBLibrary bLibrary, Mappings mappings)
        {
            this.bLibrary = bLibrary;
            this.mappings = mappings;
        }

        private static LibrarySpec SpecifyLibraryNamed(string bdtLibraryName)
        {
            return new LibrarySpec
                   {
                       Name = bdtLibraryName
                   };
        }

        /// <summary>
        /// Takes a TargetCCElement representing an ACC as input and returns a set of BBIESpecs according to
        /// the mapped BCCs of that ACC.
        /// </summary>
        /// <param name="bdtLibrary"></param>
        /// <param name="targetACCElement"></param>
        /// <returns></returns>
        private static IEnumerable<BBIESpec> SpecifyBBIEs(IBDTLibrary bdtLibrary, TargetCCElement targetACCElement)
        {
            foreach (var bcc in GetMappedBCCs(targetACCElement))
            {
                yield return BBIESpec.CloneBCC(bcc, GetBDT(bdtLibrary, bcc.Type));
            }
        }

        /// <summary>
        /// Retrieves the BDT based on the given CDT from the BDTLibrary. If the BDT does not yet exist, it is created.
        /// </summary>
        /// <param name="cdt"></param>
        /// <param name="bdtLibrary"></param>
        /// <returns></returns>
        private static IBDT GetBDT(IBDTLibrary bdtLibrary, ICDT cdt)
        {
            var bdtName = cdt.Name;
            var bdt = bdtLibrary.ElementByName(bdtName);
            if (bdt == null)
            {
                var bdtSpec = BDTSpec.CloneCDT(cdt, bdtName);
                bdt = bdtLibrary.CreateElement(bdtSpec);
            }
            return bdt;
        }

        /// <summary>
        /// Returns a set of IBCCs that belong to a given ACC from the list of TargetCCElements.
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        private static IEnumerable<IBCC> GetMappedBCCs(TargetCCElement acc)
        {
            foreach (var child in acc.Children)
            {
                if (child.IsBCC)
                {
                    yield return (IBCC) child.Reference;
                }
            }
        }

        /// <summary>
        /// Returns a set of Mappings which actually involve ASCCs on the TargetCCElement side for a
        /// given ACC.
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        private static IEnumerable<Mapping> GetASCCMappings(TargetCCElement acc)
        {
            foreach (var child in acc.Children)
            {
                if (child.IsASCC)
                {
                    yield return child.Mapping;
                }
            }
        }

        /// <summary>
        /// Takes the names of the libraries to be created as well as a qualifier as input and creates the
        /// libraries.
        /// </summary>
        /// <param name="docLibraryName"></param>
        /// <param name="bieLibraryName"></param>
        /// <param name="bdtLibraryName"></param>
        /// <param name="qualifier"></param>
        public void GenerateLibraries(string docLibraryName, string bieLibraryName, string bdtLibraryName, string qualifier)
        {
            var bdtLibrary = bLibrary.CreateBDTLibrary(SpecifyLibraryNamed(bdtLibraryName));
            var bieLibrary = bLibrary.CreateBIELibrary(SpecifyLibraryNamed(bieLibraryName));
            var docLibrary = bLibrary.CreateDOCLibrary(SpecifyLibraryNamed(docLibraryName));
            GenerateBDTsAndABIEs(bdtLibrary, bieLibrary);
            GenerateDOC(docLibrary, qualifier);
        }

        /// <summary>
        /// Takes a IBDTLibrary and a IBIELibrary as input and generates BDTs and ABIEs properly.
        /// </summary>
        /// <param name="bdtLibrary"></param>
        /// <param name="bieLibrary"></param>
        private void GenerateBDTsAndABIEs(IBDTLibrary bdtLibrary, IBIELibrary bieLibrary)
        {
            foreach (var accMapping in mappings.GetACCMappings())
            {
                var acc = (IACC) accMapping.TargetCC.Reference;
                var abieSpec = new ABIESpec
                               {
                                   BasedOn = acc,
                                   Name = GetABIEName(acc),
                                   BBIEs = SpecifyBBIEs(bdtLibrary, accMapping.TargetCC),
                                   ASBIEs = SpecifyASBIES(accMapping, bdtLibrary, bieLibrary),
                               };
                accMapping.TargetBIE = new TargetBIEElement(abieSpec.Name, bieLibrary.CreateElement(abieSpec));
                // TODO iterate over ASCC-Mappings and add ASBIES as TargetBIE
            }
        }

        /// <summary>
        /// At the moment this method returns the name of the given IACC. The name may however be also constructed from the complex typ
        /// of the source XML Schema document.
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        private static string GetABIEName(IACC acc)
        {
            //TODO this needs to be reconsidered
            return acc.Name;
        }

        /// <summary>
        /// Takes a Mapping representing actually an ACCMapping, an IBDTLibrary amd an IBIELibrary as input and
        /// returns a set of ASBIESpecs. Notice, that with this funtcion we only create ASBIEs that are based on existing
        /// ASCCs.
        /// </summary>
        /// <param name="accMapping"></param>
        /// <param name="bdtLibrary"></param>
        /// <param name="bieLibrary"></param>
        /// <returns></returns>
        private IEnumerable<ASBIESpec> SpecifyASBIES(Mapping accMapping, IBDTLibrary bdtLibrary, IBIELibrary bieLibrary)
        {
            foreach (var asccMapping in GetASCCMappings(accMapping.TargetCC))
            {
                var ascc = ((IASCC) asccMapping.TargetCC.Reference);

                var associatedABIE = GetABIE(ascc.AssociatedElement, bdtLibrary, asccMapping.TargetCC, bieLibrary);
                var asbieSpec = ASBIESpec.CloneASCC(ascc, ascc.Name, associatedABIE.Id);
                yield return asbieSpec;
            }
        }

        /// <summary>
        /// Returns an IABIE based on its IACC in the given IBIELibrary.
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="bdtLibrary"></param>
        /// <param name="targetCCElement"></param>
        /// <param name="bieLibrary"></param>
        /// <returns></returns>
        private static IABIE GetABIE(IACC acc, IBDTLibrary bdtLibrary, TargetCCElement targetCCElement, IBIELibrary bieLibrary)
        {
            var abieName = GetABIEName(acc);
            IABIE abie = bieLibrary.ElementByName(abieName);
            if (abie == null)
            {
                var abieSpec = new ABIESpec
                               {
                                   BasedOn = acc,
                                   Name = abieName,
                                   BBIEs = SpecifyBBIEs(bdtLibrary, targetCCElement),
                               };
                abie = bieLibrary.CreateElement(abieSpec);
            }
            return abie;
        }

        /// <summary>
        /// This method creates the root ABIE in the DocLibrary representing the document which has been mapped.
        /// </summary>
        /// <param name="docLibrary"></param>
        /// <param name="qualifier"></param>
        private void GenerateDOC(IDOCLibrary docLibrary, string qualifier)
        {
            docLibrary.CreateElement(new ABIESpec
                                     {
                                         Name = mappings.RootSourceElement.Name,
                                         ASBIEs = GenerateASBIEsToChildren(mappings.RootSourceElement, qualifier, docLibrary),
                                     });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceElement"></param>
        /// <param name="qualifier"></param>
        /// <param name="docLibrary"></param>
        /// <returns></returns>
        private IABIE GenerateDOCABIE(SourceElement sourceElement, string qualifier, IDOCLibrary docLibrary)
        {
            var asbies = new List<ASBIESpec>
                         {
                             GenerateASBIEToTarget(sourceElement, "self")
                         };
            asbies.AddRange(GenerateASBIEsToChildren(sourceElement, qualifier, docLibrary));
            return docLibrary.CreateElement(new ABIESpec
                                            {
                                                Name = qualifier + "_" + sourceElement.Name,
                                                ASBIEs = asbies,
                                            });
        }

        /// <summary>
        /// Takes a SourceElement representing a Target as input and returns an ASBIESpec.
        /// </summary>
        /// <param name="sourceElement"></param>
        /// <param name="asbieName"></param>
        /// <returns></returns>
        private static ASBIESpec GenerateASBIEToTarget(SourceElement sourceElement, string asbieName)
        {
            var abie = sourceElement.Mapping.TargetBIE.Reference as IABIE;
            if (abie == null)
            {
                // TODO error
                throw new Exception("Source element mapped to ABIE, but reference cannot be resolved to an ABIE: " + sourceElement.Name);
            }
            return new ASBIESpec
                   {
                       AggregationKind = EAAggregationKind.Composite,
                       AssociatedABIEId = abie.Id,
                       Name = asbieName,
                   };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceElement"></param>
        /// <param name="qualifier"></param>
        /// <param name="docLibrary"></param>
        /// <returns></returns>
        private IEnumerable<ASBIESpec> GenerateASBIEsToChildren(SourceElement sourceElement, string qualifier, IDOCLibrary docLibrary)
        {
            foreach (var childElement in sourceElement.Children)
            {
                if (NeedsDOCABIE(childElement))
                {
                    var childABIE = GenerateDOCABIE(childElement, qualifier, docLibrary);
                    yield return new ASBIESpec
                               {
                                   AggregationKind = EAAggregationKind.Composite,
                                   AssociatedABIEId = childABIE.Id,
                                   Name = childElement.Name,
                               };
                }
                else
                {
                    if (IsMappedToABIE(childElement))
                    {
                        yield return GenerateASBIEToTarget(childElement, childElement.Name);
                    }
                }
            }
        }

        /// <summary>
        /// If a source element is mapped to an ABIE and at least one of its children is
        /// also mapped to an ABIE, we must create an ABIE in the DOCLibrary.
        /// </summary>
        /// <param name="sourceElement"></param>
        /// <returns></returns>
        private static bool NeedsDOCABIE(SourceElement sourceElement)
        {
            if (IsMappedToABIE(sourceElement))
            {
                foreach (var child in sourceElement.Children)
                {
                    if (IsMappedToABIE(child))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceElement"></param>
        /// <returns></returns>
        private static bool IsMappedToABIE(SourceElement sourceElement)
        {
            return sourceElement != null && 
                   sourceElement.Mapping != null &&
                   sourceElement.Mapping.TargetBIE != null &&
                   sourceElement.Mapping.TargetBIE.IsABIE;
        }
    }
}