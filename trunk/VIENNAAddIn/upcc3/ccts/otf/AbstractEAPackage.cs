using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public abstract class AbstractEAPackage : AbstractEAItem, IEAPackage
    {
        protected readonly List<IEAElement> elements = new List<IEAElement>();
        protected readonly List<IEAPackage> subPackages = new List<IEAPackage>();

        #region IEAPackage Members

        protected AbstractEAPackage(int id, string name, int parentId) : base(id, name)
        {
            ParentId = parentId;
        }

        public IEnumerable<IEAPackage> SubPackages
        {
            get { return subPackages; }
        }

        public int ParentId { get; private set; }

        public void WithEachSubPackageDo<T>(Action<T> action)
        {
            foreach (IEAPackage eaPackage in subPackages)
            {
                if (eaPackage is T)
                {
                    action((T) eaPackage);
                }
                eaPackage.WithEachSubPackageDo(action);
            }
        }

        public void WithAllItemsDo(Action<IEAItem> action)
        {
            foreach (IEAElement eaElement in EAElements)
            {
                action(eaElement);
            }
            foreach (IEAPackage eaPackage in subPackages)
            {
                action(eaPackage);
                eaPackage.WithAllItemsDo(action);
            }
        }

        public void AddSubPackage(IEAPackage package)
        {
            subPackages.Add(package);
            package.ParentPackage = this;
            NeedsValidation = true;
        }

        public IEAPackage ParentPackage { get; set; }

        public IEnumerable<IEAElement> EAElements
        {
            get { return elements; }
        }

        public void ReplaceSubPackage(IEAPackage package)
        {
            RemoveSubPackage(package.Id);
            AddSubPackage(package);
        }

        public void ReplaceElement(IEAElement element)
        {
            RemoveElement(element.Id);
            AddElement(element);
        }

        public override IEnumerable<IValidationIssue> ValidateAll()
        {
            foreach (IValidationIssue issue in Validate())
            {
                yield return issue;
            }
            foreach (IEAElement eaElement in elements)
            {
                foreach (IValidationIssue issue in eaElement.ValidateAll())
                {
                    yield return issue;
                }
            }
            foreach (IEAPackage eaPackage in subPackages)
            {
                foreach (IValidationIssue issue in eaPackage.ValidateAll())
                {
                    yield return issue;
                }
            }
        }

        public void AddElement(IEAElement element)
        {
            elements.Add(element);
            element.Package = this;
            NeedsValidation = true;
        }

        public void CopyChildren(IEAPackage package)
        {
            elements.AddRange(package.EAElements);
            subPackages.AddRange(package.SubPackages);
        }

        public void RemoveSubPackage(int id)
        {
            subPackages.RemoveAll(p => p.Id == id);
            NeedsValidation = true;
        }

        public void RemoveElement(int id)
        {
            elements.RemoveAll(element => element.Id == id);
            NeedsValidation = true;
        }

        #endregion
    }
}