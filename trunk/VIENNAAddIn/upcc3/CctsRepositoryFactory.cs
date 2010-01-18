using EA;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3
{
    public static class CctsRepositoryFactory
    {
        public static CCRepository CreateCctsRepository(Repository eaRepository)
        {
            return new CCRepository(eaRepository);
        }
    }
}
