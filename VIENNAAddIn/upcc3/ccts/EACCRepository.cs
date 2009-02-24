using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.constants;
using VIENNAAddIn.upcc3.ccts.util;
using Attribute=EA.Attribute;
using TaggedValues=VIENNAAddIn.upcc3.ccts.util.TaggedValues;
using VIENNAAddIn.upcc3.XSDGenerator.Generator;

namespace VIENNAAddIn.upcc3.ccts
{
    /// <summary>
    /// A CCTSRepository backed by a UPCC 3.0 compliant Enterprise Architect (EA) repository.
    /// </summary>
    public class EACCRepository : ICCRepository
    {
        private readonly Repository eaRepository;
        private readonly List<IBusinessLibrary> libraries = new List<IBusinessLibrary>();

        public EACCRepository(Repository eaRepository)
        {
            this.eaRepository = eaRepository;
        }

        #region ICCTSRepository Members

        public IBusinessLibrary GetCDTLibrary(int id)
        {
            throw new NotImplementedException();
        }

        public IBusinessLibrary GetRootLibrary()
        {
            throw new System.NotImplementedException();
        }

        public IBusinessLibrary GetLibrary(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IBusinessLibrary> Libraries
        {
            get { throw new System.NotImplementedException(); }
        }

        public ICDT GetCDT(int id)
        {
            throw new System.NotImplementedException();
        }

        public void EachCDT(int cdtLibraryId, Action<ICDT> visit)
        {
            IList<ICDT> cdts = FetchCDTs(cdtLibraryId);
            foreach (ICDT cdt in cdts)
            {
                visit(cdt);
            }
        }

        public void TraverseLibrariesDepthFirst(Action<IBusinessLibrary> visit)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Retrieves all elements of the given library that are valid CDTs.
        /// 
        /// Reports warnings if the package itself is not a CDT library and/or some of the package elements are not valid CDTs.
        /// </summary>
        /// <param name="cdtLibraryId"></param>
        /// <returns></returns>
        private IList<ICDT> FetchCDTs(int cdtLibraryId)
        {
            Package package = eaRepository.GetPackageByID(cdtLibraryId);
            // TODO verify that package is a CDTLibrary
            var cdts = new List<ICDT>();
            foreach (Element e in package.Elements)
            {
                if (e.Stereotype.Equals(CCTS_Types.CDT.ToString()))
                {
                    var cdt = new CDT(e.ElementID, e.Name)
                                  {
                                      BusinessTerms = e.GetTaggedValues(TaggedValues.BusinessTerm),
                                      Definition = e.GetTaggedValue(TaggedValues.Definition),
                                      DictionaryEntryName = e.GetTaggedValue(TaggedValues.DictionaryEntryName),
                                      LanguageCode = e.GetTaggedValue(TaggedValues.LanguageCode),
                                      UniqueIdentifier = e.GetTaggedValue(TaggedValues.UniqueIdentifier),
                                      UsageRules = e.GetTaggedValues(TaggedValues.UsageRule),
                                      VersionIdentifier = e.GetTaggedValue(TaggedValues.VersionIdentifier)
                                  };
                    foreach (Attribute attribute in e.Attributes)
                    {
                        switch (attribute.Stereotype)
                        {
                            case "SUP":
                                // TODO validate SUP
                                cdt.AddSupplementaryComponent(CreateComponent(attribute, DTComponentType.SUP));
                                break;
                            case "CON":
                                cdt.CON = CreateComponent(attribute, DTComponentType.CON);
                                break;
                            default:
                                // TODO generate warning
                                break;
                        }
                    }
                    // TODO validate CDT
                    cdts.Add(cdt);
                }
                // TODO else report error, but continue loading valid CDTs
            }
            return cdts;
        }

        private static DTComponent CreateComponent(Attribute attribute, DTComponentType componentType)
        {
            return new DTComponent(componentType, attribute.AttributeID, attribute.Name,
                                         attribute.Type)
                       {
                           BusinessTerms = attribute.GetTaggedValues(TaggedValues.BusinessTerm),
                           Definition = attribute.GetTaggedValue(TaggedValues.Definition),
                           DictionaryEntryName = attribute.GetTaggedValue(TaggedValues.DictionaryEntryName),
                           LanguageCode = attribute.GetTaggedValue(TaggedValues.LanguageCode),
                           UniqueIdentifier = attribute.GetTaggedValue(TaggedValues.UniqueIdentifier),
                           UsageRules = attribute.GetTaggedValues(TaggedValues.UsageRule),
                           VersionIdentifier = attribute.GetTaggedValue(TaggedValues.VersionIdentifier),
                           ModificationAllowedIndicator = ("true" == attribute.GetTaggedValue(TaggedValues.ModificationAllowedIndicator).DefaultTo("true")),
                           UpperBound = attribute.UpperBound,
                           LowerBound = attribute.LowerBound,
                       };
        }

        public void EachLibrary(Action<IBusinessLibrary> visit)
        {
            fetchLibraries();
            foreach (IBusinessLibrary library in libraries)
            {
                visit(library);
            }
        }

        private void fetchLibraries()
        {
            libraries.Clear();
            if (eaRepository.Models.Count != 1)
            {
                // TODO throw exception
            }
            var model = (Package) eaRepository.Models.GetAt(0);
            foreach (Package package in model.Packages)
            {
                fetchRecursively(package, String.Empty);
            }
        }

        private void fetchRecursively(Package package, string parentBaseUrn)
        {
            string stereotype = package.Element.Stereotype;
            BusinessLibraryType libraryType;
            switch (stereotype)
            {
                case "bLibrary":
                    // TODO bLibrary checks
                    libraryType = BusinessLibraryType.bLibrary;
                    foreach (Package child in package.Packages)
                    {
                        fetchRecursively(child, parentBaseUrn + package.GetTaggedValue(TaggedValues.BaseURN));
                    }
                    break;
                case "BDTLibrary":
                    // TODO BDTLibrary checks
                    libraryType = BusinessLibraryType.BDTLibrary;
                    break;
                case "BIELibrary":
                    // TODO BIELibrary checks
                    libraryType = BusinessLibraryType.BIELibrary;
                    break;
                case "CCLibrary":
                    // TODO CCLibrary checks
                    libraryType = BusinessLibraryType.CCLibrary;
                    break;
                case "CDTLibrary":
                    // TODO CDTLibrary checks
                    libraryType = BusinessLibraryType.CDTLibrary;
                    break;
                case "DOCLibrary":
                    // TODO DOCLibrary checks
                    libraryType = BusinessLibraryType.DOCLibrary;
                    break;
                case "ENUMLibrary":
                    // TODO ENUMLibrary checks
                    libraryType = BusinessLibraryType.ENUMLibrary;
                    break;
                case "PRIMLibrary":
                    // TODO PRIMLibrary checks
                    libraryType = BusinessLibraryType.PRIMLibrary;
                    break;
                default:
                    // TODO generate error message for user
                    Console.WriteLine("invalid package stereotype: " + stereotype + " for package " + package.Name);
                    return;
            }
            var lib = new BusinessLibrary(libraryType, package.PackageID, package.Name)
                          {
                              BusinessTerms = package.GetTaggedValues(TaggedValues.BusinessTerm),
                              Copyrights = package.GetTaggedValues(TaggedValues.Copyright),
                              Owners = package.GetTaggedValues(TaggedValues.Owner),
                              References = package.GetTaggedValues(TaggedValues.Reference),
                              Status = package.GetTaggedValue(TaggedValues.Status),
                              UniqueIdentifier = package.GetTaggedValue(TaggedValues.UniqueIdentifier),
                              VersionIdentifier = package.GetTaggedValue(TaggedValues.VersionIdentifier),
                              BaseURN = parentBaseUrn + package.GetTaggedValue(TaggedValues.BaseURN)
                          };
            libraries.Add(lib);
        }
    }
}