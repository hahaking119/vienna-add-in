using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public class BusinessLibrary : IBusinessLibrary
    {
        private IList<string> businessTerms;
        private IList<string> copyrights;
        private IList<string> owners;
        private IList<string> references;

        public BusinessLibrary(BusinessLibraryType type, int id, string name)
        {
            Type = type;
            Id = id;
            Name = name;
        }

        #region IBusinessLibrary Members


        public bool IsA(BusinessLibraryType type)
        {
            return Type == type;
        }


        public int Id { get; private set; }
        BusinessLibraryType IBusinessLibrary.Type
        {
            get { return Type; }
//            set { Type = value; }
        }

        public string Name { get; set; }
        public IBusinessLibrary Parent
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IEnumerable<IBusinessLibrary> Children
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IEnumerable<IBusinessLibrary> AllChildren
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Status { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string BaseURN { get; set; }
        public string NamespacePrefix
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return businessTerms; }
            set { businessTerms = new List<string>(value).AsReadOnly(); }
        }

        public IEnumerable<string> Copyrights
        {
            get { return copyrights; }
            set { copyrights = new List<string>(value).AsReadOnly(); }
        }

        public IEnumerable<string> Owners
        {
            get { return owners; }
            set { owners = new List<string>(value).AsReadOnly(); }
        }

        public IEnumerable<string> References
        {
            get { return references; }
            set { references = new List<string>(value).AsReadOnly(); }
        }

        public BusinessLibraryType Type { get; private set; }

        #endregion
    }
}