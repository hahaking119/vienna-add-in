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
                foreach (Package rootBLibraryPackage in AllRootBLibraryPackages())
                {
                    foreach (Package package in rootBLibraryPackage.EnumeratePackages())
                    {
                        yield return package;
                    }
                }
            }
        }

        #region ICCRepository Members

        public object FindByPath(Path path)
        {
            var o = eaRepository.Resolve<object>(path);
            if (o == null)
            {
                return null;
            }
            if (o is Element)
            {
                return GetElement((Element) o);
            }
            if (o is Package)
            {
                return GetLibrary((Package) o);
            }
            throw new Exception("path resolved to an object that's neither an element nor a package");
        }

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

        public IEnumerable<IBDTLibrary> GetBdtLibraries()
        {
            return from p in ContainedPackages
                   where p.IsBDTLibrary()
                   select (IBDTLibrary) new BDTLibrary(this, p);
        }

        public IEnumerable<IBIELibrary> GetBieLibraries()
        {
            return from p in ContainedPackages
                   where p.IsBIELibrary()
                   select (IBIELibrary) new BIELibrary(this, p);
        }

        public IEnumerable<IDOCLibrary> GetDocLibraries()
        {
            return from p in ContainedPackages
                   where p.IsDOCLibrary()
                   select (IDOCLibrary) new DOCLibrary(this, p);
        }

        public IBLibrary GetBLibraryById(int id)
        {
            return GetLibraryById(id) as IBLibrary;
        }

        public IPRIMLibrary GetPrimLibraryById(int id)
        {
            return GetLibraryById(id) as IPRIMLibrary;
        }

        public IENUMLibrary GetEnumLibraryById(int id)
        {
            return GetLibraryById(id) as IENUMLibrary;
        }

        public ICDTLibrary GetCdtLibraryById(int id)
        {
            return GetLibraryById(id) as ICDTLibrary;
        }

        public ICCLibrary GetCcLibraryById(int id)
        {
            return GetLibraryById(id) as ICCLibrary;
        }

        public IBDTLibrary GetBdtLibraryById(int id)
        {
            return GetLibraryById(id) as IBDTLibrary;
        }

        public IBIELibrary GetBieLibraryById(int id)
        {
            return GetLibraryById(id) as IBIELibrary;
        }

        public IDOCLibrary GetDocLibraryById(int id)
        {
            return GetLibraryById(id) as IDOCLibrary;
        }

        public ICDT GetCdtById(int id)
        {
            return new CDT(this, eaRepository.GetElementByID(id));
        }

        public IACC GetAccById(int id)
        {
            return new ACC(this, eaRepository.GetElementByID(id));
        }

        public IBDT GetBdtById(int id)
        {
            return new BDT(this, eaRepository.GetElementByID(id));
        }

        public IABIE GetAbieById(int id)
        {
            return new ABIE(this, eaRepository.GetElementByID(id));
        }

        #endregion

        public IDOCLibrary GetDocLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        private object GetLibraryById(int id)
        {
            return id == 0 ? null : GetLibrary(eaRepository.GetPackageByID(id));
        }

        private IEnumerable<Package> AllRootBLibraryPackages()
        {
            foreach (Package model in eaRepository.Models)
            {
                foreach (Package rootPackage in model.Packages)
                {
                    if (rootPackage.IsA(Stereotype.BInformationV))
                    {
                        foreach (Package subPackage in rootPackage.Packages)
                        {
                            if (subPackage.IsA(Stereotype.bLibrary))
                            {
                                yield return subPackage;
                            }
                        }
                    }
                    else
                    {
                        if (rootPackage.IsA(Stereotype.bLibrary))
                        {
                            yield return rootPackage;
                        }
                    }
                }
            }
        }

        public object GetLibrary(Package package)
        {
            if (package == null)
            {
                return null;
            }
            if (package.Element == null)
            {
                // this is a model, which cannot be a business library
                return null;
            }
            switch (package.Element.Stereotype)
            {
                case "bLibrary":
                    return new BLibrary(this, package);
                case "BDTLibrary":
                    return new BDTLibrary(this, package);
                case "BIELibrary":
                    return new BIELibrary(this, package);
                case "CCLibrary":
                    return new CCLibrary(this, package);
                case "CDTLibrary":
                    return new CDTLibrary(this, package);
                case "DOCLibrary":
                    return new DOCLibrary(this, package);
                case "ENUMLibrary":
                    return new ENUMLibrary(this, package);
                case "PRIMLibrary":
                    return new PRIMLibrary(this, package);
                default:
                    throw new ArgumentException("Invalid package stereotype: " + package.Element.Stereotype);
            }
        }

        private object GetElement(Element element)
        {
            if (element == null)
            {
                return null;
            }
            switch (element.Stereotype)
            {
                case "BDT":
                    return new BDT(this, element);
                case "ABIE":
                    return new ABIE(this, element);
                case "ACC":
                    return new ACC(this, element);
                case "CDT":
                    return new CDT(this, element);
                case "ENUM":
                    return new ENUM(this, element);
                case "PRIM":
                    return new PRIM(this, element);
                default:
                    throw new ArgumentException("Invalid element stereotype: " + element.Stereotype);
            }
        }

        public IBasicType GetIType(int id)
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

        public IENUM GetENUM(int id)
        {
            return new ENUM(this, eaRepository.GetElementByID(id));
        }

        public IPRIM GetPRIM(int id)
        {
            return new PRIM(this, eaRepository.GetElementByID(id));
        }

        ///<summary>
        /// Create a new <<bLibrary>> package in the given parent package.
        ///</summary>
        ///<param name="spec"></param>
        ///<param name="parentPackage"></param>
        ///<returns></returns>
        public IBLibrary CreateBLibrary(BLibrarySpec spec, Package parentPackage)
        {
            var libraryPackage = (Package) parentPackage.Packages.AddNew(spec.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = parentPackage.PackageID;
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

            parentPackage.Packages.Refresh();
            return new BLibrary(this, libraryPackage);
        }
    }
}