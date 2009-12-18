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
using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal partial class UpccDocLibrary : IDocLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccDocLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region IDocLibrary Members

		/// <summary>
		/// The DOCLibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return umlPackage.Id; }
        }

		/// <summary>
		/// The DOCLibrary's name.
		/// </summary>
        public string Name
        {
            get { return umlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this DOCLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(umlPackage.Parent); }
        }

		/// <summary>
		/// The MAs contained in this DOCLibrary.
		/// </summary>
		public IEnumerable<IMa> Mas
		{
            get
            {
                foreach (var umlclass in umlPackage.Classes)
                {
                    yield return new UpccMa(umlclass);
                }
            }
		}

		/// <summary>
		/// Retrieves a MA by name.
		/// <param name="name">A MA's name.</param>
		/// <returns>The MA with the given <paramref name="name"/> or <c>null</c> if no such MA is found.</returns>
		/// </summary>
        public IMa GetMaByName(string name)
		{
            foreach (IMa ma in Mas)
            {
                if (ma.Name == name)
                {
                    return ma;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a MA based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a MA.</param>
		/// <returns>The newly created MA.</returns>
		/// </summary>
		public IMa CreateMa(MaSpec specification)
		{
		    return new UpccMa(umlPackage.CreateClass(MaSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a MA to match the given <paramref name="specification"/>.
		/// <param name="ma">A MA.</param>
		/// <param name="specification">A new specification for the given MA.</param>
		/// <returns>The updated MA. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IMa UpdateMa(IMa ma, MaSpec specification)
		{
		    return new UpccMa(umlPackage.UpdateClass(((UpccMa) ma).UmlClass, MaSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a MA from this DOCLibrary.
		/// <param name="ma">A MA.</param>
		/// </summary>
        public void RemoveMa(IMa ma)
		{
            umlPackage.RemoveClass(((UpccMa) ma).UmlClass);
		}

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.businessTerm).SplitValues; }
        }

        ///<summary>
        /// Tagged value 'copyright'.
        ///</summary>
        public IEnumerable<string> Copyrights
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.copyright).SplitValues; }
        }

        ///<summary>
        /// Tagged value 'owner'.
        ///</summary>
        public IEnumerable<string> Owners
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.owner).SplitValues; }
        }

        ///<summary>
        /// Tagged value 'reference'.
        ///</summary>
        public IEnumerable<string> References
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.reference).SplitValues; }
        }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
        public string Status
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.status).Value; }
        }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
        public string UniqueIdentifier
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.uniqueIdentifier).Value; }
        }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
        public string VersionIdentifier
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.versionIdentifier).Value; }
        }

        ///<summary>
        /// Tagged value 'baseURN'.
        ///</summary>
        public string BaseURN
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.baseURN).Value; }
        }

        ///<summary>
        /// Tagged value 'namespacePrefix'.
        ///</summary>
        public string NamespacePrefix
        {
            get { return umlPackage.GetTaggedValue(TaggedValues.namespacePrefix).Value; }
        }

        #endregion
	}
}
