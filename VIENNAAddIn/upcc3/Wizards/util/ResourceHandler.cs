using System.IO;
using System.Net;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    ///<summary>
    ///</summary>
    public class ResourceHandler
    {
        #region Properties

        private string[] Resources { get; set; }
        private string DownloadUri { get; set; }
        private string StorageDirectory { get; set; }

        #endregion   

        ///<summary>
        ///</summary>
        ///<param name="resources"></param>
        ///<param name="downloadUri"></param>
        ///<param name="storageDirectory"></param>
        public ResourceHandler(string[] resources, string downloadUri, string storageDirectory)
        {
            Resources = resources;
            DownloadUri = downloadUri;
            StorageDirectory = storageDirectory;
        }
        

        ///<summary>
        /// The method retrieves a resource, such as an XMI file, located at a particular URI. 
        /// The content of the resource is returned as a string. 
        ///</summary>
        ///<param name="uri">
        /// A URI pointing to the resource to be retrieved. An example would be 
        /// "http://www.umm-dev.org/xmi/primlibrary.xmi".
        /// </param>
        ///<returns>A string representation from the content located at the specified URI.</returns>
        private string RetrieveContentFromUri(string uri)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(uri);
            }
        }

        ///<summary>
        /// The method writes content to a particular file on a file system. The name of the output file
        /// as well as the content to be written to the output file are specified in the parameters of the
        /// method.
        ///</summary>
        ///<param name="downloadUri">
        /// A URI pointing to the resource to be retrieved. An example would be 
        /// "http://www.umm-dev.org/xmi/primlibrary.xmi".
        /// </param>        
        ///<param name="outputFile">A path including the file name that the content is stored at. 
        /// An example would be "c:\\temp\\output\\file.xmi". 
        /// </param>        
        private void CacheSingleResourceLocally(string downloadUri, string outputFile)
        {
            string currentFileContent = "";
            string newFileContent = RetrieveContentFromUri(downloadUri);

            // First it is checked whether the file to be written already exists on the file system. 
            if (File.Exists(outputFile))
            {
                currentFileContent = File.ReadAllText(outputFile);
            }

            // Second it is checked whether the content to be written is empty or not. In case it is 
            // empty the file is not written to the file system.
            if (!string.IsNullOrEmpty(newFileContent))
            {
                // Third it is checked whether the content of the file equals the new content or not.
                // If the content is different then it is actually written to the file system. 
                if (!currentFileContent.Equals(newFileContent))
                {
                    // Write the content to the file located on the filesystem at the path 
                    // specified in the parameter outputFile. 
                    using (StreamWriter writer = File.CreateText(outputFile))
                    {
                        writer.Write(newFileContent);
                    }
                }
            }
        }

        ///<summary>
        ///</summary>
        public void CacheResourcesLocally()
        {
            foreach (string resourceFile in Resources)
            {
                CacheSingleResourceLocally(DownloadUri + resourceFile, StorageDirectory + resourceFile);
            }
        }
    }
}