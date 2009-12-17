using System.Collections.Generic;
using Moq;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo
{
    public class UmlTaggedValueBuilder
    {
        private string value;

        public UmlTaggedValueBuilder WithValue(string value)
        {
            this.value = value;
            return this;
        }

        public UmlTaggedValueBuilder WithSplitValues(IEnumerable<string> values)
        {
            value = MultiPartTaggedValue.Merge(values);
            return this;
        }

        public IUmlTaggedValue Build()
        {
            var taggedValueMock = new Mock<IUmlTaggedValue>();
            taggedValueMock.SetupGet(taggedValue => taggedValue.Value).Returns(value);
            taggedValueMock.SetupGet(taggedValue => taggedValue.SplitValues).Returns(MultiPartTaggedValue.Split(value));
            return taggedValueMock.Object;
        }
    }
}