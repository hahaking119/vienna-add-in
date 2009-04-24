using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace VIENNAAddInUnitTests.upcc3.Wizards
{
    [TestFixture]
    public class CacheTest
    {
        #region Test Setup

        public class CDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        {
            public new TValue this[TKey key]
            {
                get
                {
                    try
                    {
                        return base[key];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new CacheException("Value could not be retrieved since key \"" + key + "\" was not found in dictionary!");
                    }
                }

                set
                {
                    if (!ContainsKey(key))
                    {
                        throw new CacheException("Value could not be set since key \"" + key + "\" was not found in dictionary!");
                    }

                    base[key] = value;                    
                }
                
            }

            public new void Add(TKey key, TValue value)
            {
                if (ContainsKey(key))
                {
                    throw new CacheException("Key/Value pair could not be stored since key \"" + key + "\" was not found in dictionary!");
                }
                
                base.Add(key, value);
            }
        }

        public class CacheException : Exception
        {
            public CacheException(string message) : base(message)
            {
            }
        }

        #endregion

        [Test]
        public void TestDictionaryExceptions()
        {
            CDictionary<int, string> sampleDict = new CDictionary<int, string>();

            try
            {
                sampleDict.Add(1, "1");
                sampleDict.Add(2, "2");
                Console.WriteLine(sampleDict[1]);
                Console.WriteLine(sampleDict[3]);
            }
            catch (CacheException e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                sampleDict[13] = "3";                
            }
            catch (CacheException e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                sampleDict.Add(3, "3");
                sampleDict.Add(1, "3");
            }
            catch (CacheException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}