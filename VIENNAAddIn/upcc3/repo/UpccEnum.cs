// ReSharper disable RedundantUsingDirective
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
// ReSharper restore RedundantUsingDirective
using System.Collections.Generic;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccEnum : IEnum
    {
        public UpccEnum(IUmlEnumeration umlEnumeration)
        {
            UmlEnumeration = umlEnumeration;
        }

        public IUmlEnumeration UmlEnumeration { get; private set; }

        #region IEnum Members

        public int Id
        {
            get { return UmlEnumeration.Id; }
        }

        public string Name
        {
            get { return UmlEnumeration.Name; }
        }

		public IEnumLibrary EnumLibrary
        {
            get { return new UpccEnumLibrary(UmlEnumeration.Package); }
        }

		public IEnum IsEquivalentTo
        {
            get
            {
                var dependency = UmlEnumeration.GetFirstDependencyByStereotype("isEquivalentTo");
                return dependency == null ? null : new UpccEnum(dependency.Target);
            }
        }

		public IEnumerable<ICodelistEntry> CodelistEntries
        {
            get
            {
                foreach (var enumerationLiteral in UmlEnumeration.GetEnumerationLiteralsByStereotype("CodelistEntry"))
                {
                    yield return new UpccCodelistEntry(enumerationLiteral);
                }
            }
        }

		/// <summary>
		/// Creates a(n) CodelistEntry based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) CodelistEntry.</param>
		/// <returns>The newly created CodelistEntry.</returns>
		/// </summary>
		public ICodelistEntry CreateCodelistEntry(CodelistEntrySpec specification)
		{
		    return new UpccCodelistEntry(UmlEnumeration.CreateEnumerationLiteral(CodelistEntrySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a(n) CodelistEntry to match the given <paramref name="specification"/>.
		/// <param name="codelistEntry">A(n) CodelistEntry.</param>
		/// <param name="specification">A new specification for the given CodelistEntry.</param>
		/// <returns>The updated CodelistEntry. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public ICodelistEntry UpdateCodelistEntry(ICodelistEntry codelistEntry, CodelistEntrySpec specification)
		{
		    return new UpccCodelistEntry(UmlEnumeration.UpdateEnumerationLiteral(((UpccCodelistEntry) codelistEntry).UmlEnumerationLiteral, CodelistEntrySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a(n) CodelistEntry from this ENUM.
		/// <param name="codelistEntry">A(n) CodelistEntry.</param>
		/// </summary>
        public void RemoveCodelistEntry(ICodelistEntry codelistEntry)
		{
            UmlEnumeration.RemoveEnumerationLiteral(((UpccCodelistEntry) codelistEntry).UmlEnumerationLiteral);
		}

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return UmlEnumeration.GetTaggedValue("businessTerm").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'codeListAgencyIdentifier'.
        ///</summary>
        public string CodeListAgencyIdentifier
        {
            get { return UmlEnumeration.GetTaggedValue("codeListAgencyIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'codeListAgencyName'.
        ///</summary>
        public string CodeListAgencyName
        {
            get { return UmlEnumeration.GetTaggedValue("codeListAgencyName").Value; }
        }

        ///<summary>
        /// Tagged value 'codeListIdentifier'.
        ///</summary>
        public string CodeListIdentifier
        {
            get { return UmlEnumeration.GetTaggedValue("codeListIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'codeListName'.
        ///</summary>
        public string CodeListName
        {
            get { return UmlEnumeration.GetTaggedValue("codeListName").Value; }
        }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
        public string DictionaryEntryName
        {
            get { return UmlEnumeration.GetTaggedValue("dictionaryEntryName").Value; }
        }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
        public string Definition
        {
            get { return UmlEnumeration.GetTaggedValue("definition").Value; }
        }

        ///<summary>
        /// Tagged value 'enumerationURI'.
        ///</summary>
        public string EnumerationURI
        {
            get { return UmlEnumeration.GetTaggedValue("enumerationURI").Value; }
        }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
        public string LanguageCode
        {
            get { return UmlEnumeration.GetTaggedValue("languageCode").Value; }
        }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
        public string ModificationAllowedIndicator
        {
            get { return UmlEnumeration.GetTaggedValue("modificationAllowedIndicator").Value; }
        }

        ///<summary>
        /// Tagged value 'restrictedPrimitive'.
        ///</summary>
        public string RestrictedPrimitive
        {
            get { return UmlEnumeration.GetTaggedValue("restrictedPrimitive").Value; }
        }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
        public string Status
        {
            get { return UmlEnumeration.GetTaggedValue("status").Value; }
        }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
        public string UniqueIdentifier
        {
            get { return UmlEnumeration.GetTaggedValue("uniqueIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
        public string VersionIdentifier
        {
            get { return UmlEnumeration.GetTaggedValue("versionIdentifier").Value; }
        }

        #endregion
    }
}
