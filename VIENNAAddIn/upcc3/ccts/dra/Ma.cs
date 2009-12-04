using System;
using System.Collections.Generic;
using CctsRepository.DocLibrary;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class Ma : IMa
    {
        public Ma(CCRepository repository, Element element)
        {
        }

        public string Name { get; set; }
        public IDocLibrary DocLibrary
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IAsma> Asmas { get; set; }
        public int Id{ get; set; }

        public void Update(MaSpec spec)
        {
        }
    }
}