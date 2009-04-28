// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace VIENNAAddInUnitTests.TestRepository
{
    /// <summary>
    /// A builder class for elements.
    /// </summary>
    public class ElementBuilder : RepositoryItemBuilder<ElementBuilder>
    {
        private readonly List<AttributeBuilder> attributes = new List<AttributeBuilder>();

        /// <summary>
        /// Build an element with the given <paramref name="name"/> and <paramref name="stereotype"/>.
        /// </summary>
        /// <param name="name">The element's name.</param>
        /// <param name="stereotype">The element's stereotype.</param>
        public ElementBuilder(string name, string stereotype) : base(name, stereotype)
        {
        }

        public ElementBuilder Attributes(params AttributeBuilder[] attributes)
        {
            this.attributes.AddRange(attributes);
            return this;
        }

        public IEnumerable<AttributeBuilder> GetAttributes()
        {
            return attributes;
        }
    }
}