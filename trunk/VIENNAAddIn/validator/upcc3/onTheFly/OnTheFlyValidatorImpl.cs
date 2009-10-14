using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EA;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;
using Attribute=System.Attribute;
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
                library = new ValidatingPRIMLibrary(null, validationContext);
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
        private readonly Repository repository;
        private readonly IValidationContext validationContext;
        private readonly List<IPRIM> prims = new List<IPRIM>();

        public ValidatingPRIMLibrary(Repository repository, IValidationContext validationContext)
        {
            this.repository = repository;
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
            ValidateIsEquivalentToDependencies(element);
            prims.Add(new ValidatingPRIM(element));
        }

        private void ValidateIsEquivalentToDependencies(Element element)
        {
            var connectors = element.Connectors;
            if (connectors != null)
            {
                bool isClient = false;
                bool isSupplier = false;
                foreach (Connector connector in connectors)
                {
                    if (connector.IsIsEquivalentTo())
                    {
                        if (connector.ClientID == element.ElementID)
                        {
                            isClient = true;
                            var supplier = repository.GetElementByID(connector.SupplierID);
                            var supplierStereotype = supplier.Stereotype;
                            if (supplierStereotype != Stereotype.PRIM)
                            {
                                validationContext.AddValidationIssue(new InvalidSupplierForIsEquivalentTo(element.ElementGUID, supplierStereotype));
                            }
                        }
                        else if (connector.SupplierID == element.ElementID)
                        {
                            isSupplier = true;
                        }
                    }
                }
                if (isClient && isSupplier)
                {
                    validationContext.AddValidationIssue(new ElementIsSourceAndTargetOfIsEquivalentTo(element.ElementGUID));
                }
            }
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

    public class ValidatingPRIM : IPRIM
    {
        private readonly Element element;
        private readonly string dictionaryEntryName;

        private static List<TaggedValueProperty> taggedValueProperties;

        static ValidatingPRIM()
        {
            Type type = typeof(ValidatingPRIM);
            taggedValueProperties = new List<TaggedValueProperty>();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] attributes = property.GetCustomAttributes(typeof(TaggedValueAttribute), true);
                if (attributes.Length > 0)
                {
                    var attribute = (TaggedValueAttribute)attributes[0];
                    TaggedValues taggedValue = DetermineTaggedValue(property, attribute);
                    if (property.PropertyType == typeof(string))
                    {
                        taggedValueProperties.Add(new StringTaggedValueProperty(taggedValue, property));
                    }
                    else if (property.PropertyType == typeof(IEnumerable<string>))
                    {
                        taggedValueProperties.Add(new MultiStringTaggedValueProperty(taggedValue, property));
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        taggedValueProperties.Add(new BooleanTaggedValueProperty(taggedValue, property));
                    }
                }
            }
        }

        private static TaggedValues DetermineTaggedValue(PropertyInfo property, TaggedValueAttribute attribute)
        {
            TaggedValues taggedValue = attribute.Key;
            if (taggedValue == TaggedValues.undefined)
            {
                taggedValue = GetTaggedValue(property.Name);
            }
            if (taggedValue == TaggedValues.undefined && property.Name.EndsWith("s"))
            {
                taggedValue = GetTaggedValue(property.Name.Substring(0, property.Name.Length - 1));
            }
            if (taggedValue == TaggedValues.undefined)
            {
                throw new Exception("cannot determine tagged value of property " + property.DeclaringType.Name + "." + property.Name);
            }
            return taggedValue;
        }

        private static TaggedValues GetTaggedValue(string propertyName)
        {
            TaggedValues taggedValue;
            try
            {
                taggedValue = (TaggedValues)Enum.Parse(typeof(TaggedValues), propertyName, true);
            }
            catch (ArgumentException)
            {
                taggedValue = TaggedValues.undefined;
            }
            return taggedValue;
        }

        public ValidatingPRIM(Element element)
        {
            this.element = element;
            foreach (var taggedValueProperty in taggedValueProperties)
            {
                taggedValueProperty.LoadFromElement(this, element);
            }
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

        [OptionalTaggedValue]
        public string DictionaryEntryName
        {
            get
            {
                string value = dictionaryEntryName;
                if (string.IsNullOrEmpty(value))
                {
                    value = Name;
                }
                return value;
            }
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

    internal class BooleanTaggedValueProperty : TaggedValueProperty
    {
        public BooleanTaggedValueProperty(TaggedValues key, PropertyInfo property) : base(key, property)
        {
        }

        protected override object GetValue(TaggedValue tv)
        {
            return "true" == tv.Value.DefaultTo("true").ToLower();
        }
    }

    internal class MultiStringTaggedValueProperty : TaggedValueProperty
    {
        public MultiStringTaggedValueProperty(TaggedValues key, PropertyInfo property) : base(key, property)
        {
        }

        protected override object GetValue(TaggedValue tv)
        {
            var value = tv.Value;
            return String.IsNullOrEmpty(value) ? new string[0] : value.Split('|');
        }
    }

    internal class StringTaggedValueProperty : TaggedValueProperty
    {
        public StringTaggedValueProperty(TaggedValues key, PropertyInfo property) : base(key, property)
        {
        }

        protected override object GetValue(TaggedValue tv)
        {
            return tv.Value ?? string.Empty;
        }
    }

    internal abstract class TaggedValueProperty
    {
        private readonly TaggedValues key;
        private readonly PropertyInfo property;

        public TaggedValueProperty(TaggedValues key, PropertyInfo property)
        {
            this.key = key;
            this.property = property;
        }

        public void LoadFromElement(object obj, Element element)
        {
            foreach (TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name.Equals(key.ToString()))
                {
                    property.SetValue(obj, GetValue(tv), null);
                    return;
                }
            }
        }

        protected abstract object GetValue(TaggedValue tv);
    }

    public class DefaultAttribute : Attribute
    {
        public DefaultAttribute(Func<string> getDefaultValue)
        {
            throw new NotImplementedException();
        }
    }

    public class OptionalTaggedValueAttribute : Attribute
    {
    }
}