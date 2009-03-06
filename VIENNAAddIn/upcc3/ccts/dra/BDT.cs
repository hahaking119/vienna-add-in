using System.Linq;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class BDT : AbstractDT, IBDT
    {
        public BDT(CCRepository repository, Element element) : base(repository, element, "BDT")
        {
        }

        #region IBDT Members

        public IBasedOnDependency BasedOn
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(IsBasedOnDependency);
                return connector != null ? new BasedOnDependency(repository, connector) : null;
            }
        }

        #endregion

        private static bool IsBasedOnDependency(Connector connector)
        {
            return connector.Stereotype == "basedOn";
        }
    }
}