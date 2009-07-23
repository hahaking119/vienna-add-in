using System;
using System.Collections.Generic;
using System.Linq;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class ValidatingCCRepository : ICCRepository
    {
        private readonly Dictionary<int, IEAElement> eaElementsById = new Dictionary<int, IEAElement>();
        private readonly Dictionary<int, IEAPackage> eaPackagesById = new Dictionary<int, IEAPackage>();
        private readonly PackageRoot root = new PackageRoot();
        private readonly Dictionary<int, IValidationIssue> validationIssues = new Dictionary<int, IValidationIssue>();

        public List<IValidationIssue> ValidationIssues
        {
            get { return new List<IValidationIssue>(validationIssues.Values); }
        }

        #region ICCRepository Members

        public IBusinessLibrary GetLibrary(int id)
        {
            IEAPackage library = GetPackageById(id);
            if (library != null && library is IBusinessLibrary)
            {
                return (IBusinessLibrary) library;
            }
            return null;
        }

        public IEnumerable<IBusinessLibrary> AllLibraries()
        {
            var libraries = new List<IBusinessLibrary>();
            root.WithEachSubPackageDo((Action<IBusinessLibrary>) libraries.Add);
            return libraries;
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

        /// <summary>
        /// Traverse item tree depth-first and execute action on each item.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public void WithAllItemsDo(Action<IEAItem> action)
        {
            root.WithAllItemsDo(action);
        }

        public event Action<IEnumerable<IValidationIssue>> ValidationIssuesUpdated;

        public IValidationIssue GetValidationIssue(int issueId)
        {
            IValidationIssue issue;
            return validationIssues.TryGetValue(issueId, out issue) ? issue : null;
        }

        public void ValidateAll()
        {
            validationIssues.Clear();
            foreach (IValidationIssue issue in root.ValidateAll())
            {
                validationIssues[issue.Id] = issue;
            }
            if (ValidationIssuesUpdated != null) ValidationIssuesUpdated(new List<IValidationIssue>(validationIssues.Values));
        }

        private IEAPackage GetPackageById(int id)
        {
            if (id == 0)
            {
                return root;
            }
            IEAPackage package;
            eaPackagesById.TryGetValue(id, out package);
            return package;
        }

        private IEAElement GetElementById(int id)
        {
            IEAElement element;
            eaElementsById.TryGetValue(id, out element);
            return element;
        }

        private IEAPackage ReplacePackage(IEAPackage newPackage)
        {
            IEAPackage oldPackage = GetPackageById(newPackage.Id);
            eaPackagesById[newPackage.Id] = newPackage;
            return oldPackage;
        }

        private IEAElement ReplaceElement(IEAElement newElement)
        {
            IEAElement oldElement = GetElementById(newElement.Id);
            eaElementsById[newElement.Id] = newElement;
            return oldElement;
        }

        public void HandlePackageDeleted(int id)
        {
            IEAPackage package = GetPackageById(id);
            if (package != null)
            {
                IEAPackage parent = package.ParentPackage;
                parent.RemoveSubPackage(id);
                RemoveItem(package);
                package.WithAllItemsDo(RemoveItem);
            }
        }

        private void RemoveItem(IEAItem item)
        {
            RemoveValidationIssues(item);
            if (item is IEAPackage)
            {
                eaPackagesById.Remove(item.Id);
            }
            else
            {
                eaElementsById.Remove(item.Id);
            }
        }

        private void RemoveValidationIssues(IValidating item)
        {
            foreach (IValidationIssue issue in item.ValidationIssues)
            {
                validationIssues.Remove(issue.Id);
            }
        }

        public void HandleElementDeleted(int id)
        {
            IEAElement element = GetElementById(id);
            if (element != null)
            {
                IEAPackage package = element.Package;
                package.RemoveElement(id);
                RemoveItem(element);
            }
        }

        public void HandleElementCreatedOrModified(IEAElement newElement)
        {
            IEAElement oldElement = ReplaceElement(newElement);

            IEAPackage package = oldElement == null ? GetPackageById(newElement.PackageId) : oldElement.Package;

            if (oldElement == null)
            {
                package.AddElement(newElement);
            }
            else
            {
                package.ReplaceElement(newElement);
            }
        }

        public void HandlePackageCreatedOrModified(IEAPackage newPackage)
        {
            IEAPackage oldPackage = ReplacePackage(newPackage);

            IEAPackage parent = oldPackage == null ? GetPackageById(newPackage.ParentId) : oldPackage.ParentPackage;

            if (oldPackage == null)
            {
                parent.AddSubPackage(newPackage);
            }
            else
            {
                newPackage.CopyChildren(oldPackage);
                parent.ReplaceSubPackage(newPackage);
            }
        }

    }
}