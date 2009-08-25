using System;
using System.Collections.Generic;
using System.Linq;
using VIENNAAddIn.validator.upcc3.onTheFly;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class ValidationService
    {
        private readonly Dictionary<ItemId, RepositoryItem> itemsNeedingValidation = new Dictionary<ItemId, RepositoryItem>();
        private readonly Dictionary<int, ValidationIssue> validationIssuesById = new Dictionary<int, ValidationIssue>();
        private readonly Dictionary<ItemId, List<ValidationIssue>> validationIssuesByItemId = new Dictionary<ItemId, List<ValidationIssue>>();
        private readonly List<IConstraint> constraints = new List<IConstraint>();
        private readonly List<IValidator> validators = new List<IValidator>();
        private int NextIssueId;

        public event Action<IEnumerable<ValidationIssue>> ValidationIssuesUpdated;

        public IEnumerable<ValidationIssue> ValidationIssues
        {
            get { return new List<ValidationIssue>(validationIssuesById.Values); }
        }

        /// <summary>
        /// Returns the issue with the given ID or <c>null</c>, if no such issue exists.
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public ValidationIssue GetIssueById(int issueId)
        {
            ValidationIssue issue;
            return validationIssuesById.TryGetValue(issueId, out issue) ? issue : null;
        }

        public void Validate()
        {
            foreach (var item in itemsNeedingValidation.Values)
            {
                ValidateItem(item);
            }
            itemsNeedingValidation.Clear();
            if (ValidationIssuesUpdated != null) ValidationIssuesUpdated(ValidationIssues);
        }

        private void ValidateItem(RepositoryItem item)
        {
            AddIssues(item, CheckConstraints(item));
        }

        private IEnumerable<ValidationIssue> CheckConstraints(RepositoryItem item)
        {
            foreach (var validator in validators)
            {
                if (validator.Matches(item))
                {
                    foreach (var constraintViolation in validator.Validate(item))
                    {
                        yield return new ValidationIssue(NextIssueId++, constraintViolation);
                    }
                }
            }
        }

        private void AddIssues(RepositoryItem item, IEnumerable<ValidationIssue> itemIssues)
        {
            var issues = new List<ValidationIssue>(itemIssues);
            validationIssuesByItemId[item.Id] = issues;
            foreach (ValidationIssue issue in issues)
            {
                validationIssuesById[issue.Id] = issue;
            }
        }

        private void RemoveIssues(RepositoryItem item)
        {
            List<ValidationIssue> itemIssues;
            if (validationIssuesByItemId.TryGetValue(item.Id, out itemIssues))
            {
                foreach (ValidationIssue issue in itemIssues)
                {
                    validationIssuesById.Remove(issue.Id);
                }
                validationIssuesByItemId.Remove(item.Id);
                if (ValidationIssuesUpdated != null) ValidationIssuesUpdated(ValidationIssues);
            }
        }

        public void ItemCreatedOrModified(RepositoryItem item)
        {
            RemoveIssues(item);
            itemsNeedingValidation[item.Id] = item;
        }

        public void ItemDeleted(RepositoryItem item)
        {
            RemoveIssues(item);
            itemsNeedingValidation.Remove(item.Id);
        }

        public void AddValidator(IValidator validator)
        {
            validators.Add(validator);
        }

        public void AddConstraint(IConstraint constraint)
        {
            constraints.Add(constraint);
        }
    }
}