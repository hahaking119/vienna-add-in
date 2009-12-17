using EA;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlDependency<TTarget> : IUmlDependency<TTarget>
    {
        private readonly Connector eaConnector;
        private readonly TargetFactory createTarget;
        private readonly Repository eaRepository;

        public delegate TTarget TargetFactory(Element targetElement);

        public EaUmlDependency(Repository eaRepository, Connector eaConnector, TargetFactory createTarget)
        {
            this.eaRepository = eaRepository;
            this.eaConnector = eaConnector;
            this.createTarget = createTarget;
        }

        #region IUmlDependency Members

        public TTarget Target
        {
            get
            {
                Element targetElement = eaRepository.GetElementByID(eaConnector.SupplierID);
                return createTarget(targetElement);
            }
        }

        #endregion
    }
}