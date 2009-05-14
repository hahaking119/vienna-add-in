using Castle.Core.Interceptor;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.Utils;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.Utils
{
    [TestFixture]
    public class RepositoryCopierTest
    {
        [Test]
        public void TestCopyRepository()
        {
            var original = new EARepository1();
            var copy = new EARepository();
            RepositoryCopier.CopyRepository(original, copy);
            EAAssert.RepositoriesAreEqual(original, copy, Path.EmptyPath);
        }
    }

    public class AssertPropertiesAreEqualInterceptor<T> : IInterceptor
    {
        private readonly T actual;
        private readonly T expected;
        private readonly Path path;

        public AssertPropertiesAreEqualInterceptor(T expected, T actual, Path path)
        {
            this.expected = expected;
            this.actual = actual;
            this.path = path;
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            object expectedValue = invocation.Method.Invoke(expected, invocation.Arguments);
            object actualValue = invocation.Method.Invoke(actual, invocation.Arguments);
            invocation.ReturnValue = expectedValue;
            Assert.AreEqual(expectedValue, actualValue,
                            "The property " + typeof(T).Name + "." +
                            invocation.Method.Name.Substring(4) +
                            " of " + path + " should be equal.");
        }

        #endregion
    }

}