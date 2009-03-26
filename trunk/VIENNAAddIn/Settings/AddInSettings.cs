using System;

namespace VIENNAAddIn.Settings
{
    ///<summary>
    /// Global Add-In settings.
    ///</summary>
    public static class AddInSettings
    {
        // If this boolean field is set to true, the VIENNAAddIn is built in GIEM mode.
        // This means, that all menu entries are named as GIEM instead of VIENNAAddIn.
        internal const bool buildGIEM = false;

        /// <summary>
        /// Return the caption of the Add-In
        /// </summary>
        /// <returns></returns>
        public static String AddInCaption
        {
            get { return buildGIEM ? "GIEM" : "VIENNAAddIn"; }
        }
    }
}