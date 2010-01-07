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
    internal class UpccMa : IMa
    {
        public UpccMa(IUmlClass umlClass)
        {
            UmlClass = umlClass;
        }

        public IUmlClass UmlClass { get; private set; }

        #region IMa Members

        public int Id
        {
            get { return UmlClass.Id; }
        }

        public string Name
        {
            get { return UmlClass.Name; }
        }

		public IDocLibrary DocLibrary
        {
            get { return new UpccDocLibrary(UmlClass.Package); }
        }

		public IEnumerable<IAsma> Asmas
        {
            get
            {
                foreach (var association in UmlClass.GetAssociationsByStereotype("ASMA"))
                {
                    yield return new UpccAsma(association);
                }
            }
        }

		/// <summary>
		/// Creates a(n) ASMA based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) ASMA.</param>
		/// <returns>The newly created ASMA.</returns>
		/// </summary>
		public IAsma CreateAsma(AsmaSpec specification)
		{
		    return new UpccAsma(UmlClass.CreateAssociation(AsmaSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a(n) ASMA to match the given <paramref name="specification"/>.
		/// <param name="asma">A(n) ASMA.</param>
		/// <param name="specification">A new specification for the given ASMA.</param>
		/// <returns>The updated ASMA. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IAsma UpdateAsma(IAsma asma, AsmaSpec specification)
		{
		    return new UpccAsma(UmlClass.UpdateAssociation(((UpccAsma) asma).UmlAssociation, AsmaSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a(n) ASMA from this MA.
		/// <param name="asma">A(n) ASMA.</param>
		/// </summary>
        public void RemoveAsma(IAsma asma)
		{
            UmlClass.RemoveAssociation(((UpccAsma) asma).UmlAssociation);
		}

        #endregion
    }
}
