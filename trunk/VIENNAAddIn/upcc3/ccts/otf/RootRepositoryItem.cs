using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class RootRepositoryItem : RepositoryItem
    {
        public RootRepositoryItem() : base(
            ItemId.Null,
            ItemId.Null,
            "root",
            string.Empty,
            new Dictionary<string, string>())
        {
        }
    }
}