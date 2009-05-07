using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    internal class OnTheFlyValidatorImpl : OnTheFlyValidator
    {
        private readonly Repository repository;
        private readonly Dictionary<int, ValidationIssue> validationIssues = new Dictionary<int, ValidationIssue>();

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
                        Package package = repository.GetPackageByID(packageId);
                        if (package.IsCCLibrary())
                        {
                            if (!element.IsACC())
                            {
                                var issue = new InvalidStereotype(repository, guid, objectType, Stereotype.ACC);
                                validationIssues[issue.Id] = issue;
                                repository.EnsureOutputVisible(AddInSettings.AddInName);
                                repository.WriteOutput(AddInSettings.AddInName, issue.Message, issue.Id);
                            }
                        }
                        break;
                    }
            }
        }

        public void FocusIssueItem(int issueId)
        {
            ValidationIssue issue;
            if (validationIssues.TryGetValue(issueId, out issue))
            {
                repository.ShowInProjectView(issue.Item);
            }
        }

        public void ShowQuickFixes(int issueId)
        {
            ValidationIssue issue;
            if (validationIssues.TryGetValue(issueId, out issue))
            {
                repository.ShowInProjectView(issue.Item);
                issue.QuickFixes.First().Execute(repository, issue.GUID, issue.ObjectType);
            }
        }

        #endregion
    }
}