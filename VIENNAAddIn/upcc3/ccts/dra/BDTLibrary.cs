using System;
using System.Collections.Generic;
using EA;
using System.Linq;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BDTLibrary : BusinessLibrary, IBDTLibrary
    {
        public BDTLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IBDTLibrary Members

        public IEnumerable<IBDT> BDTs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    yield return new BDT(repository, element);
                }
            }
        }

        public void CreateBDT(BDTSpec spec)
        {
            Console.WriteLine("Creating BDT: " + spec.Name);
            Console.WriteLine("  CON: " + spec.CON.Type.Name);
            Console.WriteLine("  SUPs:");
            foreach (var sup in spec.SUPs)
            {
                Console.WriteLine("    " + sup.Name + ": " + sup.Type.Name);
            }
        }

        #endregion

        public IElement ElementByName(string name)
        {
            return BDTs.First(e => e.Name == name);
        }
    }
}