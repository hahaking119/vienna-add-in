namespace VIENNAAddIn.Settings
{
    partial class SynchStereotypesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CloseButton = new System.Windows.Forms.Button();
            this.missingList = new System.Windows.Forms.ListBox();
            this.FixAllButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(246, 261);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.Text = "&Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // missingList
            // 
            this.missingList.FormattingEnabled = true;
            this.missingList.Location = new System.Drawing.Point(1, 3);
            this.missingList.Name = "missingList";
            this.missingList.Size = new System.Drawing.Size(331, 251);
            this.missingList.TabIndex = 0;
            // 
            // FixAllButton
            // 
            this.FixAllButton.Location = new System.Drawing.Point(12, 261);
            this.FixAllButton.Name = "FixAllButton";
            this.FixAllButton.Size = new System.Drawing.Size(75, 23);
            this.FixAllButton.TabIndex = 1;
            this.FixAllButton.Text = "&Fix all";
            this.FixAllButton.UseVisualStyleBackColor = true;
            this.FixAllButton.Click += new System.EventHandler(this.FixAllButton_Click);
            // 
            // SynchStereotypesWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(333, 292);
            this.Controls.Add(this.FixAllButton);
            this.Controls.Add(this.missingList);
            this.Controls.Add(this.CloseButton);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SynchStereotypesWindow";
            this.Text = "Synchronize Tagged Values";
            this.Load += new System.EventHandler(this.SynchStereotypesWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ListBox missingList;
        private System.Windows.Forms.Button FixAllButton;
    }
}