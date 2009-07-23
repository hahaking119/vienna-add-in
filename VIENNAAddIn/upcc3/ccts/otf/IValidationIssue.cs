using EA;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public interface IValidationIssue
    {
        /// <summary>
        /// The ID of the item that was validated.
        /// </summary>
        int ValidatedItemId { get; }

        /// <summary>
        /// The ID of the item that causes the issue. E.g. if a PRIMLibrary is validated and contains a non-PRIM element, 
        /// the ValidatedItemId is the library's ID, whereas the ItemId is the ID of the element.
        /// </summary>
        int ItemId { get; }

        /// <summary>
        /// A unique ID of this issue.
        /// </summary>
        int Id { get; }

        string Message { get; }

        /// <summary>
        /// Retrieves the relevant item from the repository. This is implemented within the issue, because depending on the kind
        /// of item, different methods are used to retrieve it from the repository.
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        object ResolveItem(Repository repository);
    }
}