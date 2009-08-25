using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
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
}