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

        public IBusinessLibrary GetLibrary(ItemId id)
        {
            IEAPackage library = GetPackageById(id);
            if (library != null && library is IBusinessLibrary)
            {
                return (IBusinessLibrary) library;
            }
            return null;
        }

        public IBusinessLibrary GetLibrary(int id)
        {
            return GetLibrary(ItemId.ForPackage(id));
        }

        public IEnumerable<IBusinessLibrary> AllLibraries()
        {
            return from item in repository.AllItems()
                   where IsBusinessLibrary(item)
                   select WrapBusinessLibrary(item);
        }

        private static IBusinessLibrary WrapBusinessLibrary(IRepositoryItem item)
        {
            return WrapPackage(item) as IBusinessLibrary;
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

        private IEAPackage GetPackageById(ItemId id)
        {
            return WrapPackage(repository.GetItemById(id));
        }

        private static IEAPackage WrapPackage(IRepositoryItem item)
        {
            IEAPackage eaPackage;
            var itemData = item.Data;
            if (itemData.ParentId.IsNull)
            {
                eaPackage = new EAModel(itemData.Id, itemData.Name, itemData.ParentId);
            }
            else
            {
                switch (item.Data.Stereotype)
                {
                    case Stereotype.BInformationV:
                        {
                            eaPackage = new BInformationV(itemData.Id, itemData.Name, itemData.ParentId);
                            break;
                        }
                    case Stereotype.BLibrary:
                        {
                            eaPackage = new BLibrary(itemData.Id,
                                                     itemData.Name,
                                                     itemData.ParentId,
                                                     itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                     itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                     itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                     itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                     itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                     itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                     itemData.GetTaggedValues(TaggedValues.copyright),
                                                     itemData.GetTaggedValues(TaggedValues.owner),
                                                     itemData.GetTaggedValues(TaggedValues.reference));
                            break;
                        }
                    case Stereotype.PRIMLibrary:
                        {
                            eaPackage = new PRIMLibrary(itemData.Id,
                                                        itemData.Name,
                                                        itemData.ParentId,
                                                        itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                        itemData.GetTaggedValues(TaggedValues.copyright),
                                                        itemData.GetTaggedValues(TaggedValues.owner),
                                                        itemData.GetTaggedValues(TaggedValues.reference));
                            break;
                        }
                    case Stereotype.ENUMLibrary:
                        {
                            eaPackage = new ENUMLibrary(itemData.Id,
                                                        itemData.Name,
                                                        itemData.ParentId,
                                                        itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                        itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                        itemData.GetTaggedValues(TaggedValues.copyright),
                                                        itemData.GetTaggedValues(TaggedValues.owner),
                                                        itemData.GetTaggedValues(TaggedValues.reference));
                            break;
                        }
                    case Stereotype.CDTLibrary:
                        {
                            eaPackage = new CDTLibrary(itemData.Id,
                                                       itemData.Name,
                                                       itemData.ParentId,
                                                       itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                       itemData.GetTaggedValues(TaggedValues.copyright),
                                                       itemData.GetTaggedValues(TaggedValues.owner),
                                                       itemData.GetTaggedValues(TaggedValues.reference));
                            break;
                        }
                    case Stereotype.CCLibrary:
                        {
                            eaPackage = new CCLibrary(itemData.Id,
                                                      itemData.Name,
                                                      itemData.ParentId,
                                                      itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                      itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                      itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                      itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                      itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                      itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                      itemData.GetTaggedValues(TaggedValues.copyright),
                                                      itemData.GetTaggedValues(TaggedValues.owner),
                                                      itemData.GetTaggedValues(TaggedValues.reference));
                            break;
                        }
                    case Stereotype.BDTLibrary:
                        {
                            eaPackage = new BDTLibrary(itemData.Id,
                                                       itemData.Name,
                                                       itemData.ParentId,
                                                       itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                       itemData.GetTaggedValues(TaggedValues.copyright),
                                                       itemData.GetTaggedValues(TaggedValues.owner),
                                                       itemData.GetTaggedValues(TaggedValues.reference));
                            break;
                        }
                    case Stereotype.BIELibrary:
                        {
                            eaPackage = new BIELibrary(itemData.Id,
                                                       itemData.Name,
                                                       itemData.ParentId,
                                                       itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                       itemData.GetTaggedValues(TaggedValues.copyright),
                                                       itemData.GetTaggedValues(TaggedValues.owner),
                                                       itemData.GetTaggedValues(TaggedValues.reference));
                            break;
                        }
                    case Stereotype.DOCLibrary:
                        {
                            eaPackage = new DOCLibrary(itemData.Id,
                                                       itemData.Name,
                                                       itemData.ParentId,
                                                       itemData.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                       itemData.GetTaggedValues(TaggedValues.businessTerm),
                                                       itemData.GetTaggedValues(TaggedValues.copyright),
                                                       itemData.GetTaggedValues(TaggedValues.owner),
                                                       itemData.GetTaggedValues(TaggedValues.reference));
                            break;
                        }
                    default:
                        {
                            eaPackage = new OtherPackage(itemData.Id, itemData.Name, itemData.ParentId);
                            break;
                        }
                }
            }
            return eaPackage;
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