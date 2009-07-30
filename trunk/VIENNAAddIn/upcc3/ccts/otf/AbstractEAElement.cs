using System;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public abstract class AbstractEAElement : AbstractEAItem, IEAElement
    {
        protected AbstractEAElement(ItemId id, string name, ItemId packageId) : base(id, name)
        {
            PackageId = packageId;
        }

        #region IEAElement Members

        public ItemId PackageId { get; private set; }
        public IEAPackage Package { get; set; }

        #endregion
    }
}