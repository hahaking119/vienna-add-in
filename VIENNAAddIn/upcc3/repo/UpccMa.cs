using System;
using System.Collections.Generic;
using CctsRepository.DocLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccMa : IMa
    {
        public UpccMa(IUmlClass umlClass)
        {
            throw new NotImplementedException();
        }

        public int Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public IDocLibrary DocLibrary
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IAsma> Asmas
        {
            get { throw new NotImplementedException(); }
        }

        public IAsma CreateAsma(AsmaSpec specification)
        {
            throw new NotImplementedException();
        }

        public IAsma UpdateAsma(IAsma asma, AsmaSpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveAsma(IAsma asma)
        {
            throw new NotImplementedException();
        }
    }
}