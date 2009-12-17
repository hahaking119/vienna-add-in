using System;
using System.Collections.Generic;
using Moq;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo
{
    public class UmlDataTypeBuilder
    {
        private Mock<IUmlDataType> mock = new Mock<IUmlDataType>();

        public IUmlDataType Build()
        {
            return BuildMock().Object;
        }

        public Mock<IUmlDataType> BuildMock()
        {
            return mock;
        }

        public UmlDataTypeBuilder WithId(int id)
        {
            mock.Setup(dataType => dataType.Id).Returns(id);
            return this;
        }

        public UmlDataTypeBuilder WithName(string name)
        {
            mock.Setup(dataType => dataType.Name).Returns(name);
            return this;
        }

        public UmlDataTypeBuilder WithTaggedValue(TaggedValues taggedValueName, string value)
        {
            var taggedValueMock = new Mock<IUmlTaggedValue>();
            taggedValueMock.SetupGet(taggedValue => taggedValue.Value).Returns(value);
            taggedValueMock.SetupGet(taggedValue => taggedValue.SplitValues).Returns(new[] {value});
            mock.Setup(dataType => dataType.GetTaggedValue(taggedValueName.ToString())).Returns(taggedValueMock.Object);
            return this;
        }

        public UmlDataTypeBuilder WithMultiValuedTaggedValue(TaggedValues taggedValueName, params string[] values)
        {
            var taggedValueMock = new Mock<IUmlTaggedValue>();
            taggedValueMock.SetupGet(taggedValue => taggedValue.Value).Returns(string.Join(MultiPartTaggedValue.ValueSeparator.ToString(), values));
            taggedValueMock.SetupGet(taggedValue => taggedValue.SplitValues).Returns(values);
            mock.Setup(dataType => dataType.GetTaggedValue(taggedValueName.ToString())).Returns(taggedValueMock.Object);
            return this;
        }

        public UmlDataTypeBuilder WithPackage(IUmlPackage umlPackage)
        {
            mock.SetupGet(dataType => dataType.Package).Returns(umlPackage);
            return this;
        }

        public UmlDataTypeBuilder WithDependencies(string stereotype, params IUmlDataType[] targets)
        {
            var dependencies = new List<IUmlDependency<IUmlDataType>>();
            foreach (var target in targets)
            {
                var dependencyMock = new Mock<IUmlDependency<IUmlDataType>>();
                dependencyMock.SetupGet(dependency => dependency.Target).Returns(target);
                dependencies.Add(dependencyMock.Object);
            }
            mock.Setup(dataType => dataType.GetDependenciesByStereotype(stereotype)).Returns(dependencies);
            return this;
        }
    }
}