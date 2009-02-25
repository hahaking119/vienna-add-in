using System;
using System.Collections.Generic;
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

        public IBusinessLibrary GetRootLibrary()
        {
            var model = (Package) eaRepository.Models.GetAt(0);
            // TODO assert that the model has the <<bLibrary>> stereotype
            IBusinessLibrary library = new BusinessLibrary(this, model, BusinessLibraryType.bLibrary);
            return library;
        }

        public IBusinessLibrary GetLibrary(int id)
        {
            return id == 0 ? null : GetLibrary(eaRepository.GetPackageByID(id));
        }

        public IEnumerable<IBusinessLibrary> Libraries
        {
            get
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

        #endregion

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
                    return new BusinessLibrary(this, package, BusinessLibraryType.BDTLibrary);
                case "BIELibrary":
                    return new BusinessLibrary(this, package, BusinessLibraryType.BIELibrary);
                case "CCLibrary":
                    return new BusinessLibrary(this, package, BusinessLibraryType.CCLibrary);
                case "CDTLibrary":
                    return new CDTLibrary(this, package, BusinessLibraryType.CDTLibrary);
                case "DOCLibrary":
                    return new BusinessLibrary(this, package, BusinessLibraryType.DOCLibrary);
                case "ENUMLibrary":
                    return new BusinessLibrary(this, package, BusinessLibraryType.ENUMLibrary);
                case "PRIMLibrary":
                    return new BusinessLibrary(this, package, BusinessLibraryType.PRIMLibrary);
                default:
                    throw new ArgumentException("Element for provided ID is not a CDT or BDT.");
            }
        }

    }
}