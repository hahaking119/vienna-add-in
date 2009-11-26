// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.bLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUtils;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    /// <summary>
    /// A CCRepository that directly accesses (DRA = direct repository access) the underlying EA.Repository for each request
    /// (i.e. it is simply an interface layer on top of the EA.Repository, without any caching).
    /// </summary>
    public class CCRepository : ICCRepository
    {
        private readonly Repository eaRepository;

        public CCRepository(Repository eaRepository)
        {
            this.eaRepository = eaRepository;
        }

        public Repository EARepository
        {
            get { return eaRepository; }
        }

        private IEnumerable<Package> ContainedPackages
        {
            get
            {
                foreach (Package rootLocationPackage in GetRootLocationPackages())
                {
                    foreach (Package package in rootLocationPackage.EnumerateContainedPackages())
                    {
                        yield return package;
                    }
                }
            }
        }

        #region ICCRepository Members

        public IEnumerable<IBLibrary> GetBLibraries()
        {
            return from p in ContainedPackages
                   where p.IsBLibrary()
                   select (IBLibrary) new BLibrary(this, p);
        }

        public IEnumerable<IPRIMLibrary> GetPrimLibraries()
        {
            return from p in ContainedPackages
                   where p.IsPRIMLibrary()
                   select (IPRIMLibrary) new PRIMLibrary(this, p);
        }

        public IEnumerable<IENUMLibrary> GetEnumLibraries()
        {
            return from p in ContainedPackages
                   where p.IsENUMLibrary()
                   select (IENUMLibrary) new ENUMLibrary(this, p);
        }

        public IEnumerable<ICDTLibrary> GetCdtLibraries()
        {
            return from p in ContainedPackages
                   where p.IsCDTLibrary()
                   select (ICDTLibrary) new CDTLibrary(this, p);
        }

        public IEnumerable<ICCLibrary> GetCcLibraries()
        {
            return from p in ContainedPackages
                   where p.IsCCLibrary()
                   select (ICCLibrary) new CCLibrary(this, p);
        }

        public IEnumerable<IBdtLibrary> GetBdtLibraries()
        {
            return from p in ContainedPackages
                   where p.IsBDTLibrary()
                   select (IBdtLibrary) new BDTLibrary(this, p);
        }

        public IEnumerable<IBieLibrary> GetBieLibraries()
        {
            return from p in ContainedPackages
                   where p.IsBIELibrary()
                   select (IBieLibrary) new BIELibrary(this, p);
        }

        public IEnumerable<IDOCLibrary> GetDocLibraries()
        {
            return from p in ContainedPackages
                   where p.IsDOCLibrary()
                   select (IDOCLibrary) new DOCLibrary(this, p);
        }

        public IBLibrary GetBLibraryById(int id)
        {
            Package package = eaRepository.GetPackageByID(id);
            return package == null ? null : new BLibrary(this, package);
        }

        public IPRIMLibrary GetPrimLibraryById(int id)
        {
            Package package = eaRepository.GetPackageByID(id);
            return package == null ? null : new PRIMLibrary(this, package);
        }

        public IENUMLibrary GetEnumLibraryById(int id)
        {
            Package package = eaRepository.GetPackageByID(id);
            return package == null ? null : new ENUMLibrary(this, package);
        }

        public ICDTLibrary GetCdtLibraryById(int id)
        {
            Package package = eaRepository.GetPackageByID(id);
            return package == null ? null : new CDTLibrary(this, package);
        }

        public ICCLibrary GetCcLibraryById(int id)
        {
            Package package = eaRepository.GetPackageByID(id);
            return package == null ? null : new CCLibrary(this, package);
        }

        public IBdtLibrary GetBdtLibraryById(int id)
        {
            Package package = eaRepository.GetPackageByID(id);
            return package == null ? null : new BDTLibrary(this, package);
        }

        public IBieLibrary GetBieLibraryById(int id)
        {
            Package package = eaRepository.GetPackageByID(id);
            return package == null ? null : new BIELibrary(this, package);
        }

        public IDOCLibrary GetDocLibraryById(int id)
        {
            Package package = eaRepository.GetPackageByID(id);
            return package == null ? null : new DOCLibrary(this, package);
        }

        public IPRIM GetPrimById(int id)
        {
            Element element = eaRepository.GetElementByID(id);
            return element == null ? null : new PRIM(this, element);
        }

        public IENUM GetEnumById(int id)
        {
            Element element = eaRepository.GetElementByID(id);
            return element == null ? null : new ENUM(this, element);
        }

        public ICDT GetCdtById(int id)
        {
            Element element = eaRepository.GetElementByID(id);
            return element == null ? null : new CDT(this, element);
        }

        public IACC GetAccById(int id)
        {
            Element element = eaRepository.GetElementByID(id);
            return element == null ? null : new ACC(this, element);
        }

        public IBdt GetBdtById(int id)
        {
            Element element = eaRepository.GetElementByID(id);
            return element == null ? null : new BDT(this, element);
        }

        public IAbie GetAbieById(int id)
        {
            Element element = eaRepository.GetElementByID(id);
            return element == null ? null : new ABIE(this, element);
        }

        public IBLibrary GetBLibraryByPath(Path path)
        {
            var package = eaRepository.Resolve<Package>(path);
            return package == null ? null : new BLibrary(this, package);
        }

        public IPRIMLibrary GetPrimLibraryByPath(Path path)
        {
            var package = eaRepository.Resolve<Package>(path);
            return package == null ? null : new PRIMLibrary(this, package);
        }

        public IENUMLibrary GetEnumLibraryByPath(Path path)
        {
            var package = eaRepository.Resolve<Package>(path);
            return package == null ? null : new ENUMLibrary(this, package);
        }

        public ICDTLibrary GetCdtLibraryByPath(Path path)
        {
            var package = eaRepository.Resolve<Package>(path);
            return package == null ? null : new CDTLibrary(this, package);
        }

        public ICCLibrary GetCcLibraryByPath(Path path)
        {
            var package = eaRepository.Resolve<Package>(path);
            return package == null ? null : new CCLibrary(this, package);
        }

        public IBdtLibrary GetBdtLibraryByPath(Path path)
        {
            var package = eaRepository.Resolve<Package>(path);
            return package == null ? null : new BDTLibrary(this, package);
        }

        public IBieLibrary GetBieLibraryByPath(Path path)
        {
            var package = eaRepository.Resolve<Package>(path);
            return package == null ? null : new BIELibrary(this, package);
        }

        public IDOCLibrary GetDocLibraryByPath(Path path)
        {
            var package = eaRepository.Resolve<Package>(path);
            return package == null ? null : new DOCLibrary(this, package);
        }

        public IPRIM GetPrimByPath(Path path)
        {
            var element = eaRepository.Resolve<Element>(path);
            return element == null ? null : new PRIM(this, element);
        }

        public IENUM GetEnumByPath(Path path)
        {
            var element = eaRepository.Resolve<Element>(path);
            return element == null ? null : new ENUM(this, element);
        }

        public ICDT GetCdtByPath(Path path)
        {
            var element = eaRepository.Resolve<Element>(path);
            return element == null ? null : new CDT(this, element);
        }

        public IACC GetAccByPath(Path path)
        {
            var element = eaRepository.Resolve<Element>(path);
            return element == null ? null : new ACC(this, element);
        }

        public IBdt GetBdtByPath(Path path)
        {
            var element = eaRepository.Resolve<Element>(path);
            return element == null ? null : new BDT(this, element);
        }

        public IAbie GetAbieByPath(Path path)
        {
            var element = eaRepository.Resolve<Element>(path);
            return element == null ? null : new ABIE(this, element);
        }

        /// <summary>
        /// Candidate root locations for EA-based UPCC models are models and top-level packages with stereotype "BInformationV".
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Path> GetRootLocations()
        {
            return GetRootLocationPackages().Convert(p => p.GetPath(eaRepository));
        }

        public IBLibrary CreateRootBLibrary(Path rootLocation, BLibrarySpec spec)
        {
            var rootPackage = eaRepository.Resolve<Package>(rootLocation);
            if (rootPackage == null)
            {
                throw new ArgumentException("Invalid root location: " + rootLocation);
            }

            var libraryPackage = (Package) rootPackage.Packages.AddNew(spec.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = rootPackage.PackageID;
            libraryPackage.Element.Stereotype = Stereotype.bLibrary;
            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, spec.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, spec.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, spec.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, spec.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, spec.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, spec.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, spec.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, spec.VersionIdentifier);
            libraryPackage.Update();

            var packageDiagram = (Diagram) libraryPackage.Diagrams.AddNew(spec.Name, "Package");
            packageDiagram.Update();

            libraryPackage.Diagrams.Refresh();
            rootPackage.Packages.Refresh();

            return new BLibrary(this, libraryPackage);
        }

        #endregion

        private IEnumerable<Package> GetRootLocationPackages()
        {
            foreach (Package model in eaRepository.Models)
            {
                yield return model;
                foreach (Package rootPackage in model.Packages)
                {
                    if (rootPackage.IsA(Stereotype.BInformationV))
                    {
                        yield return rootPackage;
                    }
                }
            }
        }

        public IBasicType GetBasicTypeById(int id)
        {
            Element element = eaRepository.GetElementByID(id);
            if (element == null)
            {
                return null;
            }
            switch (element.Stereotype)
            {
                case "PRIM":
                    return new PRIM(this, element);
                case "ENUM":
                    return new ENUM(this, element);
                default:
                    return null;
            }
        }
    }
}