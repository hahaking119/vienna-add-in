/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


//using VIENNAAddIn.common;


namespace VIENNAAddIn.common
{
    class MDGSetter
    {



        /// <summary>
        /// Checks if an opened model has been previously defined as an UMM2 
        /// model
        /// </summary>
        /// <param name="repository">the model, which has been openend and will
        /// be checked now</param>
        /// <returns>true if the model has been marked as an UMM2 model, 
        /// false if not
        /// </returns>
        public static bool CheckIfModelIsUMM2Model(EA.Repository repository)
        {
            foreach (EA.ProjectIssues issue in repository.Issues)
            {
                if (issue.Name.Equals("UMM2Model"))
                {
                    MessageBox.Show("Model is defined as an UMM2 Model", "VIENNAAddIn");                    
                    return true;
                }
            }            
            return false;
        }







        /// <summary>
        /// defines a normal EA model as an UMM2 model. An "Issue" is added to the repository object,
        /// which marks the model permanently until the user chooses to revert the setting. Also
        /// an MDG file is loaded, which contains the UMM2 Profile and the UMM2 Standard Transaction
        /// Patterns
        /// </summary>
        /// <param name="repository">the model, which should be marked as UMM2 model</param>
        /// <returns>true if the model can be successfully marked and the relevant 
        /// MDG file (Profiles, Patterns) can be loaded successfully</returns>
        public static bool SetAsUMM2Model(EA.Repository repository)
        {
            // set the issue to mark the model as UMMModel
            EA.ProjectIssues pIssues = (EA.ProjectIssues)repository.Issues.AddNew("UMM2Model", "Issue");
            pIssues.Update();
            repository.Issues.Refresh();
            // display a message box after the setting
            String succMsg = "This Model is now defined as an UMM2 Model";
            
            /* load the MDG UMM2 technology file, which contains the patterns 
                         * and profiles */
            try
            {
                String mdgFile = WindowsRegistryLoader.getMDGFile();
                StreamReader sr = new StreamReader(mdgFile);
                String fileContent = sr.ReadToEnd();
                sr.Close();
                /* finally, after reading the file from the filesystem, load it into the model */
                repository.ImportTechnology(fileContent);                
                MessageBox.Show(succMsg, "VIENNAAddIn");


            }
            catch (Exception e)
            {
                String err = "The following exception occured while loading the MDG File: " + e.Message;
                MessageBox.Show(err, "UMMA2ddIn Error");                
                UnSetAsUMM2Model(repository);
                return false;
            }
            return true;

        }


        /// <summary>
        /// Unmark an EA Model, which has previously defined as an UMM2 Model. This operation
        /// also unloads the MDG technology file, which contains the UMM2 Profile and the UMM2
        /// Standard transaction patterns
        /// </summary>
        /// <param name="repository">the model which shouldnt be marked as UMM2 Model any longer</param>
        /// <returns>true, if the model can be successfully unmarked</returns>
        public static bool UnSetAsUMM2Model(EA.Repository repository)
        {
            /* iterate over all issues to find the one, which defines the model 
             * as an UMM2 model */
            EA.Collection pIssues = repository.Issues;
            for (short i = 0; i < pIssues.Count; i++)
            {
                EA.ProjectIssues pIssue = (EA.ProjectIssues)pIssues.GetAt(i);
                if (pIssue.Name.Equals("UMM2Model"))
                {
                    pIssues.DeleteAt(i, true);
                    String unSetMsg = "Model is not defined as an UMM2 Model any longer";
                    MessageBox.Show(unSetMsg, "VIENNAAddIn");                    
                    break;
                }
            }
            /* unload mdg umm2 technology */
            bool mdg_unloaded = repository.DeleteTechnology("UMM2FoundV1");
            // leave a msg if there is a problem with unloading the MDG file
            if (!mdg_unloaded)
            {
                String err = "The MDG Technology File, which contains the UMM2 Profile and some Patterns could not be unloaded";
                MessageBox.Show(err, "VIENNAAddIn Error");
            }
            else
            {
                //logger.Info("Successfully unloaded MDG file");
            }
            return true;
        }















    }
}
