using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class InvalidSupplierForIsEquivalentTo : ValidationIssue
    {
        public string SupplierStereotype { get; private set; }

        public InvalidSupplierForIsEquivalentTo(string guid, string supplierStereotype) : base(guid)
        {
            SupplierStereotype = supplierStereotype;
        }

        public override object GetItem(Repository repository)
        {
            return repository.GetElementByGuid(GUID);
        }
    }
    public class ElementIsSourceAndTargetOfIsEquivalentTo : ValidationIssue
    {
        public ElementIsSourceAndTargetOfIsEquivalentTo(string guid) : base(guid)
        {
        }

        public override object GetItem(Repository repository)
        {
            return repository.GetElementByGuid(GUID);
        }
    }
}