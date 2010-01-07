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
    internal class UpccCdtLibrary : ICdtLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccCdtLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region ICdtLibrary Members

		/// <summary>
		/// The CDTLibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return umlPackage.Id; }
        }

		/// <summary>
		/// The CDTLibrary's name.
		/// </summary>
        public string Name
        {
            get { return umlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this CDTLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(umlPackage.Parent); }
        }

		/// <summary>
		/// The CDTs contained in this CDTLibrary.
		/// </summary>
		public IEnumerable<ICdt> Cdts
		{
            get
            {
                foreach (var umlClass in umlPackage.Classes)
                {
                    yield return new UpccCdt(umlClass);
                }
            }
		}

		/// <summary>
		/// Retrieves a CDT by name.
		/// <param name="name">A CDT's name.</param>
		/// <returns>The CDT with the given <paramref name="name"/> or <c>null</c> if no such CDT is found.</returns>
		/// </summary>
        public ICdt GetCdtByName(string name)
		{
            foreach (ICdt cdt in Cdts)
            {
                if (cdt.Name == name)
                {
                    return cdt;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a CDT based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a CDT.</param>
		/// <returns>The newly created CDT.</returns>
		/// </summary>
		public ICdt CreateCdt(CdtSpec specification)
		{
		    return new UpccCdt(umlPackage.CreateClass(CdtSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a CDT to match the given <paramref name="specification"/>.
		/// <param name="cdt">A CDT.</param>
		/// <param name="specification">A new specification for the given CDT.</param>
		/// <returns>The updated CDT. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public ICdt UpdateCdt(ICdt cdt, CdtSpec specification)
		{
		    return new UpccCdt(umlPackage.UpdateClass(((UpccCdt) cdt).UmlClass, CdtSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a CDT from this CDTLibrary.
		/// <param name="cdt">A CDT.</param>
		/// </summary>
        public void RemoveCdt(ICdt cdt)
		{
            umlPackage.RemoveClass(((UpccCdt) cdt).UmlClass);
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

        ///<summary>
        /// Tagged value 'baseURN'.
        ///</summary>
        public string BaseURN
        {
            get { return umlPackage.GetTaggedValue("baseURN").Value; }
        }

        ///<summary>
        /// Tagged value 'namespacePrefix'.
        ///</summary>
        public string NamespacePrefix
        {
            get { return umlPackage.GetTaggedValue("namespacePrefix").Value; }
        }

        #endregion
	}
}
