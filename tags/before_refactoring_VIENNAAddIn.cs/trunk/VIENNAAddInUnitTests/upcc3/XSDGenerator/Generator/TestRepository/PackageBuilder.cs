// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    /// <summary>
    /// A builder class for packages.
    /// </summary>
    public class PackageBuilder : RepositoryItemBuilder<PackageBuilder>
    {
        private readonly List<ElementBuilder> elements = new List<ElementBuilder>();
        private readonly List<PackageBuilder> packages = new List<PackageBuilder>();

        /// <summary>
        /// Build a package with the given <paramref name="name"/> and  <paramref name="stereotype"/>.
        /// </summary>
        /// <param name="name">The package's name.</param>
        /// <param name="stereotype">The package's stereotype.</param>
        public PackageBuilder(string name, string stereotype) : base(name, stereotype)
        {
        }

        /// <summary>
        /// Add sub-packages to this package.
        /// </summary>
        /// <param name="packages">The sub-packages.</param>
        /// <returns>This object.</returns>
        public PackageBuilder Packages(params PackageBuilder[] packages)
        {
            this.packages.AddRange(packages);
            return this;
        }

        /// <summary>
        /// Add elements to this package.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns>This object.</returns>
        public PackageBuilder Elements(params ElementBuilder[] elements)
        {
            this.elements.AddRange(elements);
            return this;
        }

        /// <returns>The package's sub-packages.</returns>
        public IEnumerable<PackageBuilder> GetPackages()
        {
            return packages;
        }

        /// <returns>The package's elements.</returns>
        public IEnumerable<ElementBuilder> GetElements()
        {
            return elements;
        }
    }
}