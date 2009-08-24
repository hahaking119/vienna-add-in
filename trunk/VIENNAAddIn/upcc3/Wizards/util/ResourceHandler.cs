﻿// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.IO;
using System.Net;
using VIENNAAddIn.Settings;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    ///<summary>
    /// The ResourceHandler class may be used to retrieve resources located at a particular URI on 
    /// the web to the local file system. The configuration of the ResourceHandler is achieved 
    /// through the the constructor. More details may be found in the documentation of the 
    /// constructor. 
    ///</summary>
    public class ResourceHandler
    {
        #region Constants

        public static readonly string DefaultDownloadUri = "http://www.umm-dev.org/xmi/";

        public static readonly string[] DefaultResources = new[]
                                                               {
                                                                   "enumlibrary.xmi", "primlibrary.xmi", "cdtlibrary.xmi",
                                                                   "cclibrary.xmi"
                                                               };

        public static readonly string DefaultStorageDirectory = AddInSettings.HomeDirectory + "upcc3\\resources\\xmi\\";

        #endregion

        #region Class Fields

        private readonly string downloadUri;
        private readonly string[] resources;
        private readonly string storageDirectory;

        #endregion

        #region Constructor

        ///<summary>
        /// The constructor of the ResourceHandler initializes the ResourceHandler and therefore
        /// requires three input parameters specifying details on the resources to be retrieved
        /// such as the URI where the resources are located at. 
        ///</summary>
        ///<param name="resources">
        /// A string array of the resources to be retrieved. An example would be:
        ///   new string[] {"primlibrary.xmi", "cdtlibrary.xmi"}
        /// </param>
        ///<param name="downloadUri">
        /// A string representing the URI where the resources are to be retrieved from. An
        /// example would be "http://www.myresources.com/xmi/".
        /// </param>
        ///<param name="storageDirectory">
        /// A string representing the location on the local file system specifying where the
        /// resources retrieved should be stored at. An example would be "c:\\temp\\cache\\".
        /// </param>
        public ResourceHandler(string[] resources, string downloadUri, string storageDirectory)
        {
            this.resources = resources;
            this.downloadUri = downloadUri;
            this.storageDirectory = storageDirectory;
        }

        #endregion

        #region Class Methods

        ///<summary>
        /// The method retrieves resources located at a particular URI and caches these resources on the 
        /// local file system. Retrieving of the resources is performed based upon the parameters set in 
        /// the ResourceHandler constructor.
        ///</summary>
        public void CacheResourcesLocally()
        {
            foreach (string resourceFile in resources)
            {
                CacheSingleResourceLocally(downloadUri + resourceFile, storageDirectory + resourceFile);
            }
        }

        #endregion

        #region Private Class Methods

        ///<summary>
        /// The method retrieves a resource, such as an XMI file, located at a particular URI. 
        /// The content of the resource is returned as a string. 
        ///</summary>
        ///<param name="uri">
        /// A URI pointing to the resource to be retrieved. An example would be 
        /// "http://www.umm-dev.org/xmi/primlibrary.xmi".
        /// </param>
        ///<returns>
        /// A string representation from the content located at the specified URI.
        ///</returns>
        private static string RetrieveContentFromUri(string uri)
        {
            using (var client = new WebClient())
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
        private static void CacheSingleResourceLocally(string downloadUri, string outputFile)
        {
            string currentFileContent = "";
            string newFileContent = RetrieveContentFromUri(downloadUri);

            if (File.Exists(outputFile))
            {
                currentFileContent = File.ReadAllText(outputFile);
            }

            if (!string.IsNullOrEmpty(newFileContent))
            {
                if (!currentFileContent.Equals(newFileContent))
                {
                    using (StreamWriter writer = File.CreateText(outputFile))
                    {
                        writer.Write(newFileContent);
                    }
                }
            }
        }

        #endregion
    }
}