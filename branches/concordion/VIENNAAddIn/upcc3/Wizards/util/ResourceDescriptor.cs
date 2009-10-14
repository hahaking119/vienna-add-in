using VIENNAAddIn.Settings;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    public class ResourceDescriptor
    {
        /// A string array of the resources to be retrieved. An example would be:
        /// new string[] {"primlibrary.xmi", "cdtlibrary.xmi"}
        public string[] Resources { get; private set; }

        /// A string representing the URI where the resources are to be retrieved from. An
        /// example would be "http://www.myresources.com/xmi/".
        public string DownloadUri { get; private set; }

        /// A string representing the location on the local file system specifying where the
        /// resources retrieved should be stored at. An example would be "c:\\temp\\cache\\".
        public string StorageDirectory { get; private set; }

        public ResourceDescriptor()
        {
            Resources = new[] {"enumlibrary.xmi", "primlibrary.xmi", "cdtlibrary.xmi", "cclibrary.xmi"};
            DownloadUri = "http://www.umm-dev.org/xmi/";
            StorageDirectory = AddInSettings.HomeDirectory + "upcc3\\resources\\xmi\\";
        }

        public ResourceDescriptor(ResourceDescriptor descriptor)
        {
            Resources = descriptor.Resources;
            DownloadUri = descriptor.DownloadUri;
            StorageDirectory = descriptor.StorageDirectory;
        }

        public ResourceDescriptor(string[] resources, string downloadURI, string storageDirectory)
        {
            Resources = resources;
            DownloadUri = downloadURI;
            StorageDirectory = storageDirectory;
        }

        public ResourceDescriptor(string majorVersion, string minorVersion) : this()
        {
            string relativePath = majorVersion + "_" + minorVersion;

            DownloadUri += relativePath + "/";
            StorageDirectory += relativePath + "\\";            
        }

    }
}