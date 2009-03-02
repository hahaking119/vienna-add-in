using System;
using System.Collections.Generic;
using EA;
using System.Linq;
using VIENNAAddIn.upcc3.ccts.util;

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

        public IBDT CreateBDT(BDTSpec spec)
        {
            var bdt = (Element) package.Elements.AddNew(spec.Name, "Class");
            bdt.Stereotype = "BDT";
            bdt.AddConnector("basedOn", "basedOn", spec.BasedOn.Id);
            bdt.AddAttribute("CON", "Content", spec.CON.Type.Name, spec.CON.Type.Id);
            foreach (var sup in spec.SUPs)
            {
                bdt.AddAttribute("SUP", sup.Name, sup.Type.Name, sup.Type.Id);
            }
            bdt.Update();
            package.Elements.Refresh();
            return new BDT(repository, bdt);
        }

        #endregion

        public IElement ElementByName(string name)
        {
            return BDTs.First(e => e.Name == name);
        }
    }
}