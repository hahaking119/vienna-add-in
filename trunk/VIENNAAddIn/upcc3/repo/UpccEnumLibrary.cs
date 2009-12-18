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
    internal class UpccEnumLibrary : IEnumLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccEnumLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region IEnumLibrary Members

		/// <summary>
		/// The ENUMLibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return umlPackage.Id; }
        }

		/// <summary>
		/// The ENUMLibrary's name.
		/// </summary>
        public string Name
        {
            get { return umlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this ENUMLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(umlPackage.Parent); }
        }

		/// <summary>
		/// The ENUMs contained in this ENUMLibrary.
		/// </summary>
		public IEnumerable<IEnum> Enums
		{
            get
            {
                foreach (var umlenumeration in umlPackage.Enumerations)
                {
                    yield return new UpccEnum(umlenumeration);
                }
            }
		}

		/// <summary>
		/// Retrieves a ENUM by name.
		/// <param name="name">A ENUM's name.</param>
		/// <returns>The ENUM with the given <paramref name="name"/> or <c>null</c> if no such ENUM is found.</returns>
		/// </summary>
        public IEnum GetEnumByName(string name)
		{
            foreach (IEnum @enum in Enums)
            {
                if (@enum.Name == name)
                {
                    return @enum;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a ENUM based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a ENUM.</param>
		/// <returns>The newly created ENUM.</returns>
		/// </summary>
		public IEnum CreateEnum(EnumSpec specification)
		{
		    return new UpccEnum(umlPackage.CreateEnumeration(EnumSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a ENUM to match the given <paramref name="specification"/>.
		/// <param name="@enum">A ENUM.</param>
		/// <param name="specification">A new specification for the given ENUM.</param>
		/// <returns>The updated ENUM. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IEnum UpdateEnum(IEnum @enum, EnumSpec specification)
		{
		    return new UpccEnum(umlPackage.UpdateEnumeration(((UpccEnum) @enum).UmlEnumeration, EnumSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a ENUM from this ENUMLibrary.
		/// <param name="@enum">A ENUM.</param>
		/// </summary>
        public void RemoveEnum(IEnum @enum)
		{
            umlPackage.RemoveEnumeration(((UpccEnum) @enum).UmlEnumeration);
		}

		/// <summary>
		/// The IDSCHEMEs contained in this ENUMLibrary.
		/// </summary>
		public IEnumerable<IIdScheme> IdSchemes
		{
            get
            {
                foreach (var umldataType in umlPackage.DataTypes)
                {
                    yield return new UpccIdScheme(umldataType);
                }
            }
		}

		/// <summary>
		/// Retrieves a IDSCHEME by name.
		/// <param name="name">A IDSCHEME's name.</param>
		/// <returns>The IDSCHEME with the given <paramref name="name"/> or <c>null</c> if no such IDSCHEME is found.</returns>
		/// </summary>
        public IIdScheme GetIdSchemeByName(string name)
		{
            foreach (IIdScheme idScheme in IdSchemes)
            {
                if (idScheme.Name == name)
                {
                    return idScheme;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a IDSCHEME based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a IDSCHEME.</param>
		/// <returns>The newly created IDSCHEME.</returns>
		/// </summary>
		public IIdScheme CreateIdScheme(IdSchemeSpec specification)
		{
		    return new UpccIdScheme(umlPackage.CreateDataType(IdSchemeSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a IDSCHEME to match the given <paramref name="specification"/>.
		/// <param name="idScheme">A IDSCHEME.</param>
		/// <param name="specification">A new specification for the given IDSCHEME.</param>
		/// <returns>The updated IDSCHEME. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IIdScheme UpdateIdScheme(IIdScheme idScheme, IdSchemeSpec specification)
		{
		    return new UpccIdScheme(umlPackage.UpdateDataType(((UpccIdScheme) idScheme).UmlDataType, IdSchemeSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a IDSCHEME from this ENUMLibrary.
		/// <param name="idScheme">A IDSCHEME.</param>
		/// </summary>
        public void RemoveIdScheme(IIdScheme idScheme)
		{
            umlPackage.RemoveDataType(((UpccIdScheme) idScheme).UmlDataType);
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
