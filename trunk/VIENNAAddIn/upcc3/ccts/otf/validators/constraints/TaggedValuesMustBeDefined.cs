using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
    public class TaggedValuesMustBeDefined<T> : SafeConstraint<T> where T : RepositoryItem
    {
        private readonly TaggedValues[] taggedValues;

        public TaggedValuesMustBeDefined(params TaggedValues[] taggedValues)
        {
            this.taggedValues = taggedValues;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(T item)
        {
            foreach (var taggedValue in taggedValues)
            {
                if (item.GetTaggedValue(taggedValue) == null)
                {
                    yield return new ConstraintViolation(item.Id, item.Id, "Tagged value " + taggedValue + " of " + item.Name + " must be defined.");
                }
            }
        }
    }
}