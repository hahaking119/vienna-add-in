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
    internal class UpccCcLibrary : ICcLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccCcLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region ICcLibrary Members

		/// <summary>
		/// The CCLibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return umlPackage.Id; }
        }

		/// <summary>
		/// The CCLibrary's name.
		/// </summary>
        public string Name
        {
            get { return umlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this CCLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(umlPackage.Parent); }
        }

		/// <summary>
		/// The ACCs contained in this CCLibrary.
		/// </summary>
		public IEnumerable<IAcc> Accs
		{
            get
            {
                foreach (var umlClass in umlPackage.Classes)
                {
                    yield return new UpccAcc(umlClass);
                }
            }
		}

		/// <summary>
		/// Retrieves a ACC by name.
		/// <param name="name">A ACC's name.</param>
		/// <returns>The ACC with the given <paramref name="name"/> or <c>null</c> if no such ACC is found.</returns>
		/// </summary>
        public IAcc GetAccByName(string name)
		{
            foreach (IAcc acc in Accs)
            {
                if (acc.Name == name)
                {
                    return acc;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a ACC based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a ACC.</param>
		/// <returns>The newly created ACC.</returns>
		/// </summary>
		public IAcc CreateAcc(AccSpec specification)
		{
		    return new UpccAcc(umlPackage.CreateClass(AccSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a ACC to match the given <paramref name="specification"/>.
		/// <param name="acc">A ACC.</param>
		/// <param name="specification">A new specification for the given ACC.</param>
		/// <returns>The updated ACC. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IAcc UpdateAcc(IAcc acc, AccSpec specification)
		{
		    return new UpccAcc(umlPackage.UpdateClass(((UpccAcc) acc).UmlClass, AccSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a ACC from this CCLibrary.
		/// <param name="acc">A ACC.</param>
		/// </summary>
        public void RemoveAcc(IAcc acc)
		{
            umlPackage.RemoveClass(((UpccAcc) acc).UmlClass);
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
