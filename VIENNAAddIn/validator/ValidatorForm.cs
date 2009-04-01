/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using VIENNAAddIn.validator;
using VIENNAAddIn.common.logging;
using VIENNAAddIn.ErrorReporter;

namespace VIENNAAddIn.validator
{
    public partial class ValidatorForm : Form
    {
        

        //The scope upon which the validator is activated (an EA package ID)
        private String scope;
        //The repository upon which the validator validates
        private EA.Repository repository;

        private Boolean running = false;

        //The Validationmessages
        private List<ValidationMessage> validationMessages = new List<ValidationMessage>();
        
        public ValidatorForm(EA.Repository repository, String scope)
        {
            InitializeComponent();
            
            this.scope = scope;
            this.repository = repository;
           
            setStatusText("Selected validation scope: " + getScopeInStatusBar(repository, scope) + " Press Start to invoke a validation run.");

            // initialize progress bar
            this.progressBar.Maximum = 100;
            this.progressBar.Minimum = 1;
            this.progressBar.Value = 1;
            this.progressBar.Step = 1; 

            // initialize timer ...
            this.progressTimer.Tick += new EventHandler(progressTimer_Tick);
        }

        /// <summary>
        /// Perform a Progress Bar step
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void progressTimer_Tick(object sender, EventArgs e)
        {
            this.progressBar.PerformStep(); 
        }

        /// <summary>
        /// Called as soon as the background worker responsible for the validation of the model
        /// has finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {     
                MessageBox.Show(e.Error.Message);
                this.progressBar.Value = 1; 
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled 
                // the operation.
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.
            }
            else
            {
                this.progressTimer.Stop();
                this.progressBar.Value = this.progressBar.Maximum; 

                //Calculate the elapsed time
                //                DateTime endTime = DateTime.Now;
                //double elapsedTime = (endTime.ToFileTime() - startTime.ToFileTime()) / (double)TimeSpan.TicksPerSecond;

                //Set the status message
                setStatusText("Validation complete.");
            }

            // Enable the Start button.
            this.startButton.Enabled = true;
        }

        /// <summary>
        /// DoWork method of the Background worker - responsible for carrying out the processing time intense
        /// operation of validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bworker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Deactiveate Start Button
            setStatusText("Validation started. Please wait...");

            validationMessages.Clear();

            var validationContext = new ValidationContext(repository);            
            validationContext.ValidationMessageAdded += HandleValidationMessageAdded;
            try
            {
                var validator = new Validator();
                validator.validate(validationContext, scope);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, LogType.ERROR);
                // stop worker ...
                this.bworker.CancelAsync();
                //e.WorkerEventArgs.Cancel = true;
                this.progressTimer.Stop();
                
               
                
            }
        }



        /// <summary>
        /// Handle Message Added Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleValidationMessageAdded(object sender, ValidationMessageAddedEventArgs e)
        {
            bworker.ReportProgress(0,e);                     
                      
        }

        /// <summary>
        /// Handle the newly added Validationmessage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            ValidationMessageAddedEventArgs vma = (ValidationMessageAddedEventArgs)e.UserState;
                
            validationMessages.AddRange(vma.Messages);
            fillListView();  
        }

        /// <summary>
        /// Reset the validatorForm
        /// </summary>
        public void resetValidatorForm(String scope) {

            setStatusText("Selected validation scope: " + getScopeInStatusBar(repository, scope));
            this.scope = scope;

            //Delete the old validation messages
            foreach (ListViewItem item in validationMessagesListView.Items)
            {
                validationMessagesListView.Items.Remove(item);
            }
            //Reset the progress bar
            progressBar.Value = progressBar.Minimum;

        }


        private void startButton_Click(object sender, EventArgs e)
        {
            //Is the validation currently running?
            if (!running)
            {
                running = true;
                startButton.Text = "Stop";

                //If a top-down validation of the whole model is initialized, the intervall must be higher
                if (scope == "ROOT")
                {
                    progressTimer.Interval = 800;
                }
                else
                {
                    progressTimer.Interval = 400;
                }


                //Set back the list view
                //Delete the old validation messages
                foreach (ListViewItem item in validationMessagesListView.Items)
                {
                    validationMessagesListView.Items.Remove(item);
                }
                //Reset the progress bar
                progressBar.Value = progressBar.Minimum;
                //Reset the message detail box
                this.detailsBox.Clear();


                this.startButton.Enabled = false;
                this.progressTimer.Start();

                if (bworker.IsBusy)
                    bworker.CancelAsync();

                bworker.RunWorkerAsync(validationMessages);


            }
            else
            {
                bworker.CancelAsync();
                running = false;
                startButton.Text = "Start";
            }
        }



        /// <summary>
        /// Fill the ListView with the Validationmessagedetails
        /// </summary>
        private void fillListView()
        {

            this.validationMessagesListView.BeginUpdate();

            //Delete the old validation messages
            foreach (ListViewItem item in validationMessagesListView.Items) {
                validationMessagesListView.Items.Remove(item);
            }


            //Iterate over the validation messages generated by the different validatiors
            //and add the message to the grid
            String selectedLevel = "";
            if (levelSelector.SelectedItem != null)
                selectedLevel = levelSelector.SelectedItem.ToString();

            if (this.validationMessages != null && this.validationMessages.Count != 0)
            {

                foreach (ValidationMessage message in this.validationMessages)
                {                    
                    if (selectedLevel == "" || selectedLevel.ToUpper() == "ALL")
                    {
                        addValidationMessageToList(message);
                    }
                    else if (selectedLevel.ToUpper() == "INFO")
                    {
                        if (message.ErrorLevel == ValidationMessage.errorLevelTypes.INFO)
                            addValidationMessageToList(message);
                    }
                    else if (selectedLevel.ToUpper() == "WARN")
                    {
                        if (message.ErrorLevel == ValidationMessage.errorLevelTypes.WARN)
                            addValidationMessageToList(message);
                    }
                    else if (selectedLevel.ToUpper() == "ERROR")
                    {
                        if (message.ErrorLevel == ValidationMessage.errorLevelTypes.ERROR)
                            addValidationMessageToList(message);
                    }

                }
            }
            else
            {
                //No validation messages returned - the validation was successful
                ValidationMessage successMessage = new ValidationMessage("No errors found.", "No errors were found in your model", "-", ValidationMessage.errorLevelTypes.INFO, 0);
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();
                
                this.validationMessages.Add(successMessage);
                addValidationMessageToList(successMessage);
            }

            

            this.validationMessagesListView.EndUpdate();

        }


        /// <summary>
        /// Add the validationmessage to the validation message listview
        /// </summary>
        /// <param name="message"></param>
        private void addValidationMessageToList(ValidationMessage message)
        {

            //Depending on the warning level get the appropriate color
            Color c = getColor(message.ErrorLevel);

            String msg = message.Message;
            String affectedView = message.AffectedView;
            int msgID = message.MessageID;

            Font normalFont = new Font("Tahoma", 8);


            //Add a new ListViewItem with the appropriate format
            //Add the error level
            ListViewItem item = new ListViewItem(new String[] { message.ErrorLevel.ToString() }, 0, Color.Black, c, normalFont);
            item.UseItemStyleForSubItems = false;
            //Add the affectedView
            item.SubItems.Add(affectedView, Color.Black, c, normalFont);
            //Add the message
            item.SubItems.Add(msg, Color.Black, c, normalFont);
            //Add the messageID for later retrieval of information in the detail box
            item.SubItems.Add(msgID.ToString(), Color.Black, c, normalFont);


            this.validationMessagesListView.Items.Add(item);

        }





        

        /// <summary>
        /// According to the errorlevel this method returns the backgroundcolor for the 
        /// ListView
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private Color getColor(ValidationMessage.errorLevelTypes level)
        {
            Color c = Color.White;

            if (level == ValidationMessage.errorLevelTypes.ERROR)
                c = Color.FromArgb(255, 139, 147);
            else if (level == ValidationMessage.errorLevelTypes.WARN)
                c = Color.FromArgb(255, 255, 204);
            else if (level == ValidationMessage.errorLevelTypes.INFO)
                c = Color.FromArgb(152, 251, 152);



            return c;
        }

        /// <summary>
        /// Shows only the validation messages according to the selected level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void levelSelector_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Clear the message detail box
            this.detailsBox.Clear();
            //Refill the list view
            fillListView();
        }




        private void errorLink_MouseClick(object sender, MouseEventArgs e)
        {

        }

 




        /// <summary>
        /// Set the text of the status bar
        /// </summary>
        /// <param name="s"></param>
        private void setStatusText(String s) {
            statusBar.Items["statusLabel"].Text = s;
        }

        private void validationMessagesListView_ItemSelectionChanged_1(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //Get the message id from the list
            //Through the message id, we can set the text of the detail message
            String s = "";
            try
            {
                s = this.validationMessagesListView.SelectedItems[0].SubItems[3].Text;

                if (s != null)
                {

                    foreach (ValidationMessage vmsg in this.validationMessages)
                    {

                        if (vmsg.MessageID.ToString() == s)
                        {
                            this.detailsBox.Clear();
                            setStatusText("Message selected.");
                            if (vmsg.MessageDetail == null || vmsg.MessageDetail == "")
                            {
                                this.detailsBox.AppendText("No message detail.");
                                
                            }
                            else
                            {
                                this.detailsBox.AppendText(vmsg.MessageDetail);
                            }
                            break;
                        }

                    }
                }


            }
            catch (Exception exe) { 
            };
        }



        /// <summary>
        /// Return the full package name which is scheduled as root element for the next validation run
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        private String getScopeInStatusBar(EA.Repository repository, String scope)
        {
            
    		String s = "";

			if (scope == null || scope == "") {
				s = "No scope determined.";
			}
			else if (scope.Equals("ROOT_UMM")) {
				s = "Entire UMM model.";
			}
            else if (scope.Equals("ROOT_UCC"))
            {
                s = "Entire UPCC model.";
            }
			else {
				try {
					int packageID = Int32.Parse(scope.Trim());
					EA.Package p = repository.GetPackageByID(packageID);
					s = "<<" + p.Element.Stereotype + ">> ";					
					s += p.Name;
				} catch (Exception e) {
					s = "Illegal scope detected. Please restart valdiator.";
				}
			}

            return s;
	

        }

        private void errorLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            String messageID = "";
            int packageID = 0;
            //Get the message ID of the errorneous element
            try
            {
                messageID = this.validationMessagesListView.SelectedItems[0].SubItems[3].Text;
                
            }
            catch (Exception exe) {}


            if (messageID != null && validationMessages != null)
            {
                foreach (ValidationMessage vmsg in this.validationMessages)
                {
                    //Get the ID of the erroneous element
                    if (vmsg.MessageID.ToString() == messageID)
                    {
                        packageID = vmsg.AffectedPackageID;
                        break;
                    }

                }

                if (packageID != 0)
                {
                    try
                    {
                        repository.ShowInProjectView(repository.GetPackageByID(packageID));
                        this.setStatusText("Package containing the error is highlighted in the project explorer.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace.ToString());
                    }
                }
                else
                {
                    this.setStatusText("There is no erroneous element associated with this message.");
                }
            }
            else
            {
                this.setStatusText("There is no erroneous element associated with this message.");
            }

        }





    }
}
