using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class BDT : AbstractDT, IBDT
    {
        public BDT(IBusinessLibrary library, Element element) : base(library, element)
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