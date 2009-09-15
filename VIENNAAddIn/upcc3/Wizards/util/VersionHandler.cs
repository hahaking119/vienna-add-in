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

namespace VIENNAAddIn.upcc3.Wizards.util
{
    public struct VersionDescriptor
    {
        public string Version;
        public string Iteration;
        public string Comment;

        public string ResourceDirectory
        {
            get { return Version + "_" + Iteration; }
        }

        public override string ToString()
        {
            return string.Format("VersionDescriptor <\"{0}\", \"{1}\", \"{2}\">", Version, Iteration, Comment);
        }
        
        public static VersionDescriptor ParseVersionString(string versionString)
        {                      
            string[] stringTokens = versionString.Split('|');

            if (stringTokens.Length == 3)
            {
                VersionDescriptor versionDescriptor = new VersionDescriptor
                                                          {
                                                              Version = stringTokens[0],
                                                              Iteration = stringTokens[1],
                                                              Comment = stringTokens[2]
                                                          };

                return versionDescriptor;
            }
            
            throw new ArgumentException("expected version string: <Version|Iteration|Comment>, but was: <{0}>", versionString);            
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
            string versionsString = webClientMediator.DownloadString(uri);            

            AvailableVersions = new List<VersionDescriptor>();

            foreach (string version in versionsString.Split('\n'))
            {
                AvailableVersions.Add(VersionDescriptor.ParseVersionString(version));
            }
        }

        public List<VersionDescriptor> FilterDescriptors(string version)
        {
            List <VersionDescriptor> filteredDescriptors = new List<VersionDescriptor>();

            foreach (VersionDescriptor descriptor in AvailableVersions)
            {
                if (descriptor.Version == version)                
                {
                    filteredDescriptors.Add(descriptor);
                }
            }

            return filteredDescriptors;
        }

        public VersionDescriptor FilterDescriptors(string version, string iteration)
        {
            foreach (VersionDescriptor descriptor in AvailableVersions)
            {
                if ((descriptor.Version == version) && (descriptor.Iteration == iteration))
                {
                    return descriptor;
                }
            }

            throw new ArgumentException(string.Format("expected version: <{0}|{1}>, not found", version, iteration));
        }

    }

    public interface IWebClientMediator
    {
        string DownloadString(string uri);
    }

}