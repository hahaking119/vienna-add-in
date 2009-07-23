using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public abstract class AbstractEAElement : AbstractEAItem, IEAElement
    {
        #region IEAElement Members

        protected AbstractEAElement(int id, string name, int packageId) : base(id, name)
        {
            PackageId = packageId;
        }

        public int PackageId { get; set; }
        public IEAPackage Package { get; set; }

        public override IEnumerable<IValidationIssue> ValidateAll()
        {
            return Validate();
        }

        #endregion
    }
}