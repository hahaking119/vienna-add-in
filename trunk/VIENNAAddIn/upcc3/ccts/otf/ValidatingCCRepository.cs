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
using VIENNAAddIn.upcc3.ccts.otf.validators;
using VIENNAAddInUtils;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class ValidatingCCRepository : ICCRepository
    {
        private readonly RepositoryContentLoader contentLoader;
        private readonly HierarchicalRepository repository;
        private readonly ValidationService validationService;

        public ValidatingCCRepository(Repository eaRepository)
        {
            validationService = new ValidationService();
            validationService.AddValidator(new BLibraryValidator());
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.PRIMLibrary, Stereotype.PRIM));
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.ENUMLibrary, Stereotype.ENUM));
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.CDTLibrary, Stereotype.CDT));
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.CCLibrary, Stereotype.ACC));
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.BDTLibrary, Stereotype.BDT));
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.BIELibrary, Stereotype.ABIE));
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.DOCLibrary, Stereotype.ABIE));
            validationService.AddValidator(new PRIMValidator());

            repository = new HierarchicalRepository();
            repository.OnItemCreatedOrModified += validationService.ItemCreatedOrModified;
            repository.OnItemDeleted += validationService.ItemDeleted;

            contentLoader = new RepositoryContentLoader(eaRepository);
            contentLoader.ItemLoaded += repository.ItemLoaded;
        }

        public IEnumerable<ValidationIssue> ValidationIssues
        {
            get { return validationService.ValidationIssues; }
        }

        #region ICCRepository Members

        public IEnumerable<IBLibrary> GetBLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.bLibrary
                   select WrapItem(item) as IBLibrary;
        }

        public IEnumerable<IPRIMLibrary> GetPrimLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.PRIMLibrary
                   select WrapItem(item) as IPRIMLibrary;
        }

        public IEnumerable<IENUMLibrary> GetEnumLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.ENUMLibrary
                   select WrapItem(item) as IENUMLibrary;
        }

        public IEnumerable<ICDTLibrary> GetCdtLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.CDTLibrary
                   select WrapItem(item) as ICDTLibrary;
        }

        public IEnumerable<ICCLibrary> GetCcLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.CCLibrary
                   select WrapItem(item) as ICCLibrary;
        }

        public IEnumerable<IBdtLibrary> GetBdtLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.BDTLibrary
                   select WrapItem(item) as IBdtLibrary;
        }

        public IEnumerable<IBIELibrary> GetBieLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.BIELibrary
                   select WrapItem(item) as IBIELibrary;
        }

        public IEnumerable<IDOCLibrary> GetDocLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.DOCLibrary
                   select WrapItem(item) as IDOCLibrary;
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

        public IBdtLibrary GetBdtLibraryById(int id)
        {
            return GetLibraryById(id) as IBdtLibrary;
        }

        public IBIELibrary GetBieLibraryById(int id)
        {
            return GetLibraryById(id) as IBIELibrary;
        }

        public IDOCLibrary GetDocLibraryById(int id)
        {
            return GetLibraryById(id) as IDOCLibrary;
        }

        public IPRIM GetPrimById(int id)
        {
            throw new NotImplementedException();
        }

        public IENUM GetEnumById(int id)
        {
            throw new NotImplementedException();
        }

        public IABIE GetAbieByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Path> GetRootLocations()
        {
            throw new NotImplementedException();
        }

        public IBLibrary CreateRootBLibrary(Path rootLocation, BLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public ICDT GetCdtById(int id)
        {
            throw new NotImplementedException();
        }

        public IACC GetAccById(int id)
        {
            throw new NotImplementedException();
        }

        public IBdt GetBdtById(int id)
        {
            throw new NotImplementedException();
        }

        public IABIE GetAbieById(int id)
        {
            throw new NotImplementedException();
        }

        public IBLibrary GetBLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IPRIMLibrary GetPrimLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IENUMLibrary GetEnumLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public ICDTLibrary GetCdtLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public ICCLibrary GetCcLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IBdtLibrary GetBdtLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IBIELibrary GetBieLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IDOCLibrary GetDocLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IPRIM GetPrimByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IENUM GetEnumByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public ICDT GetCdtByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IACC GetAccByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IBdt GetBdtByPath(Path path)
        {
            throw new NotImplementedException();
        }

        #endregion

        private object GetLibraryById(int id)
        {
            return WrapItem(repository.GetItemById(ItemId.ForPackage(id)));
        }

        public event Action<IEnumerable<ValidationIssue>> ValidationIssuesUpdated
        {
            add { validationService.ValidationIssuesUpdated += value; }
            remove { validationService.ValidationIssuesUpdated -= value; }
        }

        public ValidationIssue GetValidationIssue(int issueId)
        {
            return validationService.GetIssueById(issueId);
        }

        private static object WrapItem(RepositoryItem item)
        {
            return CCItemWrapper.Wrap(item);
        }

        public void ItemDeleted(ItemId id)
        {
            contentLoader.ItemDeleted(id);
            repository.ItemDeleted(id);
            validationService.Validate();
        }

        public void LoadRepositoryContent()
        {
            contentLoader.LoadRepositoryContent();
            validationService.Validate();
        }

        public void LoadItemByGUID(ObjectType objectType, string guid)
        {
            contentLoader.LoadItemByGUID(objectType, guid);
            validationService.Validate();
        }

        public void LoadElementByID(int id)
        {
            contentLoader.LoadElementByID(id);
            validationService.Validate();
        }

        public void LoadPackageByID(int id)
        {
            contentLoader.LoadPackageByID(id);
            validationService.Validate();
        }
    }
}