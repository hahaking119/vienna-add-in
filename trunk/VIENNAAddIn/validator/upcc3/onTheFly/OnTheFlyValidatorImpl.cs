using System;
using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class OnTheFlyValidatorImpl : OnTheFlyValidator
    {
        private readonly Repository repository;
        private readonly Dictionary<int, ValidationIssue> validationIssues = new Dictionary<int, ValidationIssue>();

        private readonly Dictionary<int, IValidatingBusinessLibrary> libraries = new Dictionary<int, IValidatingBusinessLibrary>();
        private OnTheFlyValidationContext validationContext;

        public OnTheFlyValidatorImpl(Repository repository)
        {
            this.repository = repository;
            validationContext = new OnTheFlyValidationContext();
            validationContext.ValidationIssueAdded += validationContext_ValidationIssueAdded;
        }

        private void validationContext_ValidationIssueAdded(object sender, ValidationIssueAddedEventArgs e)
        {
            var issue = e.Issue;
            repository.EnsureOutputVisible(AddInSettings.AddInName);
            repository.WriteOutput(AddInSettings.AddInName, "ERROR: Validation issue " + issue.GetType().Name + " (GUID=" + issue.GUID + ")", issue.Id);
        }

        #region OnTheFlyValidator Members

        public void ProcessItemModified(string guid, ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.otElement:
                {
                    Element element = repository.GetElementByGuid(guid);
                    int packageId = element.PackageID;
                    var library = GetLibrary(packageId);
                    if (library != null)
                    {
//                            library.AddElement(element);
                    }
                    // TODO else package is not a UPCC library => if element has a valid UPCC stereotype, show error/quickfix for package
                    Package package = repository.GetPackageByID(packageId);
//                        if (package.IsCCLibrary())
//                        {
//                            if (!element.IsACC())
//                            {
//                                var issue = new InvalidStereotype(repository, guid, objectType, Stereotype.ACC);
//                                validationIssues[issue.Id] = issue;
//                                repository.EnsureOutputVisible(AddInSettings.AddInName);
//                                repository.WriteOutput(AddInSettings.AddInName, issue.Message, issue.Id);
//                            }
//                        } else if (package.IsPRIMLibrary())
//                        {
//
//                        }
                    break;
                }
            }
        }

        private IValidatingBusinessLibrary GetLibrary(int id)
        {
            IValidatingBusinessLibrary library;
            if (libraries.TryGetValue(id, out library))
            {
                return library;
            }
            // TODO this should not happen here:
            Package package = repository.GetPackageByID(id);
            if (package.IsPRIMLibrary())
            {
                library = new ValidatingPRIMLibrary(validationContext);
                libraries[id] = library;
            }
            return library;
        }

        public void FocusIssueItem(int issueId)
        {
            ValidationIssue issue;
            if (validationIssues.TryGetValue(issueId, out issue))
            {
//                repository.ShowInProjectView(issue.Item);
            }
        }

        public void ShowQuickFixes(int issueId)
        {
            ValidationIssue issue = validationContext.GetIssue(issueId);
            if (issue != null)
            {
                var item = issue.GetItem(repository);
                repository.ShowInProjectView(item);
                issue.QuickFixes.First().Execute(repository, item);
            }
        }

        public void ProcessElementCreated(int id)
        {
            Element element = repository.GetElementByID(id);
            int packageId = element.PackageID;
            var library = GetLibrary(packageId);
            if (library != null)
            {
                library.ProcessElementCreated(element);
            }
            // TODO else package is not a UPCC library => if element has a valid UPCC stereotype, show error/quickfix for package
            Package package = repository.GetPackageByID(packageId);
        }

        #endregion
    }

    public class ValidationIssueAddedEventArgs : EventArgs
    {
        public ValidationIssue Issue { get; private set; }

        public ValidationIssueAddedEventArgs(ValidationIssue issue)
        {
            Issue = issue;
        }
    }

    public class OnTheFlyValidationContext : IValidationContext
    {
        private readonly Dictionary<int, ValidationIssue> validationIssues = new Dictionary<int, ValidationIssue>();

        public event EventHandler<ValidationIssueAddedEventArgs> ValidationIssueAdded;

        public void AddValidationIssue(ValidationIssue validationIssue)
        {
            validationIssues[validationIssue.Id] = validationIssue;
            ValidationIssueAdded(this, new ValidationIssueAddedEventArgs(validationIssue));
        }

        public ValidationIssue GetIssue(int issueId)
        {
            ValidationIssue issue;
            if (validationIssues.TryGetValue(issueId, out issue))
            {
                return issue;
            }
            return null;
        }
    }

    public class NoSubpackagesAllowed : ValidationIssue
    {
        public NoSubpackagesAllowed(string guid)
            : base(guid)
        {
        }

        public override IEnumerable<QuickFix> QuickFixes
        {
            get { return new QuickFix[] {new DeletePackage(GUID)}; }
        }

        public override object GetItem(Repository repository)
        {
            return repository.GetPackageByGuid(GUID);
        }
    }

    public class DeletePackage : QuickFix
    {
        private readonly string packageGUID;

        public DeletePackage(string packageGUID)
        {
            this.packageGUID = packageGUID;
        }

        #region QuickFix Members

        public void Execute(Repository repository, object item)
        {
            // TODO ask the user for a confirmation?
            // TODO if confirmed, delete the package
            throw new NotImplementedException();
        }

        #endregion
    }

    public interface IValidationContext
    {
        void AddValidationIssue(ValidationIssue validationIssue);
    }

    internal interface IValidatingBusinessLibrary
    {
        void ProcessElementCreated(Element element);
    }

    public class ValidatingPRIMLibrary : IValidatingBusinessLibrary
    {
        private readonly IValidationContext validationContext;
        private List<IPRIM> prims = new List<IPRIM>();

        public ValidatingPRIMLibrary(IValidationContext validationContext)
        {
            this.validationContext = validationContext;
        }

        public IEnumerable<IPRIM> PRIMs
        {
            get { return prims; }
        }

        public void ProcessElementCreated(Element element)
        {
            ValidateStereotype(element);
            ValidateDuplicateNames(element);
            prims.Add(new PRIM(element));
        }

        private void ValidateDuplicateNames(Element element)
        {
            string name = element.Name;
            bool hasDuplicateName = false;
            foreach (IPRIM prim in PRIMs)
            {
                if (name == prim.Name)
                {
                    hasDuplicateName = true;
                    validationContext.AddValidationIssue(new DuplicateElementName(prim.GUID));
                }
            }
            if (hasDuplicateName)
            {
                validationContext.AddValidationIssue(new DuplicateElementName(element.ElementGUID));
            }
        }

        private void ValidateStereotype(Element element)
        {
            if (element.Stereotype != Stereotype.PRIM)
            {
                validationContext.AddValidationIssue(new InvalidElementStereotype(element.ElementGUID, Stereotype.PRIM));
            }
        }

        public void ProcessCreatePackage(Package package)
        {
            validationContext.AddValidationIssue(new NoSubpackagesAllowed(package.PackageGUID));
        }
    }

    public class PRIM : IPRIM
    {
        private readonly Element element;

        public PRIM(Element element)
        {
            this.element = element;
        }

        #region IPRIM Members

        public int Id
        {
            get { throw new NotImplementedException(); }
        }

        public string GUID
        {
            get { return element.ElementGUID; }
        }

        public string Name
        {
            get { return element.Name; }
        }

        public string DictionaryEntryName
        {
            get { throw new NotImplementedException(); }
        }

        public string Definition
        {
            get { throw new NotImplementedException(); }
        }

        public string UniqueIdentifier
        {
            get { throw new NotImplementedException(); }
        }

        public string VersionIdentifier
        {
            get { throw new NotImplementedException(); }
        }

        public string LanguageCode
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { throw new NotImplementedException(); }
        }

        public IBusinessLibrary Library
        {
            get { throw new NotImplementedException(); }
        }

        public string Pattern
        {
            get { throw new NotImplementedException(); }
        }

        public string FractionDigits
        {
            get { throw new NotImplementedException(); }
        }

        public string Length
        {
            get { throw new NotImplementedException(); }
        }

        public string MaxExclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MaxInclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MaxLength
        {
            get { throw new NotImplementedException(); }
        }

        public string MinExclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MinInclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MinLength
        {
            get { throw new NotImplementedException(); }
        }

        public string TotalDigits
        {
            get { throw new NotImplementedException(); }
        }

        public string WhiteSpace
        {
            get { throw new NotImplementedException(); }
        }

        public IPRIM IsEquivalentTo
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}