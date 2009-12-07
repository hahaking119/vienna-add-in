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

        public string Name
        {
            get { return element.Name; }
        }

        public IDocLibrary DocLibrary
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IAsma> Asmas { get; set; }

        public int Id
        {
            get { return element.ElementID; }
        }

        public void Update(MaSpec spec)
        {
        }
    }
}