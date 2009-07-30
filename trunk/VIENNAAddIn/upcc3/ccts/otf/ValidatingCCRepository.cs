using System;
using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;
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
            validationService.AddConstraint(new BLibraryConstraints());

            repository = new HierarchicalRepository();
            repository.OnItemCreatedOrModified += validationService.ItemCreatedOrModified;
            repository.OnItemDeleted += validationService.ItemDeleted;

            contentLoader = new RepositoryContentLoader(eaRepository);
            contentLoader.ItemLoaded += repository.ItemLoaded;
        }

        public IEnumerable<IValidationIssue> ValidationIssues
        {
            get { return validationService.ValidationIssues; }
        }

        #region ICCRepository Members

        public IBusinessLibrary GetLibrary(int id)
        {
            return WrapItem(repository.GetItemById(ItemId.ForPackage(id))) as IBusinessLibrary;
        }

        public IEnumerable<IBusinessLibrary> AllLibraries()
        {
            return from item in repository.AllItems()
                   where IsBusinessLibrary(item)
                   select WrapBusinessLibrary(item);
        }

        private static IBusinessLibrary WrapBusinessLibrary(IRepositoryItem item)
        {
            return WrapItem(item) as IBusinessLibrary;
        }

        private static bool IsBusinessLibrary(IRepositoryItem item)
        {
            return Stereotype.IsBusinessLibraryStereotype(item.Data.Stereotype);
        }

        public IEnumerable<T> Libraries<T>() where T : IBusinessLibrary
        {
            return from library in AllLibraries()
                   where library is T
                   select (T) library;
        }

        public T LibraryByName<T>(string name) where T : IBusinessLibrary
        {
            throw new NotImplementedException();
        }

        public object FindByPath(Path path)
        {
            throw new NotImplementedException();
        }

        #endregion

        public event Action<IEnumerable<IValidationIssue>> ValidationIssuesUpdated
        {
            add { validationService.ValidationIssuesUpdated += value; }
            remove { validationService.ValidationIssuesUpdated -= value; }
        }

        public IValidationIssue GetValidationIssue(int issueId)
        {
            return validationService.GetIssueById(issueId);
        }

        private static object WrapItem(IRepositoryItem item)
        {
            return CCItemWrapper.Wrap(item);
        }

        public void ItemDeleted(ItemId id)
        {
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