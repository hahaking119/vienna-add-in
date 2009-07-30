using EA;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public abstract class AbstractValidationIssue : IValidationIssue
    {
        private static int NextId;

        protected AbstractValidationIssue(ItemId validatedItemId, ItemId itemId)
        {
            Id = NextId++;
            ValidatedItemId = validatedItemId;
            ItemId = itemId;
        }

        #region IValidationIssue Members

        public ItemId ValidatedItemId { get; private set; }
        public ItemId ItemId { get; private set; }
        public int Id { get; private set; }
        public abstract string Message { get; }

        #endregion

        public override string ToString()
        {
            return "ValidationIssue[Type=" + GetType().Name + ", ItemId=" + ItemId + "]";
        }
    }
}