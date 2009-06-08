// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    /// <summary>
    /// A CCRepository that directly accesses (DRA = direct repository access) the underlying EA.Repository
    /// (i.e. it is simply an interface layer on top of the EA.Repository)
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

        #region ICCRepository Members

        public IBusinessLibrary GetLibrary(int id)
        {
            return id == 0 ? null : GetLibrary(eaRepository.GetPackageByID(id));
        }

        public IEnumerable<T> Libraries<T>() where T : IBusinessLibrary
        {
            return from library in AllLibraries()
                   where library is T
                   select (T) library;
        }

        public T LibraryByName<T>(string name) where T : IBusinessLibrary
        {
            var libs = new List<T>(from library in Libraries<T>() where library.Name == name select library);
            if (libs.Count > 1)
            {
                throw new Exception(String.Format("Inconsistent data: found two libraries with name '{0}'.", name));
            }
            return libs.Count == 0 ? default(T) : libs[0];
        }

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

        public IEnumerable<IBusinessLibrary> AllLibraries()
        {
            foreach (var rootBLibraryPackage in AllRootBLibraryPackages())
            {
                var bLibrary = (IBLibrary) GetLibrary(rootBLibraryPackage);
                yield return bLibrary;
                foreach (IBusinessLibrary child in bLibrary.AllChildren)
                {
                    yield return child;
                }
            }
        }

        private IEnumerable<Package> AllRootBLibraryPackages()
        {
            foreach (Package model in eaRepository.Models)
            {
                foreach (Package rootPackage in model.Packages)
                {
                    if (rootPackage.HasStereotype(Stereotype.BInformationV))
                    {
                        foreach (Package subPackage in rootPackage.Packages)
                        {
                            if (subPackage.HasStereotype(Stereotype.BLibrary))
                            {
                                yield return subPackage;
                            }
                        }
                    }
                    else
                    {
                        if (rootPackage.HasStereotype(Stereotype.BLibrary))
                        {
                            yield return rootPackage;
                        }
                    }
                }
            }
        }

        #endregion

        public ICDT GetCDT(int id)
        {
            return new CDT(this, eaRepository.GetElementByID(id));
        }

        public IACC GetACC(int id)
        {
            return new ACC(this, eaRepository.GetElementByID(id));
        }

        public IBDT GetBDT(int id)
        {
            return new BDT(this, eaRepository.GetElementByID(id));
        }

        public IABIE GetABIE(int id)
        {
            return new ABIE(this, eaRepository.GetElementByID(id));
        }

        public IBusinessLibrary GetLibrary(Package package)
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

        public object GetElement(Element element)
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
        public IBLibrary CreateBLibrary(LibrarySpec spec, Package parentPackage)
        {
            return new BLibrary(this, BusinessLibrary.CreateLibraryPackage(spec, parentPackage, Stereotype.BLibrary));
        }
    }
}