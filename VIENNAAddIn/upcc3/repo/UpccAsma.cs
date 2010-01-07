using System;
using CctsRepository;
using CctsRepository.DocLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccAsma : IAsma
    {
        public IUmlAssociation UmlAssociation { get; set; }

        public UpccAsma(IUmlAssociation umlAssociation)
        {
            UmlAssociation = umlAssociation;
        }

        public int Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string UpperBound
        {
            get { throw new NotImplementedException(); }
        }

        public string LowerBound
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsOptional()
        {
            throw new NotImplementedException();
        }

        public IMa AssociatingMa
        {
            get { throw new NotImplementedException(); }
        }

        public BieAggregator AssociatedBieAggregator
        {
            get { throw new NotImplementedException(); }
        }
    }
}