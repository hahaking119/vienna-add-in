using EA;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ea
{
    internal class EaUmlDependency<TTarget> : IUmlDependency<TTarget>
    {
        private readonly Connector eaConnector;
        private readonly TargetFactory createTarget;
        private readonly Repository eaRepository;

        public delegate TTarget TargetFactory(Repository eaRepository, Element targetElement);

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
                return createTarget(eaRepository, targetElement);
            }
        }

        #endregion
    }
}