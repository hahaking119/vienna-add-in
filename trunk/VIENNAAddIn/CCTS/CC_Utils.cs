/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using VIENNAAddIn.Utils;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;

namespace VIENNAAddIn.CCTS {
    class CC_Utils {

        //to be used in schema generator
        internal static bool blnLinkedSchema = false;
        internal static bool blnUseAlias = false;

        /// <sUMM2ary>
        /// Checks the consistency of a Core Component Model
        /// </sUMM2ary>
        /// <param name="repo"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        internal static bool checkPackageConsistencyForQDT(EA.Repository repo, String scope) {
            //Get the BCSS package

            IList sourceCDTLibaries = new ArrayList();
            foreach (EA.Package pkg in repo.Models)
            {
                //Get a list of all CDTLibraries available under the BCSS 
                Utility.getAllSubPackagesRecursively(pkg, sourceCDTLibaries, CCTS_Types.CDTLibrary.ToString());
                
            }

            if (sourceCDTLibaries.Count == 0)
            {
                MessageBox.Show("No CDTLibraries have  been found.", "Error");
                return false;
            }
            return true;
        }




        /// <sUMM2ary>
        /// Checks the consistency of a Core Component Model
        /// </sUMM2ary>
        /// <param name="repo"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        internal static bool checkPackageConsistencyForCC(EA.Repository repo, String scope) {
            //Get the BCSS package
            
            IList sourceCCLibaries = new ArrayList();
            foreach (EA.Package pkg in repo.Models)
            {
                //Get a list of all CCLibraries available under the BCSS 
                Utility.getAllSubPackagesRecursively(pkg, sourceCCLibaries, CCTS_Types.CCLibrary.ToString());
            }
            
            if (sourceCCLibaries.Count == 0) {
                MessageBox.Show("No CCSLibraries have  been found.", "Error");
                return false;
            }
            return true;
        }






    }
}
