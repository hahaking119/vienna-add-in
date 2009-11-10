using EA;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ea
{
    internal class EaUmlDependency : IUmlDependency
    {
        private readonly Connector eaConnector;
        private readonly Repository eaRepository;

        public EaUmlDependency(Repository eaRepository, Connector eaConnector)
        {
            this.eaRepository = eaRepository;
            this.eaConnector = eaConnector;
        }

        #region IUmlDependency Members

        public IUmlClass Target
        {
            get
            {
                Element targetElement = eaRepository.GetElementByID(eaConnector.SupplierID);
                return new EaUmlClass(eaRepository, targetElement);
            }
        }

        #endregion
    }
}