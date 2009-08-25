namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class IValidationIssue
    {
        private readonly ConstraintViolation constraintViolation;

        public IValidationIssue(int id, ConstraintViolation constraintViolation)
        {
            Id = id;
            this.constraintViolation = constraintViolation;
        }

        /// <summary>
        /// The ID of the item that was validated.
        /// </summary>
        public ItemId ValidatedItemId
        {
            get { return constraintViolation.ValidatedItemId; }
        }

        /// <summary>
        /// The ID of the item that causes the issue. E.g. if a PRIMLibrary is validated and contains a non-PRIM element, 
        /// the ValidatedItemId is the library's ID, whereas the ItemId is the ID of the element.
        /// </summary>
        public ItemId ItemId
        {
            get { return constraintViolation.OffendingItemId; }
        }

        /// <summary>
        /// A unique ID of this issue.
        /// </summary>
        public int Id { get; private set; }

        public string Message
        {
            get { return constraintViolation.Message; }
        }
    }
}