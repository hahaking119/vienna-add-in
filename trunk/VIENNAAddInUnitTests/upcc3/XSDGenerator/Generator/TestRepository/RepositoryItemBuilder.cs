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
    /// Base class for builders of test repository items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryItemBuilder<T> where T : class
    {
        private readonly string name;
        private readonly string stereotype;
        private readonly List<TaggedValueBuilder> taggedValues = new List<TaggedValueBuilder>();

        protected RepositoryItemBuilder(string name, string stereotype)
        {
            this.name = name;
            this.stereotype = stereotype;
        }

        public string GetName()
        {
            return name;
        }

        public string GetStereotype()
        {
            return stereotype;
        }

        public T TaggedValues(params TaggedValueBuilder[] taggedValues)
        {
            this.taggedValues.AddRange(taggedValues);
            return this as T;
        }

        public IEnumerable<TaggedValueBuilder> GetTaggedValues()
        {
            return taggedValues;
        }
    }
}