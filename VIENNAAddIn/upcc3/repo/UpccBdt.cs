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
// ReSharper restore RedundantUsingDirective
using System.Collections.Generic;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccBdt : IBdt
    {
        public UpccBdt(IUmlClass umlClass)
        {
            UmlClass = umlClass;
        }

        public IUmlClass UmlClass { get; private set; }

        #region IBdt Members

        public int Id
        {
            get { return UmlClass.Id; }
        }

        public string Name
        {
            get { return UmlClass.Name; }
        }

		public IBdtLibrary BdtLibrary
        {
            get { return new UpccBdtLibrary(UmlClass.Package); }
        }

		public IBdt IsEquivalentTo
        {
            get
            {
                var dependency = UmlClass.GetFirstDependencyByStereotype("isEquivalentTo");
                return dependency == null ? null : new UpccBdt(dependency.Target);
            }
        }

		public ICdt BasedOn
        {
            get
            {
                var dependency = UmlClass.GetFirstDependencyByStereotype("basedOn");
                return dependency == null ? null : new UpccCdt(dependency.Target);
            }
        }

		public IBdtCon Con
        {
            get
            {
                var attribute = UmlClass.GetFirstAttributeByStereotype("CON");
                return attribute == null ? null : new UpccBdtCon(attribute, this);
            }
        }

		public IEnumerable<IBdtSup> Sups
        {
            get
            {
                foreach (var attribute in UmlClass.GetAttributesByStereotype("SUP"))
                {
                    yield return new UpccBdtSup(attribute, this);
                }
            }
        }

		/// <summary>
		/// Creates a(n) SUP based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) SUP.</param>
		/// <returns>The newly created SUP.</returns>
		/// </summary>
		public IBdtSup CreateBdtSup(BdtSupSpec specification)
		{
		    return new UpccBdtSup(UmlClass.CreateAttribute(BdtSupSpecConverter.Convert(specification)), this);
		}

		/// <summary>
		/// Updates a(n) SUP to match the given <paramref name="specification"/>.
		/// <param name="bdtSup">A(n) SUP.</param>
		/// <param name="specification">A new specification for the given SUP.</param>
		/// <returns>The updated SUP. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBdtSup UpdateBdtSup(IBdtSup bdtSup, BdtSupSpec specification)
		{
		    return new UpccBdtSup(UmlClass.UpdateAttribute(((UpccBdtSup) bdtSup).UmlAttribute, BdtSupSpecConverter.Convert(specification)), this);
		}

		/// <summary>
		/// Removes a(n) SUP from this BDT.
		/// <param name="bdtSup">A(n) SUP.</param>
		/// </summary>
        public void RemoveBdtSup(IBdtSup bdtSup)
		{
            UmlClass.RemoveAttribute(((UpccBdtSup) bdtSup).UmlAttribute);
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
