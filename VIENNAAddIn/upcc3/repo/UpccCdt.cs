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
    internal class UpccCdt : ICdt
    {
        public UpccCdt(IUmlClass umlClass)
        {
            UmlClass = umlClass;
        }

        public IUmlClass UmlClass { get; private set; }

        #region ICdt Members

        public int Id
        {
            get { return UmlClass.Id; }
        }

        public string Name
        {
            get { return UmlClass.Name; }
        }

		public ICdtLibrary CdtLibrary
        {
            get { return new UpccCdtLibrary(UmlClass.Package); }
        }

		public ICdt IsEquivalentTo
        {
            get
            {
                var dependency = UmlClass.GetFirstDependencyByStereotype("isEquivalentTo");
                return dependency == null ? null : new UpccCdt(dependency.Target);
            }
        }

		public ICdtCon Con
        {
            get
            {
                var attribute = UmlClass.GetFirstAttributeByStereotype("CON");
                return attribute == null ? null : new UpccCdtCon(attribute);
            }
        }

		public IEnumerable<ICdtSup> Sups
        {
            get
            {
                foreach (var attribute in UmlClass.GetAttributesByStereotype("SUP"))
                {
                    yield return new UpccCdtSup(attribute);
                }
            }
        }

		/// <summary>
		/// Creates a(n) SUP based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) SUP.</param>
		/// <returns>The newly created SUP.</returns>
		/// </summary>
		public ICdtSup CreateCdtSup(CdtSupSpec specification)
		{
		    return new UpccCdtSup(UmlClass.CreateAttribute(CdtSupSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a(n) SUP to match the given <paramref name="specification"/>.
		/// <param name="cdtSup">A(n) SUP.</param>
		/// <param name="specification">A new specification for the given SUP.</param>
		/// <returns>The updated SUP. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public ICdtSup UpdateCdtSup(ICdtSup cdtSup, CdtSupSpec specification)
		{
		    return new UpccCdtSup(UmlClass.UpdateAttribute(((UpccCdtSup) cdtSup).UmlAttribute, CdtSupSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a(n) SUP from this CDT.
		/// <param name="cdtSup">A(n) SUP.</param>
		/// </summary>
        public void RemoveCdtSup(ICdtSup cdtSup)
		{
            UmlClass.RemoveAttribute(((UpccCdtSup) cdtSup).UmlAttribute);
		}

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return UmlClass.GetTaggedValue("businessTerm").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
        public string Definition
        {
            get { return UmlClass.GetTaggedValue("definition").Value; }
        }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
        public string DictionaryEntryName
        {
            get { return UmlClass.GetTaggedValue("dictionaryEntryName").Value; }
        }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
        public string LanguageCode
        {
            get { return UmlClass.GetTaggedValue("languageCode").Value; }
        }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
        public string UniqueIdentifier
        {
            get { return UmlClass.GetTaggedValue("uniqueIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
        public string VersionIdentifier
        {
            get { return UmlClass.GetTaggedValue("versionIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'usageRule'.
        ///</summary>
        public IEnumerable<string> UsageRules
        {
            get { return UmlClass.GetTaggedValue("usageRule").SplitValues; }
        }

        #endregion
    }
}
