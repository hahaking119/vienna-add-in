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
    internal class UpccPrimLibrary : IPrimLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccPrimLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region IPrimLibrary Members

		/// <summary>
		/// The PRIMLibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return umlPackage.Id; }
        }

		/// <summary>
		/// The PRIMLibrary's name.
		/// </summary>
        public string Name
        {
            get { return umlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this PRIMLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(umlPackage.Parent); }
        }

		/// <summary>
		/// The PRIMs contained in this PRIMLibrary.
		/// </summary>
		public IEnumerable<IPrim> Prims
		{
            get
            {
                foreach (var umlDataType in umlPackage.DataTypes)
                {
                    yield return new UpccPrim(umlDataType);
                }
            }
		}

		/// <summary>
		/// Retrieves a PRIM by name.
		/// <param name="name">A PRIM's name.</param>
		/// <returns>The PRIM with the given <paramref name="name"/> or <c>null</c> if no such PRIM is found.</returns>
		/// </summary>
        public IPrim GetPrimByName(string name)
		{
            foreach (IPrim prim in Prims)
            {
                if (prim.Name == name)
                {
                    return prim;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a PRIM based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a PRIM.</param>
		/// <returns>The newly created PRIM.</returns>
		/// </summary>
		public IPrim CreatePrim(PrimSpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a PRIM to match the given <paramref name="specification"/>.
		/// <param name="prim">A PRIM.</param>
		/// <param name="specification">A new specification for the given PRIM.</param>
		/// <returns>The updated PRIM. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IPrim UpdatePrim(IPrim prim, PrimSpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a PRIM from this PRIMLibrary.
		/// <param name="prim">A PRIM.</param>
		/// </summary>
        public void RemovePrim(IPrim prim)
		{
			throw new NotImplementedException();
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
