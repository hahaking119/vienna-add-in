using EA;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public abstract class AbstractValidationIssue : IValidationIssue
    {
        private static int nextId;

        protected AbstractValidationIssue(int validatedItemId, int itemId)
        {
            Id = nextId++;
            ValidatedItemId = validatedItemId;
            ItemId = itemId;
        }

        #region IValidationIssue Members

        public int ValidatedItemId { get; private set; }
        public int ItemId { get; private set; }
        public int Id { get; private set; }
        public abstract string Message { get; }
        public abstract object ResolveItem(Repository repository);

        #endregion

        public override string ToString()
        {
            return "ValidationIssue[Type=" + GetType().Name + ", ItemId=" + ItemId + "]";
        }
    }
}