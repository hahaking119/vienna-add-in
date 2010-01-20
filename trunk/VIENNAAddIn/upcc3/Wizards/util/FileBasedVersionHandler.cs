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
using VIENNAAddIn.Settings;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    ///<summary>
    /// TODO
    ///</summary>
    public class FileBasedVersionHandler : IVersionHandler
    {
        private readonly IVersionsFile versionsFile;

        public FileBasedVersionHandler(IVersionsFile versionsFile)
        {
            this.versionsFile = versionsFile;
        }

        public List<VersionDescriptor> AvailableVersions { get; private set;}

        public void RetrieveAvailableVersions()
        {
            AvailableVersions = new List<VersionDescriptor>();

            string versionsString = versionsFile.GetContent();
            
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
}