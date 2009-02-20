using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
//    public enum DataTypeType
//    {
//        CDT, BDT
//    }

    public interface IDT
    {
        int Id { get; }
//        DataTypeType Type { get; }
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

    public interface ICDT : IDT
    {
    }

    public class CDTSpec: ICDT
    {
        public int Id
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IBusinessLibrary Library
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Definition
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string DictionaryEntryName
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string LanguageCode
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string UniqueIdentifier
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string VersionIdentifier
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IList<string> BusinessTerms
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IList<string> UsageRules
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IList<IDTComponent> SUPs
        {
            get { throw new System.NotImplementedException(); }
        }

        public IDTComponent CON
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }

    public interface IBDT : IDT
    {
        ICDT BasedOn { get; }
    }
}