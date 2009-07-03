using System;
using System.Collections.Generic;
using System.Linq;
using EA;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class ValidatingCCRepository : ICCRepository
    {
        private readonly ContentLoader contentLoader;
        private readonly Dictionary<int, EAPackage> eaPackagesById = new Dictionary<int, EAPackage>();
        private readonly List<EAPackage> models;

        public ValidatingCCRepository(Repository eaRepository)
        {
            contentLoader = new ContentLoader(eaRepository);
            models = contentLoader.LoadRepositoryContent();
//            ValidateAll();
        }

        private readonly Dictionary<int, IValidationIssue> validationIssues = new Dictionary<int, IValidationIssue>();

        public event EventHandler<ValidationIssueAddedEventArgs> ValidationIssueAdded;

        private void AddValidationIssue(IValidationIssue validationIssue)
        {
            validationIssues[validationIssue.Id] = validationIssue;
            if (ValidationIssueAdded != null) ValidationIssueAdded(this, new ValidationIssueAddedEventArgs(validationIssue));
        }

        public IValidationIssue GetValidationIssue(int issueId)
        {
            IValidationIssue issue;
            return validationIssues.TryGetValue(issueId, out issue) ? issue : null;
        }

        public List<IValidationIssue> ValidationIssues
        {
            get { return new List<IValidationIssue>(validationIssues.Values); }
        }

        #region ICCRepository Members

        public IBusinessLibrary GetLibrary(int id)
        {
            EAPackage library = GetPackageById(id);
            if (library != null && library is IBusinessLibrary)
            {
                return (IBusinessLibrary) library;
            }
            return null;
        }

        public IEnumerable<IBusinessLibrary> AllLibraries()
        {
            var libraries = new List<IBusinessLibrary>();
            WithEachLibraryDo<IBusinessLibrary>(libraries.Add);
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

        public void ValidateAll()
        {
            foreach (EAPackage model in models)
            {
                foreach(var issue in model.ValidateAll())
                {
                    AddValidationIssue(issue);
                }
            }
        }

        private EAPackage GetPackageById(int id)
        {
            EAPackage package;
            eaPackagesById.TryGetValue(id, out package);
            return package;
        }

        public void WithEachLibraryDo<TLibrary>(Action<TLibrary> action)
        {
            foreach (EAPackage eaPackage in models)
            {
                if (eaPackage is TLibrary)
                {
                    action((TLibrary) eaPackage);
                }
                eaPackage.WithEachSubPackageDo(action);
            }
        }

        public void ReloadPackage(int id)
        {
            EAPackage package = GetPackageById(id);
            if (package == null)
            {
                // a new package
                // TODO
            }
            else
            {
                eaPackagesById.Remove(id);
                if (package is EAModel)
                {
                    // TODO simply reload the model's content
                }
                else
                {
                    EAPackage parent = package.ParentPackage;
                    parent.ReplaceSubPackage(id, contentLoader.LoadPackage(id));
                }
            }
        }
    }

    public interface IValidationIssue
    {
        int ItemId { get; }
        int Id { get; }
        object ResolveItem(Repository repository);
    }

    internal class ContentLoader
    {
        private readonly Repository eaRepository;

        public ContentLoader(Repository eaRepository)
        {
            this.eaRepository = eaRepository;
        }

        public List<EAPackage> LoadRepositoryContent()
        {
            var models = new List<EAPackage>();
            foreach (Package model in eaRepository.Models)
            {
                models.Add(LoadPackageRecursively(model));
            }
            return models;
        }

        private static EAPackage LoadPackageRecursively(Package package)
        {
            EAPackage eaPackage = LoadPackage(package);
            foreach (Package subPackage in package.Packages)
            {
                eaPackage.AddSubPackage(LoadPackageRecursively(subPackage));
            }
            foreach (Element element in package.Elements)
            {
                eaPackage.AddElement(LoadElement(element));
            }
            return eaPackage;
        }

        public static EAElement LoadElement(Element element)
        {
            return new OtherElement(element.ElementID);
        }

        public static EAPackage LoadPackage(Package package)
        {
            EAPackage eaPackage;
            int packageId = package.PackageID;
            if (package.ParentID == 0)
            {
                eaPackage = new EAModel(packageId);
            }
            else
            {
                switch (package.Element.Stereotype)
                {
                    case Stereotype.BInformationV:
                    {
                        eaPackage = new BInformationV(packageId);
                        break;
                    }
                    case Stereotype.BLibrary:
                    {
                        eaPackage = new BLibrary(package);
                        break;
                    }
                    case Stereotype.PRIMLibrary:
                    {
                        eaPackage = new PRIMLibrary(package);
                        break;
                    }
                    case Stereotype.ENUMLibrary:
                    {
                        eaPackage = new ENUMLibrary(package);
                        break;
                    }
                    case Stereotype.CDTLibrary:
                    {
                        eaPackage = new CDTLibrary(package);
                        break;
                    }
                    case Stereotype.CCLibrary:
                    {
                        eaPackage = new CCLibrary(package);
                        break;
                    }
                    case Stereotype.BDTLibrary:
                    {
                        eaPackage = new BDTLibrary(package);
                        break;
                    }
                    case Stereotype.BIELibrary:
                    {
                        eaPackage = new BIELibrary(package);
                        break;
                    }
                    case Stereotype.DOCLibrary:
                    {
                        eaPackage = new DOCLibrary(package);
                        break;
                    }
                    default:
                    {
                        eaPackage = new OtherPackage(packageId);
                        break;
                    }
                }
            }
            return eaPackage;
        }

        public EAPackage LoadPackage(int id)
        {
            return LoadPackage(eaRepository.GetPackageByID(id));
        }
    }

    internal class OtherElement : EAElement
    {
        public OtherElement(int id)
        {
            Id = id;
        }

        #region EAElement Members

        public int Id { get; set; }
        public EAPackage Package { get; set; }

        #endregion
    }

    internal class OtherPackage : AbstractEAPackage
    {
        public OtherPackage(int id) : base(id)
        {
        }

        public override IEnumerable<IValidationIssue> Validate()
        {
            yield break;
        }
    }

    internal class BInformationV : AbstractEAPackage
    {
        public BInformationV(int id) : base(id)
        {
        }

        public override IEnumerable<IValidationIssue> Validate()
        {
            yield break;
        }
    }

    internal class EAModel : AbstractEAPackage
    {
        public EAModel(int id) : base(id)
        {
        }

        public override IEnumerable<IValidationIssue> Validate()
        {
            yield break;
        }
    }

    internal interface EAPackage
    {
        EAPackage ParentPackage { get; set; }
        int Id { get; }
        void WithEachSubPackageDo<T>(Action<T> action);
        void AddSubPackage(EAPackage package);
        void ReplaceSubPackage(int id, EAPackage package);
        IEnumerable<IValidationIssue> ValidateAll();
        IEnumerable<IValidationIssue> Validate();
        void AddElement(EAElement element);
    }

    internal interface EAElement
    {
        int Id { get; }
        EAPackage Package { get; set; }
    }

    internal abstract class AbstractEAPackage : EAPackage
    {
        protected readonly List<EAElement> elements = new List<EAElement>();
        protected readonly List<EAPackage> subPackages = new List<EAPackage>();

        protected AbstractEAPackage(int id)
        {
            Id = id;
        }

        #region EAPackage Members

        public void WithEachSubPackageDo<T>(Action<T> action)
        {
            foreach (EAPackage eaPackage in subPackages)
            {
                if (eaPackage is T)
                {
                    action((T) eaPackage);
                }
                eaPackage.WithEachSubPackageDo(action);
            }
        }

        public void AddSubPackage(EAPackage package)
        {
            subPackages.Add(package);
            package.ParentPackage = this;
        }

        public EAPackage ParentPackage { get; set; }
        public int Id { get; private set; }

        public void ReplaceSubPackage(int id, EAPackage package)
        {
            subPackages.RemoveAll(p => p.Id == id);
            AddSubPackage(package);
        }

        public IEnumerable<IValidationIssue> ValidateAll()
        {
            foreach (IValidationIssue issue in Validate())
            {
                yield return issue;
            }
            foreach (EAPackage eaPackage in subPackages)
            {
                foreach (IValidationIssue issue in eaPackage.ValidateAll())
                {
                    yield return issue;
                }
            }
        }

        public abstract IEnumerable<IValidationIssue> Validate();

        public void AddElement(EAElement element)
        {
            elements.Add(element);
            element.Package = this;
        }

        #endregion
    }
    public class ValidationIssueAddedEventArgs : EventArgs
    {
        public IValidationIssue Issue { get; private set; }

        public ValidationIssueAddedEventArgs(IValidationIssue issue)
        {
            Issue = issue;
        }
    }


}