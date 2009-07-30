namespace VIENNAAddIn.upcc3.ccts.otf
{
    public interface IValidationIssue
    {
        /// <summary>
        /// The ID of the item that was validated.
        /// </summary>
        ItemId ValidatedItemId { get; }

        /// <summary>
        /// The ID of the item that causes the issue. E.g. if a PRIMLibrary is validated and contains a non-PRIM element, 
        /// the ValidatedItemId is the library's ID, whereas the ItemId is the ID of the element.
        /// </summary>
        ItemId ItemId { get; }

        /// <summary>
        /// A unique ID of this issue.
        /// </summary>
        int Id { get; }

        string Message { get; }
    }
}