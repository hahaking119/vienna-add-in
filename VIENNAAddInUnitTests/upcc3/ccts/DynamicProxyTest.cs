// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Reflection;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.ccts
{
    [TestFixture]
    public class DynamicProxyTest
    {
        private static T CreateElementProxy<T>(Element element)
        {
            var proxyGenerator = new ProxyGenerator();
            return (T) proxyGenerator.CreateInterfaceProxyWithoutTarget(typeof (T), null,
                                                                        new ProxyGenerationOptions
                                                                        {
                                                                            Selector =
                                                                                new MyInterceptorSelector(element)
                                                                        });
        }

        [Test]
        public void TestDynamicProxy()
        {
            var eaRepository = new EARepository1();
//            var ccRepository = new CCRepository(eaRepository);
//            var cdtDate = ccRepository.FindByPath(EARepository1.PathToDate()) as ICDT;
//            Console.WriteLine(cdtDate.Id);

            Element cdtDateElement = eaRepository.GetElementByID(9);
            Assert.AreEqual("Date", cdtDateElement.Name);
            Assert.AreEqual("A Date.", cdtDateElement.GetTaggedValue(TaggedValues.Definition));
            var cdtDate = CreateElementProxy<ICDT>(cdtDateElement);
            Assert.AreEqual("Date", cdtDate.Name);
//            Assert.AreEqual("", cdtDate.DictionaryEntryName);
//            Assert.AreEqual(0, cdtDate.BusinessTerms.Count());
//            Assert.AreEqual("A Date.", cdtDate.Definition);
        }
    }

    internal class MyInterceptorSelector : IInterceptorSelector
    {
        private readonly Element element;

        public MyInterceptorSelector(Element element)
        {
            this.element = element;
        }

        #region IInterceptorSelector Members

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            Console.WriteLine("{0}#{1}:", method.DeclaringType, method.Name);
            if (method.IsSpecialName && method.Name.StartsWith("get_"))
            {
                string propertyName = method.Name.Substring(4);
                Console.WriteLine("  property {0}", propertyName);
                PropertyInfo propertyInfo = method.DeclaringType.GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    throw new Exception(String.Format("property {0}#{1} not found", method.DeclaringType, propertyName));
                }
                object[] attributes = propertyInfo.GetCustomAttributes(typeof (TaggedValueAttribute), false);
                if (attributes.Length == 0)
                {
                    Console.WriteLine("  property: " + propertyName);
                    return new IInterceptor[] {new ElementPropertyInterceptor(element, propertyName)};
                }
                bool multiValued = true;
                if (typeof (string) == propertyInfo.PropertyType)
                {
                    multiValued = false;
                }
                var taggedValueAttribute = ((TaggedValueAttribute) attributes[0]);
                TaggedValues key = taggedValueAttribute.Key;
                if (key == TaggedValues.Undefined)
                {
                    if (multiValued && propertyName.EndsWith("s"))
                    {
                        propertyName = propertyName.Substring(0, propertyName.Length - 1);
                    }
                    key = TaggedValuesExtensions.ForString(propertyName);
                }
                Console.WriteLine("  tagged value: {0} (multivalued: {1})", key.AsString(), multiValued);
                var interceptor = new TaggedValueInterceptor(
                    key, multiValued, element);
                return new[] {interceptor};
            }
            return new[] {new DefaultInterceptor()};
        }

        #endregion
    }

    internal class DefaultInterceptor : IInterceptor
    {
        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class ElementPropertyInterceptor : IInterceptor
    {
        private readonly Element element;
        private readonly PropertyInfo propertyInfo;

        public ElementPropertyInterceptor(Element element, string propertyName)
        {
            this.element = element;
            propertyInfo = element.GetType().GetProperty(propertyName);
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = propertyInfo.GetValue(element, null);
        }

        #endregion
    }

    internal class TaggedValueInterceptor : IInterceptor
    {
        private readonly Element element;
        private readonly TaggedValues key;
        private readonly bool multiValued;

        public TaggedValueInterceptor(TaggedValues key, Element element) : this(key, false, element)
        {
            this.key = key;
        }

        public TaggedValueInterceptor(TaggedValues key, bool multiValued, Element element)
        {
            this.key = key;
            this.multiValued = multiValued;
            this.element = element;
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            if (multiValued)
            {
                invocation.ReturnValue = element.GetTaggedValues(key);
            }
            else
            {
                invocation.ReturnValue = element.GetTaggedValue(key) ?? string.Empty;
            }
        }

        #endregion
    }
}