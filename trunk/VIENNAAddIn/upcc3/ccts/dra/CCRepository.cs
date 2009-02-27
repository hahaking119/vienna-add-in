using System;
using System.Collections.Generic;
using System.Linq;
using EA;

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

        public T FindByPath<T>(Path path) where T : class
        {
            if (path.Length == 0)
            {
                return default(T);
            }
            string firstPart = path.FirstPart;
            foreach (Package model in eaRepository.Models)
            {
                foreach (Package package in model.Packages)
                {
                    if (package.Name == firstPart)
                    {
                        return ResolvePath<T>(GetLibrary(package), path.Rest);
                    }
                }
            }
            return default(T);
        }

        public IEnumerable<IBusinessLibrary> AllLibraries()
        {
            foreach (Package model in eaRepository.Models)
            {
                foreach (Package rootPackage in model.Packages)
                {
                    IBusinessLibrary rootLibrary = GetLibrary(rootPackage);
                    yield return rootLibrary;
                    if (rootLibrary is IBLibrary)
                    {
                        foreach (IBusinessLibrary child in ((IBLibrary) rootLibrary).AllChildren)
                        {
                            yield return child;
                        }
                    }
                }
            }
        }

        #endregion

        private static T ResolvePath<T>(IBusinessLibrary library, Path path) where T : class
        {
            if (library == null)
            {
                return default(T);
            }
            if (path.Length == 0)
            {
                return library as T;
            }
            string firstPart = path.FirstPart;
            if (library is IBLibrary)
            {
                return ResolvePath<T>(((IBLibrary) library).FindChildByName(firstPart), path.Rest);
            }
            return ((IElementLibrary) library).ElementByName(firstPart) as T;
        }

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
            string stereotype = package.Element.Stereotype;
            switch (stereotype)
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
                    throw new ArgumentException("Element for provided ID is not a CDT or BDT.");
            }
        }

        public IType GetIType(int id)
        {
            var element = eaRepository.GetElementByID(id);
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