using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IDT
    {
        int Id { get; }
        string Name { get; }
        IBusinessLibrary Library { get; }
        string Definition { get; }
        string DictionaryEntryName { get; }
        string LanguageCode { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        IList<string> BusinessTerms { get; }
        IList<string> UsageRules { get; }
        IList<IDTComponent> SUPs { get; }
        IDTComponent CON { get; }
    }
}