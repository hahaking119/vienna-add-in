using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VIENNAAddIn.Settings
{
    class AddInSettings
    {
        //If this boolean field is set to true, the VIENNAAddIn is built
        //in GIEM mode
        //This means, that all menu entries are named as GIEM instead
        //of VIENNAAddIn


        static bool buildGIEM = true;


        /// <summary>
        /// Return the caption of the AddIn
        /// </summary>
        /// <returns></returns>
        internal static String getAddInCaption()
        {
            if (buildGIEM)
            {
                return "GIEM";
            }
            else
            {
                return "VIENNAAddIn";
            }
        }



    }
}
