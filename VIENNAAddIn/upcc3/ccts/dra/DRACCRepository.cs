using System;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    /// <summary>
    /// An CCRepository that directly accesses (DRA = direct repository access) the underlying EA.Repository
    /// (i.e. it is simply an interface layer on top of the EA.Repository)
    /// </summary>
    public class DRACCRepository : ICCRepository
    {
        private readonly Repository eaRepository;

        public DRACCRepository(Repository eaRepository)
        {
            this.eaRepository = eaRepository;
        }

        #region ICCRepository Members

        public IBusinessLibrary GetRootLibrary()
        {
            var model = (Package) eaRepository.Models.GetAt(0);
            // TODO assert that the model has the <<bLibrary>> stereotype
            IBusinessLibrary library = new DRABusinessLibrary(this, model, BusinessLibraryType.bLibrary);
            return library;
        }

        public IBusinessLibrary GetLibrary(int id)
        {
            return id == 0 ? null : GetLibrary(eaRepository.GetPackageByID(id));
        }

        public ICDT GetCDT(int id)
        {
            return GetCDT(eaRepository.GetElementByID(id));
        }

        public void EachCDT(int cdtLibraryId, Action<ICDT> visit)
        {
            throw new NotImplementedException();
        }

        public void TraverseLibrariesDepthFirst(Action<IBusinessLibrary> visit)
        {
            var model = (Package) eaRepository.Models.GetAt(0);
            foreach (Package package in model.Packages)
            {
                traverseRecursively(package, visit);
            }
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
                    return new DRABusinessLibrary(this, package, BusinessLibraryType.bLibrary);
                case "BDTLibrary":
                    return new DRABusinessLibrary(this, package, BusinessLibraryType.BDTLibrary);
                case "BIELibrary":
                    return new DRABusinessLibrary(this, package, BusinessLibraryType.BIELibrary);
                case "CCLibrary":
                    return new DRABusinessLibrary(this, package, BusinessLibraryType.CCLibrary);
                case "CDTLibrary":
                    return new DRACDTLibrary(this, package, BusinessLibraryType.CDTLibrary);
                case "DOCLibrary":
                    return new DRABusinessLibrary(this, package, BusinessLibraryType.DOCLibrary);
                case "ENUMLibrary":
                    return new DRABusinessLibrary(this, package, BusinessLibraryType.ENUMLibrary);
                case "PRIMLibrary":
                    return new DRABusinessLibrary(this, package, BusinessLibraryType.PRIMLibrary);
                default:
                    throw new ArgumentException("Element for provided ID is not a CDT or BDT.");
            }
        }

        public IDT GetDT(int id)
        {
            Element element = eaRepository.GetElementByID(id);
            if (element == null)
            {
                return null;
            }
            switch (element.Stereotype)
            {
                case "CDT":
                    return GetCDT(element);
                case "BDT":
                    return GetBDT(element);
                default:
                    throw new ArgumentException("Element for provided ID is not a CDT or BDT.");
            }
        }

        private IBDT GetBDT(Element element)
        {
            throw new NotImplementedException();
        }

        public ICDT GetCDT(Element element)
        {
            return new DRACDT(this, element);
        }

        private void traverseRecursively(Package package, Action<IBusinessLibrary> visit)
        {
            visit(GetLibrary(package));
            if (package.Element.Stereotype == "bLibrary")
            {
                foreach (Package child in package.Packages)
                {
                    traverseRecursively(child, visit);
                }
            }
        }

        public IDTComponent GetSUP(Attribute attribute)
        {
            return new DRADTComponent(this, attribute, DTComponentType.SUP);
        }

        public IDTComponent GetCON(Attribute attribute)
        {
            return new DRADTComponent(this, attribute, DTComponentType.CON);
        }
    }

}