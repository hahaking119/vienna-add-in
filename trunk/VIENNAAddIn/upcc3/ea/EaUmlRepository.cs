using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.uml;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.ea
{
    public class EaUmlRepository : IUmlRepository
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

        public IUmlPackage GetPackageById(int id)
        {
            return new EaUmlPackage(eaRepository, eaRepository.GetPackageByID(id));
        }

        public IUmlPackage GetPackageByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IUmlDataType GetDataTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public IUmlDataType GetDataTypeByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IUmlEnumeration GetEnumerationById(int id)
        {
            throw new NotImplementedException();
        }

        public IUmlEnumeration GetEnumerationByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IUmlClass GetClassById(int id)
        {
            throw new NotImplementedException();
        }

        public IUmlClass GetClassByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Path> GetRootLocations()
        {
            throw new NotImplementedException();
        }

        public IUmlPackage CreateRootPackage(Path rootLocation, UmlPackageSpec spec)
        {
            throw new NotImplementedException();
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