// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************


using System;
using System.Collections.Generic;
using System.Net;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    public struct VersionDescriptor
    {
        public string Major;
        public string Minor;
        public string Comment;

        public string ResourceDirectory
        {
            get { return Major + "_" + Minor; }
        }

        public override string ToString()
        {
            return string.Format("VersionDescriptor <\"{0}\", \"{1}\", \"{2}\">", Major, Minor, Comment);
        }
        
        public static VersionDescriptor ParseVersionString(string versionString)
        {                      
            string[] stringTokens = versionString.Split('|');

            if (stringTokens.Length == 3)
            {
                VersionDescriptor versionDescriptor = new VersionDescriptor
                                                          {
                                                              Major = stringTokens[0],
                                                              Minor = stringTokens[1],
                                                              Comment = stringTokens[2]
                                                          };

                return versionDescriptor;
            }
            
            throw new ArgumentException("expected version string: <Major|Minor|Comment>, but was: <{0}>", versionString);            
        }
    }

    ///<summary>
    /// TODO
    ///</summary>
    public class VersionHandler
    {
        private readonly IWebClientMediator webClientMediator;
        private readonly string uri;

        public VersionHandler(IWebClientMediator webClientMediator, string uri)
        {
            this.webClientMediator = webClientMediator;
            this.uri = uri;
        }

        public List<VersionDescriptor> AvailableVersions { get; private set;}

        public void RetrieveAvailableVersions()
        {
            AvailableVersions = new List<VersionDescriptor>();
            
            string versionsString = webClientMediator.DownloadString(uri);            

            foreach (string version in versionsString.Split('\n'))
            {
                if (version.Trim() != "")
                {
                    AvailableVersions.Add(VersionDescriptor.ParseVersionString(version));    
                }                
            }
        }

        public List<string> GetMajorVersions()
        {
            List<string> majorVersions = new List<string>();

            foreach (VersionDescriptor descriptor in AvailableVersions)
            {
                if (!(majorVersions.Contains(descriptor.Major)))
                {
                    majorVersions.Add(descriptor.Major);
                }
            }

            return majorVersions;
        }

        public List<string> GetMinorVersions(string majorVersion)
        {
            List<string> minorVersions = new List<string>();

            foreach (VersionDescriptor descriptor in AvailableVersions)
            {
                if (descriptor.Major == majorVersion)
                {
                    minorVersions.Add(descriptor.Minor);
                }
            }

            return minorVersions;            
        }

        public string GetComment(string majorVersion, string minorVersion)
        {
            foreach (VersionDescriptor descriptor in AvailableVersions)
            {
                if ((descriptor.Major == majorVersion) && (descriptor.Minor == minorVersion))
                {
                    return descriptor.Comment;                    
                }
            }

            return "";
        }
    }

    public interface IWebClientMediator
    {
        string DownloadString(string uri);
    }

    public class WebClientMediator : IWebClientMediator
    {
        public string DownloadString(string uri)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(uri);
            }
        }
    }
}