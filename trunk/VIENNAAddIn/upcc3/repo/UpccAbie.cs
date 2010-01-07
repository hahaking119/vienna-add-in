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
    internal class UpccAbie : IAbie
    {
        public UpccAbie(IUmlClass umlClass)
        {
            UmlClass = umlClass;
        }

        public IUmlClass UmlClass { get; private set; }

        #region IAbie Members

        public int Id
        {
            get { return UmlClass.Id; }
        }

        public string Name
        {
            get { return UmlClass.Name; }
        }

		public IBieLibrary BieLibrary
        {
            get { return new UpccBieLibrary(UmlClass.Package); }
        }

		public IAbie IsEquivalentTo
        {
            get
            {
                var dependency = UmlClass.GetFirstDependencyByStereotype("isEquivalentTo");
                return dependency == null ? null : new UpccAbie(dependency.Target);
            }
        }

		public IAcc BasedOn
        {
            get
            {
                var dependency = UmlClass.GetFirstDependencyByStereotype("basedOn");
                return dependency == null ? null : new UpccAcc(dependency.Target);
            }
        }

		public IEnumerable<IBbie> Bbies
        {
            get
            {
                foreach (var attribute in UmlClass.GetAttributesByStereotype("BBIE"))
                {
                    yield return new UpccBbie(attribute);
                }
            }
        }

		/// <summary>
		/// Creates a(n) BBIE based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) BBIE.</param>
		/// <returns>The newly created BBIE.</returns>
		/// </summary>
		public IBbie CreateBbie(BbieSpec specification)
		{
		    return new UpccBbie(UmlClass.CreateAttribute(BbieSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a(n) BBIE to match the given <paramref name="specification"/>.
		/// <param name="bbie">A(n) BBIE.</param>
		/// <param name="specification">A new specification for the given BBIE.</param>
		/// <returns>The updated BBIE. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBbie UpdateBbie(IBbie bbie, BbieSpec specification)
		{
		    return new UpccBbie(UmlClass.UpdateAttribute(((UpccBbie) bbie).UmlAttribute, BbieSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a(n) BBIE from this ABIE.
		/// <param name="bbie">A(n) BBIE.</param>
		/// </summary>
        public void RemoveBbie(IBbie bbie)
		{
            UmlClass.RemoveAttribute(((UpccBbie) bbie).UmlAttribute);
		}

		public IEnumerable<IAsbie> Asbies
        {
            get
            {
                foreach (var association in UmlClass.GetAssociationsByStereotype("ASBIE"))
                {
                    yield return new UpccAsbie(association);
                }
            }
        }

		/// <summary>
		/// Creates a(n) ASBIE based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) ASBIE.</param>
		/// <returns>The newly created ASBIE.</returns>
		/// </summary>
		public IAsbie CreateAsbie(AsbieSpec specification)
		{
		    return new UpccAsbie(UmlClass.CreateAssociation(AsbieSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a(n) ASBIE to match the given <paramref name="specification"/>.
		/// <param name="asbie">A(n) ASBIE.</param>
		/// <param name="specification">A new specification for the given ASBIE.</param>
		/// <returns>The updated ASBIE. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IAsbie UpdateAsbie(IAsbie asbie, AsbieSpec specification)
		{
		    return new UpccAsbie(UmlClass.UpdateAssociation(((UpccAsbie) asbie).UmlAssociation, AsbieSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a(n) ASBIE from this ABIE.
		/// <param name="asbie">A(n) ASBIE.</param>
		/// </summary>
        public void RemoveAsbie(IAsbie asbie)
		{
            UmlClass.RemoveAssociation(((UpccAsbie) asbie).UmlAssociation);
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
