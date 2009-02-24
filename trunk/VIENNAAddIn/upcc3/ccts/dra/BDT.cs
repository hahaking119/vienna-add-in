using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class BDT : AbstractDT, IBDT
    {
        public BDT(CCRepository repository, Element element) : base(repository, element)
        {
        }

        #region IBDT Members

        public IEnumerable<IBasedOnDependency> BasedOn
        {
            get
            {
                foreach (Connector con in element.Connectors)
                {
                    if (con.Stereotype == "basedOn")
                    {
                        yield return new BasedOnDependency(con);
                    }
                }
            }
        }

        #endregion
    }
}