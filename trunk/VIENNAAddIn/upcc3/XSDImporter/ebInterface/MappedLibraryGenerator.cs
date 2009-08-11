using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
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

        public void GenerateLibraries(string docLibraryName, string bieLibraryName, string bdtLibraryName)
        {
            var bdtLibrary = bLibrary.CreateBDTLibrary(SpecifyLibraryNamed(bdtLibraryName));
            var bieLibrary = bLibrary.CreateBIELibrary(SpecifyLibraryNamed(bieLibraryName));
            var docLibrary = bLibrary.CreateDOCLibrary(SpecifyLibraryNamed(docLibraryName));
            GenerateBDTsAndABIEs(bdtLibrary, bieLibrary);
            GenerateDOC(docLibrary);
        }

        private void GenerateBDTsAndABIEs(IBDTLibrary bdtLibrary, IBIELibrary bieLibrary)
        {
            foreach (var accMapping in mappings.GetACCMappings())
            {
                var acc = (IACC)accMapping.TargetCC.Reference;
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

        private static string GetABIEName(IACC acc)
        {
            return acc.Name;
        }

        private IEnumerable<ASBIESpec> SpecifyASBIES(Mapping accMapping, IBDTLibrary bdtLibrary, IBIELibrary bieLibrary)
        {
            foreach (var asccMapping in GetASCCMappings(accMapping.TargetCC))
            {
                var ascc = ((IASCC)asccMapping.TargetCC.Reference);

                var associatedABIE = GetABIE(ascc.AssociatedElement, bdtLibrary, asccMapping.TargetCC, bieLibrary);
                var asbieSpec = ASBIESpec.CloneASCC(ascc, ascc.Name, associatedABIE.Id);
                yield return asbieSpec;
            }
        }

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

        private void GenerateDOC(IDOCLibrary docLibrary)
        {
            var asbies = new List<ASBIESpec>();
            foreach (var childElement in mappings.RootSourceElement.Children)
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
                               Name = mappings.RootSourceElement.Name,
                               ASBIEs = asbies,
                           };
            docLibrary.CreateElement(abieSpec);
        }
    }
}