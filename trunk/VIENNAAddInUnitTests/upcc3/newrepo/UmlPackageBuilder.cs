using System;
using System.Collections.Generic;
using Moq;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo
{
    public class UmlPackageBuilder
    {
        private int id;
        private string name;
        private IUmlPackage parent;
        private string stereotype;
        private Dictionary<TaggedValues, IUmlTaggedValue> taggedValues = new Dictionary<TaggedValues, IUmlTaggedValue>();
        private List<IUmlDataType> dataTypes = new List<IUmlDataType>();

        public UmlPackageBuilder WithId(int id)
        {
            this.id = id;
            return this;
        }

        public UmlPackageBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public UmlPackageBuilder WithStereotype(string stereotype)
        {
            this.stereotype = stereotype;
            return this;
        }

        public UmlPackageBuilder WithParent(IUmlPackage parent)
        {
            this.parent = parent;
            return this;
        }

        public UmlPackageBuilder WithTaggedValue(TaggedValues taggedValueName, string value)
        {
            taggedValues[taggedValueName] = new UmlTaggedValueBuilder()
                .WithValue(value)
                .Build();
            return this;
        }

        public UmlPackageBuilder WithMultiValuedTaggedValue(TaggedValues taggedValueName, params string[] values)
        {
            taggedValues[taggedValueName] = new UmlTaggedValueBuilder()
                .WithSplitValues(values)
                .Build();
            return this;
        }

        public IUmlPackage Build()
        {
            return BuildMock().Object;
        }

        public Mock<IUmlPackage> BuildMock()
        {
            var mock = new Mock<IUmlPackage>();
            mock.SetupGet(p => p.Id).Returns(id);
            mock.SetupGet(p => p.Name).Returns(name);
            mock.SetupGet(p => p.Stereotype).Returns(stereotype);
            mock.SetupGet(p => p.Parent).Returns(parent);
            mock.SetupGet(p => p.DataTypes).Returns(dataTypes);
            foreach (var taggedValue in taggedValues)
            {
                TaggedValues taggedValueName = taggedValue.Key;
                mock.Setup(dataType => dataType.GetTaggedValue(taggedValueName)).Returns(taggedValue.Value);
            }
            return mock;
        }

        public UmlPackageBuilder WithDataType(IUmlDataType dataType)
        {
            dataTypes.Add(dataType);
            return this;
        }
    }
}