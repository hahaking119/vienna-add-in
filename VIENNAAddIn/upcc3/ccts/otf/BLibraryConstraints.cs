using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class PRIMLibraryConstraints : BusinessLibraryConstraints
    {
        public PRIMLibraryConstraints()
        {
            AddConstraint(ParentMustBeABLibrary);
            AddConstraint(MustOnlyContainPRIMElements);
            AddConstraint(MustNotContaintAnyPackages);
        }

        private static IEnumerable<IValidationIssue> ParentMustBeABLibrary(IRepositoryItem item)
        {
            string parentStereotype = item.Parent.Data.Stereotype;
            if (!(Stereotype.BLibrary == parentStereotype))
            {
                yield return new InvalidParentPackage(item.Id, item.Data.Name);
            }
        }

        private static IEnumerable<IValidationIssue> MustOnlyContainPRIMElements(IRepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Element)
                {
                    if (child.Data.Stereotype != Stereotype.PRIM)
                    {
                        yield return new InvalidElementStereotype(item.Id, child.Id, item.Data.Name, child.Data.Name);
                    }
                }
            }
        }

        private static IEnumerable<IValidationIssue> MustNotContaintAnyPackages(IRepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Package)
                {
                    yield return new NoPackagesAllowed(item.Id, child.Id, item.Data.Name);
                }
            }
        }

        public override bool Matches(IRepositoryItem item)
        {
            return item != null && item.Data != null && Stereotype.PRIMLibrary == item.Data.Stereotype;
        }
    }
    public class BLibraryConstraints : BusinessLibraryConstraints
    {
        public BLibraryConstraints()
        {
            AddConstraint(MustNotContainAnyElements);
            AddConstraint(SubPackagesMustBeBusinessLibraries);
            AddConstraint(ParentMustBeModelOrBInformationVOrBLibrary);
        }

        private static IEnumerable<IValidationIssue> ParentMustBeModelOrBInformationVOrBLibrary(IRepositoryItem item)
        {
            string parentStereotype = item.Parent.Data.Stereotype;
            if (!(item.Parent.Data.ParentId.IsNull || Stereotype.BInformationV == parentStereotype || Stereotype.BLibrary == parentStereotype))
            {
                yield return new InvalidParentPackage(item.Id, item.Data.Name);
            }
        }

        private static IEnumerable<IValidationIssue> SubPackagesMustBeBusinessLibraries(IRepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Package && !(Stereotype.IsBusinessLibraryStereotype(child.Data.Stereotype)))
                {
                    yield return new InvalidSubPackageStereotype(item.Id, child.Id, item.Data.Name, child.Data.Name);
                }
            }
        }

        private static IEnumerable<IValidationIssue> MustNotContainAnyElements(IRepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Element)
                {
                    yield return new NoElementsAllowed(item.Id, child.Id, item.Data.Name);
                }
            }
        }

        public override bool Matches(IRepositoryItem item)
        {
            return item != null && item.Data != null && Stereotype.BLibrary == item.Data.Stereotype;
        }
    }

    public abstract class AggregateConstraints: IConstraint
    {
        private static readonly IValidationIssue[] NoIssues = new IValidationIssue[0];
        private readonly List<Constraint> constraints = new List<Constraint>();

        protected void AddConstraint(Constraint constraint)
        {
            constraints.Add(constraint);
        }

        public IEnumerable<IValidationIssue> Check(IRepositoryItem item)
        {
            if (item == null)
            {
                return NoIssues;
            }
            var issues = new List<IValidationIssue>();
            foreach (Constraint constraint in constraints)
            {
                issues.AddRange(constraint(item));
            }
            return issues;
        }

        protected static IEnumerable<IValidationIssue> TaggedValueMustNotBeEmpty(IRepositoryItem item, TaggedValues taggedValueKey)
        {
            if (string.IsNullOrEmpty(item.Data.GetTaggedValue(taggedValueKey)))
            {
                yield return new LibraryMandatoryTaggedValueNotSpecified(item.Id, item.Data.Name, taggedValueKey);
            }
        }

        #region Nested type: Constraint

        protected delegate IEnumerable<IValidationIssue> Constraint(IRepositoryItem item);

        #endregion

        public abstract bool Matches(IRepositoryItem item);
    }

    public abstract class BusinessLibraryConstraints : AggregateConstraints
    {
        protected BusinessLibraryConstraints()
        {
            AddConstraint(NameMustNotBeEmpty);
            AddConstraint(TaggedValueBaseURNMustNotBeEmpty);
            AddConstraint(TaggedValueUniqueIdentifierMustNotBeEmpty);
            AddConstraint(TaggedValueVersionIdentifierMustNotBeEmpty);
        }

        private static IEnumerable<IValidationIssue> TaggedValueBaseURNMustNotBeEmpty(IRepositoryItem item)
        {
            return TaggedValueMustNotBeEmpty(item, TaggedValues.baseURN);
        }

        private static IEnumerable<IValidationIssue> TaggedValueUniqueIdentifierMustNotBeEmpty(IRepositoryItem item)
        {
            return TaggedValueMustNotBeEmpty(item, TaggedValues.uniqueIdentifier);
        }

        private static IEnumerable<IValidationIssue> TaggedValueVersionIdentifierMustNotBeEmpty(IRepositoryItem item)
        {
            return TaggedValueMustNotBeEmpty(item, TaggedValues.versionIdentifier);
        }

        private static IEnumerable<IValidationIssue> NameMustNotBeEmpty(IRepositoryItem item)
        {
            if (string.IsNullOrEmpty(item.Data.Name))
            {
                yield return new LibraryNameNotSpecified(item.Id);
            }
        }
    }

    public interface IConstraint
    {
        bool Matches(IRepositoryItem item);
        IEnumerable<IValidationIssue> Check(IRepositoryItem item);
    }
}