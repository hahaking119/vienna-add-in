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
    internal class UpccBdtLibrary : IBdtLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccBdtLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region IBdtLibrary Members

		/// <summary>
		/// The BDTLibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return umlPackage.Id; }
        }

		/// <summary>
		/// The BDTLibrary's name.
		/// </summary>
        public string Name
        {
            get { return umlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this BDTLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(umlPackage.Parent); }
        }

		/// <summary>
		/// The BDTs contained in this BDTLibrary.
		/// </summary>
		public IEnumerable<IBdt> Bdts
		{
            get
            {
                foreach (var umlclass in umlPackage.Classes)
                {
                    yield return new UpccBdt(umlclass);
                }
            }
		}

		/// <summary>
		/// Retrieves a BDT by name.
		/// <param name="name">A BDT's name.</param>
		/// <returns>The BDT with the given <paramref name="name"/> or <c>null</c> if no such BDT is found.</returns>
		/// </summary>
        public IBdt GetBdtByName(string name)
		{
            foreach (IBdt bdt in Bdts)
            {
                if (bdt.Name == name)
                {
                    return bdt;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a BDT based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a BDT.</param>
		/// <returns>The newly created BDT.</returns>
		/// </summary>
		public IBdt CreateBdt(BdtSpec specification)
		{
		    return new UpccBdt(umlPackage.CreateClass(BdtSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a BDT to match the given <paramref name="specification"/>.
		/// <param name="bdt">A BDT.</param>
		/// <param name="specification">A new specification for the given BDT.</param>
		/// <returns>The updated BDT. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBdt UpdateBdt(IBdt bdt, BdtSpec specification)
		{
		    return new UpccBdt(umlPackage.UpdateClass(((UpccBdt) bdt).UmlClass, BdtSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a BDT from this BDTLibrary.
		/// <param name="bdt">A BDT.</param>
		/// </summary>
        public void RemoveBdt(IBdt bdt)
		{
            umlPackage.RemoveClass(((UpccBdt) bdt).UmlClass);
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
