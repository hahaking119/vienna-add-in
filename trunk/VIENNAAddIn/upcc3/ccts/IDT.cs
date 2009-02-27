using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IDT : IElement
    {
        IBusinessLibrary Library { get; }
        string Definition { get; }
        string DictionaryEntryName { get; }
        string LanguageCode { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        IEnumerable<string> BusinessTerms { get; }
        IEnumerable<string> UsageRules { get; }
        IEnumerable<IDTComponent> SUPs { get; }
        IDTComponent CON { get; }
    }
}