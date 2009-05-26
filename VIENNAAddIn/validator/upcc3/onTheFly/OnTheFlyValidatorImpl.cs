using System;
using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class OnTheFlyValidatorImpl : OnTheFlyValidator
    {
        private readonly Repository repository;
        private readonly Dictionary<int, ValidationIssue> validationIssues = new Dictionary<int, ValidationIssue>();

        private readonly Dictionary<int, IValidatingBusinessLibrary> libraries = new Dictionary<int, IValidatingBusinessLibrary>();

        public OnTheFlyValidatorImpl(Repository repository)
        {
            this.repository = repository;
        }

        #region OnTheFlyValidator Members

        public void ProcessItem(string guid, ObjectType objectType)
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
                            library.AddElement(element);
                        }
                        // TODO else package is not a UPCC library => if element has a valid UPCC stereotype, show error/quickfix for package
//                        Package package = repository.GetPackageByID(packageId);
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
            return null;
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
            ValidationIssue issue;
            if (validationIssues.TryGetValue(issueId, out issue))
            {
//                repository.ShowInProjectView(issue.Item);
//                issue.QuickFixes.First().Execute(repository, issue.GUID, issue.ObjectType);
            }
        }

        #endregion
    }

    internal interface IValidatingBusinessLibrary
    {
        void AddElement(Element element);
    }
}