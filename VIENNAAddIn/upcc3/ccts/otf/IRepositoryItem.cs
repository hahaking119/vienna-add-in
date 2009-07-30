using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public interface IRepositoryItem
    {
        IEnumerable<RepositoryItem> Children { get; }
        IRepositoryItemData Data { get; set; }
        ItemId Id { get; }
        IRepositoryItem Parent { get; }
    }
}