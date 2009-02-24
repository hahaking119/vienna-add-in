using System;
using System.Collections.Generic;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class BDT : AbstractDT, IBDT
    {
        public BDT(CCRepository repository, Element element) : base(repository, element)
        {
        }

        public IList<IBasedOnDependency> BasedOn
        {
            get
            {
                var result = new List<IBasedOnDependency>();
                foreach (Connector con in element.Connectors)
                {
                    if (con.Stereotype == "basedOn")
                    {
                        result.Add(repository.GetBasedOnDependency(con));
                    }
                }
                return result;
            }
        }
    }

}