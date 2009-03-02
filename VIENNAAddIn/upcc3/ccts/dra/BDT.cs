using System.Linq;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class BDT : AbstractDT, IBDT
    {
        public BDT(CCRepository repository, Element element) : base(repository, element, "BDT")
        {
        }

//        public IEnumerable<IBasedOnDependency> BasedOn
//        {
//            get
//            {
//                foreach (Connector con in element.Connectors)
//                {
//                    if (con.Stereotype == "basedOn")
//                    {
//                        yield return new BasedOnDependency(con);
//                    }
//                }
//            }
//        }

        #region IBDT Members

        public ICDT BasedOn
        {
            get
            {
                Connector connector =
                    Connectors.FirstOrDefault(IsBasedOnDependency);
                return connector != null ? repository.GetCDT(connector.SupplierID) : null;
            }
        }

        #endregion

        private static bool IsBasedOnDependency(Connector connector)
        {
            return connector.Stereotype == "basedOn";
        }
    }
}