using NUnit.Framework;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.util
{
    /// <summary>
    /// The purpose of the unit test CCCacheTest is to test the caching 
    /// functionality of the CCCache class which serves as a basis to 
    /// improve the performance of loading Core Components from the 
    /// repository. The following draft shows the five-layer architecture 
    /// that the CCCache is embedded in:
    /// <code>
    ///           +-------------------------------------+
    ///   (5)     |           ABIEModelerForm           |
    ///           +-------------------------------------+
    ///           +------------------+ +----------------+
    ///   (4)     |  TemporaryModel  | |  PersistModel  |
    ///           +------------------+ +----------------+
    ///           +-------------------------------------+
    ///   (3)     |                CCCache              |
    ///           +-------------------------------------+
    ///           +-------------------------------------+
    ///   (2)     |             CCRepository            |
    ///           +-------------------------------------+
    ///           +-------------------------------------+
    ///   (1)     |             EARepository            |
    ///           +-------------------------------------+ 
    /// </code>
    /// </summary>

    [TestFixture]
    public class CCCacheTest
    {
        [Test]
        [Ignore]
        public void CachesCDTLibrariesFromRepository()
        {
            // TODO: Are all CDT libraries from the repository available within the cache?
        }

        [Test]
        [Ignore]
        public void CachesCDTsForSpecificCDTLibraryFromRepository()
        {
            // TODO: Are all CDTs of the particular CDT library available within the cache?
        }

        [Test]
        [Ignore]
        public void CachesCCLibrariesFromRepository()        
        {
            // TODO: Are all CC libraries from the repository available within the cache?
        }

        [Test]
        [Ignore]
        public void CachesACCsForSpecificCCLibraryFromRepository()
        {
            // TODO: Are all ACCs of the particular CC library available within the cache?
        }

        [Test]
        [Ignore]
        public void CachesBDTLibrariesFromRepository()
        {
            // TODO: Are all BDT libraries from the repository available within the cache?
        }

        [Test]
        [Ignore]
        public void CachesBDTsForSpecificBDTLibraryFromRepository()
        {
            // TODO: Are all BDTs of the particular CC library available within the cache?
        }

        [Test]
        [Ignore]
        public void CachesBIELibrariesFromRepository()
        {
            // TODO: Are all BIE libraries from the repository available within the cache?
        }

        [Test]
        [Ignore]
        public void CachesBIEsForSpecificBIELibraryFromRepository()
        {
            // TODO: Are all BIEs of the particular CC library available within the cache?
        }
    }
}