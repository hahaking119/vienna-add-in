using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public interface IValidator
    {
        bool Matches(object item);
        IEnumerable<ConstraintViolation> Validate(object item);
    }

    public class ConstraintViolation : IEquatable<ConstraintViolation>
    {
        public ConstraintViolation(ItemId validatedItemId, ItemId offendingItemId, string message)
        {
            ValidatedItemId = validatedItemId;
            OffendingItemId = offendingItemId;
            Message = message;
        }

        /// <summary>
        /// The ID of the item that was validated.
        /// </summary>
        public ItemId ValidatedItemId { get; private set; }

        /// <summary>
        /// The ID of the item that actually violates the constraint. E.g. if a PRIMLibrary is validated and contains a non-PRIM element, 
        /// the ValidatedItemId is the library's ID, whereas the OffendingItemId is the ID of the element.
        /// </summary>
        public ItemId OffendingItemId { get; private set; }

        public string Message { get; private set; }

        public bool Equals(ConstraintViolation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.ValidatedItemId, ValidatedItemId) && Equals(other.OffendingItemId, OffendingItemId) && Equals(other.Message, Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ConstraintViolation)) return false;
            return Equals((ConstraintViolation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (ValidatedItemId != null ? ValidatedItemId.GetHashCode() : 0);
                result = (result*397) ^ (OffendingItemId != null ? OffendingItemId.GetHashCode() : 0);
                result = (result*397) ^ (Message != null ? Message.GetHashCode() : 0);
                return result;
            }
        }

        public static bool operator ==(ConstraintViolation left, ConstraintViolation right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ConstraintViolation left, ConstraintViolation right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return "Constraint violation: " + Message;
        }
    }

    /// <summary>
    /// A validator that matches non-null objects of a given type.
    /// 
    /// Sub-class this abstract class to implement type- and null-safe validators.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SafeValidator<T> : IValidator where T : class
    {
        public bool Matches(object item)
        {
            if (item != null && item is T)
            {
                return SafeMatches(item as T);
            }
            return false;
        }

        protected abstract bool SafeMatches(T item);

        public IEnumerable<ConstraintViolation> Validate(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "This indicates a programming error: Check() was called without checking that this constraint Matches().");
            }
            if (!(item is T))
            {
                throw new ArgumentException("Item is not of expected type. This indicates a programming error: Check() was called without checking that this constraint Matches().", "item");
            }
            return SafeValidate(item as T);
        }

        protected abstract IEnumerable<ConstraintViolation> SafeValidate(T item);
    }

    /// <summary>
    /// A constraint that matches non-null objects of a given type.
    /// 
    /// Sub-class this abstract class to implement type- and null-safe constraints.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SafeConstraint<T> : IConstraint where T : class
    {
        public IEnumerable<ConstraintViolation> Check(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "This indicates a programming error: Check() was called without checking that the validator matches the item.");
            }
            if (!(item is T))
            {
                throw new ArgumentException("Item is not of expected type. This indicates a programming error: Check() was called without checking that the validator matches the item.", "item");
            }
            return SafeCheck(item as T);
        }

        protected abstract IEnumerable<ConstraintViolation> SafeCheck(T item);
    }

    public abstract class ConstraintBasedValidator<T> : SafeValidator<T> where T : class
    {
        private readonly List<SafeConstraint<T>> constraints = new List<SafeConstraint<T>>();

        protected void AddConstraint(SafeConstraint<T> constraint)
        {
            constraints.Add(constraint);
        }

        protected override IEnumerable<ConstraintViolation> SafeValidate(T item)
        {
            foreach (var constraint in constraints)
            {
                foreach(var constraintViolation in constraint.Check(item))
                {
                    yield return constraintViolation;
                }
            }
        }
    }

    public class ElementLibaryValidator : BusinessLibraryValidator
    {
        public ElementLibaryValidator(string libraryStereotype, string elementStereotype):base(libraryStereotype)
        {
            AddConstraint(new ParentPackageMustHaveStereotype(Stereotype.bLibrary));
            AddConstraint(new ElementMustHaveStereotype(elementStereotype));
            AddConstraint(new MustNotContainAnyPackages());
        }        
    }

    public class BLibraryValidator : BusinessLibraryValidator
    {
        public BLibraryValidator():base(Stereotype.bLibrary)
        {
            AddConstraint(new ParentMustBeModelOrBInformationVOrBLibrary());
            AddConstraint(new SubPackagesMustBeBusinessLibraries());
            AddConstraint(new MustNotContainAnyElements());
        }
    }

    public abstract class BusinessLibraryValidator : ConstraintBasedValidator<IRepositoryItem>
    {
        private readonly string stereotype;

        protected BusinessLibraryValidator(string stereotype)
        {
            this.stereotype = stereotype;
            AddConstraint(new NameMustNotBeEmpty());
            AddConstraint(new TaggedValueMustBeDefined(TaggedValues.baseURN));
            AddConstraint(new TaggedValueMustBeDefined(TaggedValues.uniqueIdentifier));
            AddConstraint(new TaggedValueMustBeDefined(TaggedValues.versionIdentifier));
        }

        protected override bool SafeMatches(IRepositoryItem item)
        {
            return item.Data != null && item.Data.Stereotype == stereotype;
        }
    }

    public class NameMustNotBeEmpty : SafeConstraint<IRepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
        {
            if (string.IsNullOrEmpty(item.Data.Name))
            {
                yield return new ConstraintViolation(item.Id, item.Id, "The name of library " + item.Id + " is not specified.");
            }
        }
    }

    public class TaggedValueMustBeDefined : SafeConstraint<IRepositoryItem>
    {
        private readonly TaggedValues taggedValueKey;

        public TaggedValueMustBeDefined(TaggedValues taggedValueKey)
        {
            this.taggedValueKey = taggedValueKey;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
        {
            if (string.IsNullOrEmpty(item.Data.GetTaggedValue(taggedValueKey)))
            {
                yield return new ConstraintViolation(item.Id, item.Id, "Tagged value " + taggedValueKey + " of " + item.Data.Name + " is not defined.");
            }
        }
    }

    public class ParentPackageMustHaveStereotype: SafeConstraint<IRepositoryItem>
    {
        private readonly string stereotype;

        public ParentPackageMustHaveStereotype(string stereotype)
        {
            this.stereotype = stereotype;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
        {
            if (stereotype != item.Parent.Data.Stereotype)
            {
                yield return new ConstraintViolation(item.Id, item.Id, "The parent package of " + item.Data.Name + " must have stereotype " + stereotype + ".");
            }
        }
    }

    public class ElementMustHaveStereotype:SafeConstraint<IRepositoryItem>
    {
        private readonly string stereotype;

        public ElementMustHaveStereotype(string stereotype)
        {
            this.stereotype = stereotype;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Element)
                {
                    if (child.Data.Stereotype != Stereotype.PRIM)
                    {
                        yield return new ConstraintViolation(item.Id, child.Id, "Elements of " + item.Data.Name + " must have stereotype " + stereotype + ".");
                    }
                }
            }
        }
    }

    public class MustNotContainAnyPackages: SafeConstraint<IRepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Package)
                {
                    yield return new ConstraintViolation(item.Id, child.Id, item.Data.Name + " must not contain any packages.");
                }
            }
        }
    }

    public class MustNotContainAnyElements: SafeConstraint<IRepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Element)
                {
                    yield return new ConstraintViolation(item.Id, child.Id, item.Data.Name + " must not contain any elements.");
                }
            }
        }
    }

    public class SubPackagesMustBeBusinessLibraries:SafeConstraint<IRepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Package && !(Stereotype.IsBusinessLibraryStereotype(child.Data.Stereotype)))
                {
                    yield return new ConstraintViolation(item.Id, child.Id, "Sub-packages of " + item.Data.Name + " must have a UPCC business library stereotype.");
                }
            }
        }
    }

        public class ParentMustBeModelOrBInformationVOrBLibrary:SafeConstraint<IRepositoryItem>
        {
            protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
            {
                string parentStereotype = item.Parent.Data.Stereotype;
                if (!(item.Parent.Data.ParentId.IsNull || Stereotype.BInformationV == parentStereotype || Stereotype.bLibrary == parentStereotype))
                {
                    yield return new ConstraintViolation(item.Id, item.Id, 
                        "The parent package of " + item.Data.Name + 
                        " must be a model"+
                        " or have stereotype " + Stereotype.BInformationV +
                        " or have stereotype " + Stereotype.bLibrary + 
                        ".");
                }
            }
        }

public interface IConstraint
    {
        IEnumerable<ConstraintViolation> Check(object item);
    }
}