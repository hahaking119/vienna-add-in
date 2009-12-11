using System.Collections.Generic;
using CctsRepository.PrimLibrary;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc
{
    public partial class UpccPrimTests
    {
        [Test]
        public void ShouldReturnPrimName()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithName("primName")
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.Name, Is.EqualTo("primName"));
        }

        [Test]
        public void ShouldReturnPrimId()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithId(7)
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.Id, Is.EqualTo(7));
        }

        [Test]
        public void ShouldReturnPrimLibrary()
        {
            const int primLibraryId = 6;
            var primLibraryPackage = new Mock<IUmlPackage>();
            primLibraryPackage.SetupGet(package => package.Id).Returns(primLibraryId);

            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithPackage(primLibraryPackage.Object)
                .Build();

            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.PrimLibrary.Id, Is.EqualTo(primLibraryId));
        }

        [Test]
        public void ShouldReturnIsEquivalentTo()
        {
            const int equivalentPrimId = 5;
            IUmlDataType equivalentPrimUmlDataType = new UmlDataTypeBuilder()
                .WithId(equivalentPrimId)
                .Build();

            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithDependencies(Stereotype.isEquivalentTo, equivalentPrimUmlDataType)
                .Build();

            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.IsEquivalentTo.Id, Is.EqualTo(equivalentPrimId));
        }

        [Test]
        public void ShouldReturnFirstDependencyIfMultipleIsEquivalentToDependenciesExist()
        {
            const int equivalentPrimId = 5;
            IUmlDataType equivalentPrimUmlDataType1 = new UmlDataTypeBuilder()
                .WithId(equivalentPrimId)
                .Build();
            IUmlDataType equivalentPrimUmlDataType2 = new UmlDataTypeBuilder()
                .WithId(equivalentPrimId + 1)
                .Build();

            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .WithDependencies(Stereotype.isEquivalentTo, equivalentPrimUmlDataType1, equivalentPrimUmlDataType2)
                .Build();

            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.IsEquivalentTo.Id, Is.EqualTo(equivalentPrimId));
        }

        [Test]
        public void ShouldReturnNullIfIsEquivalentToIsNull()
        {
            IUmlDataType primUmlDataType = new UmlDataTypeBuilder()
                .Build();
            IPrim prim = new UpccPrim(primUmlDataType);
            Assert.That(prim.IsEquivalentTo, Is.Null);
        }

    }

    public class UmlDataTypeBuilder
    {
        private Mock<IUmlDataType> mock = new Mock<IUmlDataType>();

        public IUmlDataType Build()
        {
            return mock.Object;
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
            mock.Setup(dataType => dataType.GetTaggedValue(taggedValueName)).Returns(taggedValueMock.Object);
            return this;
        }

        public UmlDataTypeBuilder WithMultiValuedTaggedValue(TaggedValues taggedValueName, params string[] values)
        {
            var taggedValueMock = new Mock<IUmlTaggedValue>();
            taggedValueMock.SetupGet(taggedValue => taggedValue.Value).Returns(string.Join(MultiPartTaggedValue.ValueSeparator.ToString(), values));
            taggedValueMock.SetupGet(taggedValue => taggedValue.SplitValues).Returns(values);
            mock.Setup(dataType => dataType.GetTaggedValue(taggedValueName)).Returns(taggedValueMock.Object);
            return this;
        }

        public UmlDataTypeBuilder WithPackage(IUmlPackage umlPackage)
        {
            mock.SetupGet(dataType => dataType.Package).Returns(umlPackage);
            return this;
        }

        public UmlDataTypeBuilder WithDependencies(string stereotype, params IUmlDataType[] targets)
        {
            var dependencies = new List<IUmlDependency<IUmlClassifier>>();
            foreach (var target in targets)
            {
                var dependencyMock = new Mock<IUmlDependency<IUmlClassifier>>();
                dependencyMock.SetupGet(dependency => dependency.Target).Returns(target);
                dependencies.Add(dependencyMock.Object);
            }
            mock.Setup(dataType => dataType.GetDependenciesByStereotype(stereotype)).Returns(dependencies);
            return this;
        }
    }
}