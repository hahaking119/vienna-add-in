using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
    public class TaggedValueMustBeDefined : SafeConstraint<RepositoryItem>
    {
        private readonly TaggedValues taggedValueKey;

        public TaggedValueMustBeDefined(TaggedValues taggedValueKey)
        {
            this.taggedValueKey = taggedValueKey;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            if (string.IsNullOrEmpty(item.GetTaggedValue(taggedValueKey)))
            {
                yield return new ConstraintViolation(item.Id, item.Id, "Tagged value " + taggedValueKey + " of " + item.Name + " is not defined.");
            }
        }
    }
}