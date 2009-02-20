using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public enum BusinessLibraryType
    {
        bLibrary,
        BDTLibrary,
        BIELibrary,
        CCLibrary,
        CDTLibrary,
        DOCLibrary,
        ENUMLibrary,
        PRIMLibrary
    }

    public interface IBusinessLibrary
    {
        int Id { get; }
        BusinessLibraryType Type { get; }
        string Name { get; }
        IBusinessLibrary Parent { get; }
        IList<IBusinessLibrary> Children { get; }
        string Status { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string BaseURN { get; }
        string NamespacePrefix { get; }
        IList<string> BusinessTerms { get; }
        IList<string> Copyrights { get; }
        IList<string> Owners { get; }
        IList<string> References { get; }
        T CreateElement<T>(T spec);
        IList<T> Elements<T>();
        bool IsA(BusinessLibraryType type);
    }

    public interface ICDTLibrary : IBusinessLibrary
    {
        IList<ICDT> CDTs { get; }
        void EachCDT(Action<ICDT> action);
    }

    public interface ICC
    {
        long Id { get; }
        string Name { get; set; }
        IBusinessLibrary Library { get; set; }
        string Definition { get; set; }
        string DictionaryEntryName { get; set; }
        string LanguageCode { get; set; }
        string UniqueIdentifier { get; set; }
        string VersionIdentifier { get; set; }
        IList<string> BusinessTerms { get; set; }
        IList<string> UsageRules { get; set; }
    }

    public interface IACC : ICC
    {
        IList<IBCC> BCCs { get; set; }
        IList<IASCC> ASCCs { get; set; }
        IACC IsEquivalentTo { get; set; }
    }

    public interface IASCC : ICC
    {
        string SequencingKey { get; set; }
        IACC AssociatedCC { get; set; }
    }

    public interface IBCC : ICC
    {
        string SequencingKey { get; set; }
        ICDT Type { get; set; }
    }
}