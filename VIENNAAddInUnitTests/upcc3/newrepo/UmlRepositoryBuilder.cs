using System.Collections.Generic;
using Moq;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo
{
    internal class UmlRepositoryBuilder
    {
        private readonly List<UmlPackageBuilder> packageBuilders = new List<UmlPackageBuilder>();

        public UmlRepositoryBuilder With(UmlPackageBuilder packageBuilder)
        {
            packageBuilders.Add(packageBuilder);
            return this;
        }

        public IUmlRepository Build()
        {
            var packagesByStereotype = new Dictionary<string, List<IUmlPackage>>();
            foreach (UmlPackageBuilder packageBuilder in packageBuilders)
            {
                IUmlPackage package = packageBuilder.Build();
                if (packagesByStereotype.ContainsKey(package.Stereotype))
                {
                    packagesByStereotype[package.Stereotype].Add(package);
                }
                else
                {
                    packagesByStereotype[package.Stereotype] = new List<IUmlPackage>
                                                               {
                                                                   package
                                                               };
                }
            }
            var mock = new Mock<IUmlRepository>();
            foreach (string stereotype in packagesByStereotype.Keys)
            {
                string s = stereotype;
                mock.Setup(repo => repo.GetPackagesByStereotype(s)).Returns(packagesByStereotype[stereotype]);
            }
            return mock.Object;
        }
    }
}