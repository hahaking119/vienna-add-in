using System.Collections.Generic;
using EA;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ea
{
    internal class EaUmlRepository : IUmlRepository
    {
        private readonly Repository eaRepository;

        public EaUmlRepository(Repository eaRepository)
        {
            this.eaRepository = eaRepository;
        }

        #region IUmlRepository Members

        public IEnumerable<IUmlPackage> GetPackagesByStereotype(string stereotype)
        {
            var eaPackages = new List<Package>();
            foreach (Package eaModel in eaRepository.Models)
            {
                EnumeratePackages(eaModel, eaPackages);
            }
            foreach (Package eaPackage in eaPackages)
            {
                if (eaPackage.Element != null && eaPackage.Element.Stereotype == stereotype)
                {
                    yield return new EaUmlPackage(eaRepository, eaPackage);
                }
            }
        }

        #endregion

        private static void EnumeratePackages(Package root, List<Package> packageList)
        {
            packageList.Add(root);
            foreach (Package package in root.Packages)
            {
                EnumeratePackages(package, packageList);
            }
        }
    }
}