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
    internal class UpccBieLibrary : IBieLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccBieLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region IBieLibrary Members

		/// <summary>
		/// The BIELibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return umlPackage.Id; }
        }

		/// <summary>
		/// The BIELibrary's name.
		/// </summary>
        public string Name
        {
            get { return umlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this BIELibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(umlPackage.Parent); }
        }

		/// <summary>
		/// The ABIEs contained in this BIELibrary.
		/// </summary>
		public IEnumerable<IAbie> Abies
		{
            get
            {
                foreach (var umlClass in umlPackage.Classes)
                {
                    yield return new UpccAbie(umlClass);
                }
            }
		}

		/// <summary>
		/// Retrieves a ABIE by name.
		/// <param name="name">A ABIE's name.</param>
		/// <returns>The ABIE with the given <paramref name="name"/> or <c>null</c> if no such ABIE is found.</returns>
		/// </summary>
        public IAbie GetAbieByName(string name)
		{
            foreach (IAbie abie in Abies)
            {
                if (abie.Name == name)
                {
                    return abie;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a ABIE based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a ABIE.</param>
		/// <returns>The newly created ABIE.</returns>
		/// </summary>
		public IAbie CreateAbie(AbieSpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a ABIE to match the given <paramref name="specification"/>.
		/// <param name="abie">A ABIE.</param>
		/// <param name="specification">A new specification for the given ABIE.</param>
		/// <returns>The updated ABIE. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IAbie UpdateAbie(IAbie abie, AbieSpec specification)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes a ABIE from this BIELibrary.
		/// <param name="abie">A ABIE.</param>
		/// </summary>
        public void RemoveAbie(IAbie abie)
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
