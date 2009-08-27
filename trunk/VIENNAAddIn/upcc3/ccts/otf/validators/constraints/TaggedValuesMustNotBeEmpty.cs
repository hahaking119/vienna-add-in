using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
    public class TaggedValuesMustNotBeEmpty<T> : SafeConstraint<T> where T : RepositoryItem
    {
        private readonly TaggedValues[] taggedValues;

        public TaggedValuesMustNotBeEmpty(params TaggedValues[] taggedValues)
        {
            this.taggedValues = taggedValues;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(T item)
        {
            foreach (var taggedValue in taggedValues)
            {
                if (string.IsNullOrEmpty(item.GetTaggedValue(taggedValue)))
                {
                    yield return new ConstraintViolation(item.Id, item.Id, "Tagged value " + taggedValue + " of " + item.Name + " must not be empty.");
                }
            }
        }
    }
}