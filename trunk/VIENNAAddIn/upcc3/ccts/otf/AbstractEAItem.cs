using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public abstract class AbstractEAItem : IEAItem
    {
        private readonly List<IValidationIssue> validationIssues = new List<IValidationIssue>();
        private bool needsValidation;

        protected AbstractEAItem(int id, string name)
        {
            Id = id;
            Name = name;
            NeedsValidation = true;
        }

        #region IEAItem Members

        public IEnumerable<IValidationIssue> Validate()
        {
            if (NeedsValidation)
            {
                validationIssues.AddRange(PerformValidation());
                NeedsValidation = false;
            }
            return ValidationIssues;
        }

        public IEnumerable<IValidationIssue> ValidationIssues
        {
            get { return validationIssues; }
        }

        public abstract IEnumerable<IValidationIssue> ValidateAll();

        public int Id { get; private set; }

        public string Name { get; private set; }

        public bool NeedsValidation
        {
            get
            {
                return needsValidation;
            }
            set
            {
                needsValidation = value;
                if (needsValidation)
                {
                    // old validation issues are no longer relevant
                    validationIssues.Clear();
                }
            }
        }

        #endregion

        protected virtual IEnumerable<IValidationIssue> PerformValidation()
        {
            yield break;
        }
    }
}