/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using Microsoft.Win32;
using VIENNAAddIn.Exceptions;

namespace VIENNAAddIn.common
{
	/// <sUMM2ary>
	/// Loads specific values from the registry, which are needed
	/// for application configuration
	/// </sUMM2ary>
	internal class WindowsRegistryLoader
	{	
		/* the registry key where to search the UMM2 AddIn
		 * related values are set */
		private const String VIENNAAddIn_ADDIN_REGISTRY_KEY = "SOFTWARE\\VIENNAAddIn";
		/* the base key where the other keys are located under.
		 * assigned by the static constructor */
		private static RegistryKey VIENNAAddInAddInKey;

		static WindowsRegistryLoader()
		{
			// start from HKEY_CURRENT_USER
			RegistryKey currentUser = Registry.CurrentUser;
			// open the VIENNAAddIn SubKey
			VIENNAAddInAddInKey = currentUser.OpenSubKey(VIENNAAddIn_ADDIN_REGISTRY_KEY);
			/* if the key is null, the addin is installed for everyone and not just 
			 * for one user. thus the key lies in the HKLM */
			if(VIENNAAddInAddInKey == null) {
				RegistryKey localmachine = Registry.LocalMachine;
				VIENNAAddInAddInKey = localmachine.OpenSubKey(VIENNAAddIn_ADDIN_REGISTRY_KEY);
			}
		}
		/// <sUMM2ary>
		/// simply checks if all other registry retrieval methods are working
		/// without exceptions
		/// </sUMM2ary>
		internal static void checkRegistryEntries() {
			try {
				getHomeDirectory();
				getMDGFile();
				getImagesLocation();
			}
			catch(Exception ex) {
				throw new RegistryAccessException(ex.Message,ex);
			}
		}
		/// <sUMM2ary>
		/// returns the directory, where the AddIn is installed
		/// </sUMM2ary>
		/// <returns>directory, where the VIENNAAddIn is installed</returns>
		internal static String getHomeDirectory() {
			String homeDir = (String)VIENNAAddInAddInKey.GetValue("homedir");
			if(homeDir == null ||homeDir.Trim().Length == 0) {
				throw new RegistryAccessException("HomeDir Key not found");
			}
			return homeDir;
		}
		/// <sUMM2ary>
		/// returns the absolute location of the MDG file.
		/// </sUMM2ary>
		/// <returns>relative location of MDG file</returns>
		internal static String getMDGFile() {
			String mdgfileLocation = (String)VIENNAAddInAddInKey.GetValue("mdgfile");
			if(mdgfileLocation == null ||mdgfileLocation.Trim().Length == 0) {
				throw new RegistryAccessException("MDGFile Key not found");
			}
			return getHomeDirectory() + mdgfileLocation;
		}
        /// <sUMM2ary>
        /// returns the absolute location of the CCLibrary file.
        /// </sUMM2ary>
        /// <returns>relative location of cc library file</returns>
        internal static String getCCLibraryFile() {
            String ccLocation = (String)VIENNAAddInAddInKey.GetValue("cclibrary");
            if (ccLocation == null || ccLocation.Trim().Length == 0) {
                throw new RegistryAccessException("CCLibraryFile Key not found");
            }
            return getHomeDirectory() + ccLocation;
        }

        /// <sUMM2ary>
        /// returns the absoluate location of the BPEL template 
        /// </sUMM2ary>
        /// <returns></returns>
        internal static String getBpelTemplateLocation()
        {
            String bpelTemplateLocation = (String)VIENNAAddInAddInKey.GetValue("bpelTemplate");
            if (bpelTemplateLocation == null || bpelTemplateLocation.Trim().Length == 0)
            {
                throw new RegistryAccessException("BPEL template location not found");
            }
            return getHomeDirectory() + bpelTemplateLocation;
        }
        
		/// <sUMM2ary>
		/// returns the absoluate location of the images 
		/// </sUMM2ary>
		/// <returns></returns>
		internal static String getImagesLocation() 
		{
			String imageLocation = (String)VIENNAAddInAddInKey.GetValue("images");
			if(imageLocation == null || imageLocation.Trim().Length == 0) 
			{
				throw new RegistryAccessException("Image location not found");
			}
			return getHomeDirectory() + imageLocation;
		}

        
		

	}
}
