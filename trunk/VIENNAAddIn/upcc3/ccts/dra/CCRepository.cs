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

        public IEnumerable<IBusinessLibrary> AllLibraries()
        {
            foreach (Package model in eaRepository.Models)
            {
                foreach (Package rootPackage in model.Packages)
                {
                    IBusinessLibrary rootLibrary = GetLibrary(rootPackage);
                    yield return rootLibrary;
                    foreach (IBusinessLibrary child in rootLibrary.AllChildren)
                    {
                        yield return child;
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
            string stereotype = package.Element.Stereotype;
            switch (stereotype)
            {
                case "bLibrary":
                    return new BusinessLibrary(this, package, BusinessLibraryType.bLibrary);
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
    }
}