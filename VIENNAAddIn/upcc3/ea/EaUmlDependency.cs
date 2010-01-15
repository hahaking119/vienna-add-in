using EA;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.ea
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

        public IUmlClassifier Target
        {
            get
            {
                Element targetElement = eaRepository.GetElementByID(eaConnector.SupplierID);
                return new EaUmlClassifier(eaRepository, targetElement);
            }
        }

        #endregion
    }
}