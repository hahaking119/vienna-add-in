/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using Microsoft.Win32;

using VIENNAAddIn.Exceptions;

namespace VIENNAAddIn.registry
{
    /// <summary>
    /// Loads specific values from the registry, which are needed
    /// for application configuration
    /// </summary>
    internal class WindowsRegistryLoader
    {
        /* the registry key where to search the UMM2 AddIn
         * related values are set */
        private const String VIENNA_ADDIN_REGISTRY_KEY = "SOFTWARE\\VIENNAAddIn";
        /* the base key where the other keys are located under.
         * assigned by the static constructor */
        private static RegistryKey VIENNAAddInKey;

        static WindowsRegistryLoader()
        {
            // start from HKEY_CURRENT_USER
            RegistryKey currentUser = Registry.CurrentUser;
            // open the UMMAddIn SubKey
            VIENNAAddInKey = currentUser.OpenSubKey(VIENNA_ADDIN_REGISTRY_KEY);
            /* if the key is null, the addin is installed for everyone and not just 
             * for one user. thus the key lies in the HKLM */
            if (VIENNAAddInKey == null)
            {
                RegistryKey localmachine = Registry.LocalMachine;
                VIENNAAddInKey = localmachine.OpenSubKey(VIENNA_ADDIN_REGISTRY_KEY);
            }
        }
        /// <summary>
        /// simply checks if all other registry retrieval methods are working
        /// without exceptions
        /// </summary>
        internal static void checkRegistryEntries()
        {
            try
            {
                getHomeDirectory();
                getMDGFile();
                getImagesLocation();
            }
            catch (Exception ex)
            { 
                throw new RegistryAccessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// returns the directory, where the AddIn is installed
        /// </summary>
        /// <returns>directory, where the UMMAddIn is installed</returns>
        internal static String getHomeDirectory()
        {
            String homeDir = (String)VIENNAAddInKey.GetValue("homedir");
            if (homeDir == null || homeDir.Trim().Length == 0)
            {
                throw new RegistryAccessException("HomeDir Key not found");
            }
            return homeDir;
        }
        /// <summary>
        /// returns the absolute location of the MDG file.
        /// </summary>
        /// <returns>relative location of MDG file</returns>
        internal static String getMDGFile()
        {
            String mdgfileLocation = (String)VIENNAAddInKey.GetValue("mdgfile");
            if (mdgfileLocation == null || mdgfileLocation.Trim().Length == 0)
            {
                throw new RegistryAccessException("MDGFile Key not found");
            }
            return getHomeDirectory() + mdgfileLocation;
        }

        /// <summary>
        /// returns the absolute location of the MDG file.
        /// </summary>
        /// <returns>relative location of MDG file</returns>
        internal static String getMDGFileGIEM()
        {
            String mdgfileLocation = (String)VIENNAAddInKey.GetValue("mdgfilegiem");
            if (mdgfileLocation == null || mdgfileLocation.Trim().Length == 0)
            {
                throw new RegistryAccessException("MDGFile Key not found");
            }
            return getHomeDirectory() + mdgfileLocation;
        }

        /// <summary>
        /// returns the absoluate location of the images 
        /// </summary>
        /// <returns></returns>
        internal static String getImagesLocation()
        {
            String imageLocation = (String)VIENNAAddInKey.GetValue("images");
            if (imageLocation == null || imageLocation.Trim().Length == 0)
            {
                throw new RegistryAccessException("Image location not found");
            }
            return getHomeDirectory() + imageLocation;
        }
        
    }
}
