using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public interface IRepositoryItemData
    {
        ItemId Id { get; }
        ItemId ParentId { get; }
        string Name { get; }
        string Stereotype { get; }
        string GetTaggedValue(TaggedValues key);
        IEnumerable<string> GetTaggedValues(TaggedValues key);
    }
}