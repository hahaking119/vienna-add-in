using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.menu;

namespace VIENNAAddIn.Settings
{
    public partial class SynchStereotypesForm : Form
    {
        private static SynchStereotypesForm synchStereotypesForm;

        //The repository which is checked
        private Repository repo;

        public SynchStereotypesForm(Repository repository)
        {
            InitializeComponent();

            this.repo = repository;
        }

        public static void ShowForm(AddInContext context)
        {
            if (synchStereotypesForm == null || synchStereotypesForm.IsDisposed)
            {
                synchStereotypesForm = new SynchStereotypesForm(context.EARepository);
                synchStereotypesForm.Show();
            }
            else
            {
                synchStereotypesForm.resetSynchStereotypesForm();
                synchStereotypesForm.Select();
                synchStereotypesForm.Focus();
                synchStereotypesForm.Show();
            }
        }

        public void resetSynchStereotypesForm()
        {
            missingList.Items.Clear();
        }

        private void SynchStereotypesWindow_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            var ss = new SynchStereotypes();
            var result = new List<List<String>>();
            var tempList = new List<String>();
            String packagename, modelname;
            bool first;
            var pp = (Package)repo.Models.GetAt(0);
            pp = (Package)pp.Packages.GetAt(0);
            this.missingList.Items.Clear();
            ss.Check(pp);
            foreach (Package m in repo.Models)
            {
                modelname = m.Name;
                this.missingList.Items.Add("Checking model '" + modelname + "':");
                foreach (Package p in m.Packages)
                {
                    result = ss.Check(p);
                    foreach (List<String> l in result)
                    {
                        tempList.Clear();
                        packagename = "";
                        first = true;
                        foreach (String s in l)
                        {
                            if (first)
                            {
                                packagename = s;
                                first = false;
                            }
                            else
                            {
                                tempList.Add("        TaggedValue '" + s + "' is missing");
                            }
                        }
                        tempList.Insert(0, "    Checking package '" + packagename + "'... " + tempList.Count + " missing values:");
                        this.missingList.Items.AddRange(tempList.ToArray());
                    }
                }
                this.missingList.Items.Add("Finished checking model '" + modelname + "'");
            }
        }

        private void FixAllButton_Click(object sender, EventArgs e)
        {
            var ss = new SynchStereotypes();
            ss.Fix(this.repo);
            RefreshList();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
