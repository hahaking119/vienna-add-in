// ReSharper disable RedundantUsingDirective
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.repo.BdtLibrary;
using VIENNAAddIn.upcc3.repo.BieLibrary;
using VIENNAAddIn.upcc3.repo.BLibrary;
using VIENNAAddIn.upcc3.repo.CcLibrary;
using VIENNAAddIn.upcc3.repo.CdtLibrary;
using VIENNAAddIn.upcc3.repo.DocLibrary;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddIn.upcc3.repo.PrimLibrary;
// ReSharper restore RedundantUsingDirective
using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo.EnumLibrary
{
    internal class UpccCodelistEntry : ICodelistEntry
    {
        public UpccCodelistEntry(IUmlEnumerationLiteral umlEnumerationLiteral, IEnum @enum)
        {
            UmlEnumerationLiteral = umlEnumerationLiteral;
			Enum = @enum;
        }

        public IUmlEnumerationLiteral UmlEnumerationLiteral { get; private set; }

        #region ICodelistEntry Members

        public int Id
        {
            get { return UmlEnumerationLiteral.Id; }
        }

        public string Name
        {
            get { return UmlEnumerationLiteral.Name; }
        }

        public IEnum Enum { get; private set; }


        ///<summary>
        /// Tagged value 'codeName'.
        ///</summary>
        public string CodeName
        {
            get { return UmlEnumerationLiteral.GetTaggedValue("codeName").Value; }
        }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
        public string Status
        {
            get { return UmlEnumerationLiteral.GetTaggedValue("status").Value; }
        }

		#endregion
    }
}

