using System;
using System.Collections.Generic;
using System.Linq;
using VIENNAAddIn.validator.upcc3.onTheFly;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class ValidationService
    {
        private readonly Dictionary<ItemId, IRepositoryItem> itemsNeedingValidation = new Dictionary<ItemId, IRepositoryItem>();
        private readonly Dictionary<int, IValidationIssue> validationIssuesById = new Dictionary<int, IValidationIssue>();
        private readonly Dictionary<ItemId, List<IValidationIssue>> validationIssuesByItemId = new Dictionary<ItemId, List<IValidationIssue>>();
        private readonly List<IConstraint> constraints = new List<IConstraint>();
        private readonly List<IValidator> validators = new List<IValidator>();
        private int NextIssueId;

        public event Action<IEnumerable<IValidationIssue>> ValidationIssuesUpdated;

        public IEnumerable<IValidationIssue> ValidationIssues
        {
            get { return new List<IValidationIssue>(validationIssuesById.Values); }
        }

        /// <summary>
        /// Returns the issue with the given ID or <c>null</c>, if no such issue exists.
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public IValidationIssue GetIssueById(int issueId)
        {
            IValidationIssue issue;
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

        private void ValidateItem(IRepositoryItem item)
        {
            AddIssues(item, CheckConstraints(item));
        }

        private IEnumerable<IValidationIssue> CheckConstraints(IRepositoryItem item)
        {
            foreach (var validator in validators)
            {
                if (validator.Matches(item))
                {
                    foreach (var constraintViolation in validator.Validate(item))
                    {
                        yield return new IValidationIssue(NextIssueId++, constraintViolation);
                    }
                }
            }
        }

        private void AddIssues(IRepositoryItem item, IEnumerable<IValidationIssue> itemIssues)
        {
            var issues = new List<IValidationIssue>(itemIssues);
            validationIssuesByItemId[item.Id] = issues;
            foreach (IValidationIssue issue in issues)
            {
                validationIssuesById[issue.Id] = issue;
            }
        }

        private void RemoveIssues(IRepositoryItem item)
        {
            List<IValidationIssue> itemIssues;
            if (validationIssuesByItemId.TryGetValue(item.Id, out itemIssues))
            {
                foreach (IValidationIssue issue in itemIssues)
                {
                    validationIssuesById.Remove(issue.Id);
                }
                validationIssuesByItemId.Remove(item.Id);
                if (ValidationIssuesUpdated != null) ValidationIssuesUpdated(ValidationIssues);
            }
        }

        public void ItemCreatedOrModified(IRepositoryItem item)
        {
            RemoveIssues(item);
            itemsNeedingValidation[item.Id] = item;
        }

        public void ItemDeleted(IRepositoryItem item)
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