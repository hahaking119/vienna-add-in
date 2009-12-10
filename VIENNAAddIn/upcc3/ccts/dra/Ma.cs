using System;
using System.Collections.Generic;
using CctsRepository.DocLibrary;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class Ma : IMa
    {
        private readonly Element element;

        public Ma(CCRepository repository, Element element)
        {
            this.element = element;
        }

        #region IMa Members

        public string Name
        {
            get { return element.Name; }
        }

        public IDocLibrary DocLibrary
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IAsma> Asmas { get; set; }

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

        public int Id
        {
            get { return element.ElementID; }
        }

        #endregion

        public void Update(MaSpec spec)
        {
        }
    }
}