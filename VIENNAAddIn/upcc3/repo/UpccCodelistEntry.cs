using System;
using CctsRepository.EnumLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccCodelistEntry : ICodelistEntry
    {
        public IUmlEnumerationLiteral UmlEnumerationLiteral { get; set; }

        public UpccCodelistEntry(IUmlEnumerationLiteral umlEnumerationLiteral)
        {
            UmlEnumerationLiteral = umlEnumerationLiteral;
        }

        public int Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public IEnum Enum
        {
            get { throw new NotImplementedException(); }
        }

        public string CodeName
        {
            get { throw new NotImplementedException(); }
        }

        public string Status
        {
            get { throw new NotImplementedException(); }
        }
    }
}