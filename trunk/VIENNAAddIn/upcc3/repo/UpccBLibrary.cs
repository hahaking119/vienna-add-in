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
    internal class UpccBLibrary : IBLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccBLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region IBLibrary Members

		/// <summary>
		/// The bLibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return umlPackage.Id; }
        }

		/// <summary>
		/// The bLibrary's name.
		/// </summary>
        public string Name
        {
            get { return umlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this bLibrary.
		/// </summary>
		public IBLibrary Parent
        {
            get { return new UpccBLibrary(umlPackage.Parent); }
        }

		/// <summary>
		/// The bLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<IBLibrary> GetBLibraries()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a bLibrary by name.
		/// <param name="name">A bLibrary's name.</param>
		/// <returns>The bLibrary with the given <paramref name="name"/> or <c>null</c> if no such bLibrary is found.</returns>
		/// </summary>
        public IBLibrary GetBLibraryByName(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a bLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a bLibrary.</param>
		/// <returns>The newly created bLibrary.</returns>
		/// </summary>
		public IBLibrary CreateBLibrary(BLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a bLibrary to match the given <paramref name="specification"/>.
		/// <param name="bLibrary">A bLibrary.</param>
		/// <param name="specification">A new specification for the given bLibrary.</param>
		/// <returns>The updated bLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBLibrary UpdateBLibrary(IBLibrary bLibrary, BLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a bLibrary from this bLibrary.
		/// <param name="bLibrary">A bLibrary.</param>
		/// </summary>
        public void RemoveBLibrary(IBLibrary bLibrary)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// The PRIMLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<IPrimLibrary> GetPrimLibraries()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a PRIMLibrary by name.
		/// <param name="name">A PRIMLibrary's name.</param>
		/// <returns>The PRIMLibrary with the given <paramref name="name"/> or <c>null</c> if no such PRIMLibrary is found.</returns>
		/// </summary>
        public IPrimLibrary GetPrimLibraryByName(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a PRIMLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a PRIMLibrary.</param>
		/// <returns>The newly created PRIMLibrary.</returns>
		/// </summary>
		public IPrimLibrary CreatePrimLibrary(PrimLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a PRIMLibrary to match the given <paramref name="specification"/>.
		/// <param name="primLibrary">A PRIMLibrary.</param>
		/// <param name="specification">A new specification for the given PRIMLibrary.</param>
		/// <returns>The updated PRIMLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IPrimLibrary UpdatePrimLibrary(IPrimLibrary primLibrary, PrimLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a PRIMLibrary from this bLibrary.
		/// <param name="primLibrary">A PRIMLibrary.</param>
		/// </summary>
        public void RemovePrimLibrary(IPrimLibrary primLibrary)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// The ENUMLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<IEnumLibrary> GetEnumLibraries()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a ENUMLibrary by name.
		/// <param name="name">A ENUMLibrary's name.</param>
		/// <returns>The ENUMLibrary with the given <paramref name="name"/> or <c>null</c> if no such ENUMLibrary is found.</returns>
		/// </summary>
        public IEnumLibrary GetEnumLibraryByName(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a ENUMLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a ENUMLibrary.</param>
		/// <returns>The newly created ENUMLibrary.</returns>
		/// </summary>
		public IEnumLibrary CreateEnumLibrary(EnumLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a ENUMLibrary to match the given <paramref name="specification"/>.
		/// <param name="enumLibrary">A ENUMLibrary.</param>
		/// <param name="specification">A new specification for the given ENUMLibrary.</param>
		/// <returns>The updated ENUMLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IEnumLibrary UpdateEnumLibrary(IEnumLibrary enumLibrary, EnumLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a ENUMLibrary from this bLibrary.
		/// <param name="enumLibrary">A ENUMLibrary.</param>
		/// </summary>
        public void RemoveEnumLibrary(IEnumLibrary enumLibrary)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// The CDTLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<ICdtLibrary> GetCdtLibraries()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a CDTLibrary by name.
		/// <param name="name">A CDTLibrary's name.</param>
		/// <returns>The CDTLibrary with the given <paramref name="name"/> or <c>null</c> if no such CDTLibrary is found.</returns>
		/// </summary>
        public ICdtLibrary GetCdtLibraryByName(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a CDTLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a CDTLibrary.</param>
		/// <returns>The newly created CDTLibrary.</returns>
		/// </summary>
		public ICdtLibrary CreateCdtLibrary(CdtLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a CDTLibrary to match the given <paramref name="specification"/>.
		/// <param name="cdtLibrary">A CDTLibrary.</param>
		/// <param name="specification">A new specification for the given CDTLibrary.</param>
		/// <returns>The updated CDTLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public ICdtLibrary UpdateCdtLibrary(ICdtLibrary cdtLibrary, CdtLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a CDTLibrary from this bLibrary.
		/// <param name="cdtLibrary">A CDTLibrary.</param>
		/// </summary>
        public void RemoveCdtLibrary(ICdtLibrary cdtLibrary)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// The CCLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<ICcLibrary> GetCcLibraries()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a CCLibrary by name.
		/// <param name="name">A CCLibrary's name.</param>
		/// <returns>The CCLibrary with the given <paramref name="name"/> or <c>null</c> if no such CCLibrary is found.</returns>
		/// </summary>
        public ICcLibrary GetCcLibraryByName(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a CCLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a CCLibrary.</param>
		/// <returns>The newly created CCLibrary.</returns>
		/// </summary>
		public ICcLibrary CreateCcLibrary(CcLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a CCLibrary to match the given <paramref name="specification"/>.
		/// <param name="ccLibrary">A CCLibrary.</param>
		/// <param name="specification">A new specification for the given CCLibrary.</param>
		/// <returns>The updated CCLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public ICcLibrary UpdateCcLibrary(ICcLibrary ccLibrary, CcLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a CCLibrary from this bLibrary.
		/// <param name="ccLibrary">A CCLibrary.</param>
		/// </summary>
        public void RemoveCcLibrary(ICcLibrary ccLibrary)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// The BDTLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<IBdtLibrary> GetBdtLibraries()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a BDTLibrary by name.
		/// <param name="name">A BDTLibrary's name.</param>
		/// <returns>The BDTLibrary with the given <paramref name="name"/> or <c>null</c> if no such BDTLibrary is found.</returns>
		/// </summary>
        public IBdtLibrary GetBdtLibraryByName(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a BDTLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a BDTLibrary.</param>
		/// <returns>The newly created BDTLibrary.</returns>
		/// </summary>
		public IBdtLibrary CreateBdtLibrary(BdtLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a BDTLibrary to match the given <paramref name="specification"/>.
		/// <param name="bdtLibrary">A BDTLibrary.</param>
		/// <param name="specification">A new specification for the given BDTLibrary.</param>
		/// <returns>The updated BDTLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBdtLibrary UpdateBdtLibrary(IBdtLibrary bdtLibrary, BdtLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a BDTLibrary from this bLibrary.
		/// <param name="bdtLibrary">A BDTLibrary.</param>
		/// </summary>
        public void RemoveBdtLibrary(IBdtLibrary bdtLibrary)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// The BIELibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<IBieLibrary> GetBieLibraries()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a BIELibrary by name.
		/// <param name="name">A BIELibrary's name.</param>
		/// <returns>The BIELibrary with the given <paramref name="name"/> or <c>null</c> if no such BIELibrary is found.</returns>
		/// </summary>
        public IBieLibrary GetBieLibraryByName(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a BIELibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a BIELibrary.</param>
		/// <returns>The newly created BIELibrary.</returns>
		/// </summary>
		public IBieLibrary CreateBieLibrary(BieLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a BIELibrary to match the given <paramref name="specification"/>.
		/// <param name="bieLibrary">A BIELibrary.</param>
		/// <param name="specification">A new specification for the given BIELibrary.</param>
		/// <returns>The updated BIELibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBieLibrary UpdateBieLibrary(IBieLibrary bieLibrary, BieLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a BIELibrary from this bLibrary.
		/// <param name="bieLibrary">A BIELibrary.</param>
		/// </summary>
        public void RemoveBieLibrary(IBieLibrary bieLibrary)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// The DOCLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<IDocLibrary> GetDocLibraries()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a DOCLibrary by name.
		/// <param name="name">A DOCLibrary's name.</param>
		/// <returns>The DOCLibrary with the given <paramref name="name"/> or <c>null</c> if no such DOCLibrary is found.</returns>
		/// </summary>
        public IDocLibrary GetDocLibraryByName(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a DOCLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a DOCLibrary.</param>
		/// <returns>The newly created DOCLibrary.</returns>
		/// </summary>
		public IDocLibrary CreateDocLibrary(DocLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a DOCLibrary to match the given <paramref name="specification"/>.
		/// <param name="docLibrary">A DOCLibrary.</param>
		/// <param name="specification">A new specification for the given DOCLibrary.</param>
		/// <returns>The updated DOCLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IDocLibrary UpdateDocLibrary(IDocLibrary docLibrary, DocLibrarySpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a DOCLibrary from this bLibrary.
		/// <param name="docLibrary">A DOCLibrary.</param>
		/// </summary>
        public void RemoveDocLibrary(IDocLibrary docLibrary)
		{
			throw new NotImplementedException();
		}

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return umlPackage.GetTaggedValue("businessTerm").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'copyright'.
        ///</summary>
        public IEnumerable<string> Copyrights
        {
            get { return umlPackage.GetTaggedValue("copyright").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'owner'.
        ///</summary>
        public IEnumerable<string> Owners
        {
            get { return umlPackage.GetTaggedValue("owner").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'reference'.
        ///</summary>
        public IEnumerable<string> References
        {
            get { return umlPackage.GetTaggedValue("reference").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
        public string Status
        {
            get { return umlPackage.GetTaggedValue("status").Value; }
        }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
        public string UniqueIdentifier
        {
            get { return umlPackage.GetTaggedValue("uniqueIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
        public string VersionIdentifier
        {
            get { return umlPackage.GetTaggedValue("versionIdentifier").Value; }
        }

        #endregion
	}
}
