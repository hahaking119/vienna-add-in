// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using System.Windows.Input;
using CctsRepository.BdtLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialBbie
    {
        private string mName;
        private bool mChecked;
        private bool mSelected;
        private ICdt mCdtUsedInBcc;
        private List<PotentialBdt> mPotentialBdts;
        private bool mItemReadOnly;
        private Cursor mItemCursor;
        private bool mItemFocusable;

        public PotentialBbie(string bbieName, ICdt cdtOfTheBccWhichTheBbieIsBasedOn)
        {
            mName = bbieName;
            mCdtUsedInBcc = cdtOfTheBccWhichTheBbieIsBasedOn;
            mChecked = false;
            mSelected = false;
            mPotentialBdts = null;
            mItemReadOnly = false;
            mItemCursor = Cursors.IBeam;
            mItemFocusable = true;
        }

        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        public bool Checked
        {
            get { return mChecked; }
            set { mChecked = value; }
        }

        public bool Selected
        {
            get { return mSelected; }
            set { mSelected = value; }
        }

        public List<PotentialBdt> PotentialBdts
        {
            get
            {
                if (mPotentialBdts == null)
                {
                    CcCache ccCache = CcCache.GetInstance();

                    mPotentialBdts = new List<PotentialBdt>();

                    foreach (IBdtLibrary bdtLibrary in ccCache.GetBdtLibraries())
                    {
                        foreach (IBdt bdt in ccCache.GetBdtsFromBdtLibrary(bdtLibrary.Name))
                        {
                            if (bdt.BasedOn.Id == mCdtUsedInBcc.Id)
                            {
                                mPotentialBdts.Add(new PotentialBdt(bdt));
                            }
                        }
                    }

                    foreach (string prospectiveBdtName in ProspectiveBdts.GetInstance().Bdts(mCdtUsedInBcc.Id))
                    {
                        AddPotentialBdt(prospectiveBdtName);
                    }                    

                    if (mPotentialBdts.Count == 0)
                    {
                        AddPotentialBdt();
                    }
                }

                return mPotentialBdts;
            }

            set { mPotentialBdts = value; }
        }

        public string AddPotentialBdt()
        {
            string newBdtName = "";

            for (int i = 1; i != -1; i++)
            {
                bool foundBdtWithTheSameName = false;
                newBdtName = "New" + i + mCdtUsedInBcc.Name;

                foreach (PotentialBdt potentialBdt in PotentialBdts)
                {
                    if (potentialBdt.Name.Equals(newBdtName))
                    {
                        foundBdtWithTheSameName = true;
                    }
                }

                if (!foundBdtWithTheSameName)
                {
                    AddPotentialBdt(newBdtName);

                    break;
                }
            }

            return newBdtName;
        }


        public void AddPotentialBdt(string newBdtName)
        {
            bool bdtAlreadyAddedByPotentialBdtsSetter = false;

            foreach (PotentialBdt potentialBdt in PotentialBdts)
            {
                if (potentialBdt.Name.Equals(newBdtName))
                {
                    bdtAlreadyAddedByPotentialBdtsSetter = true;
                }
            }
            
            if (!bdtAlreadyAddedByPotentialBdtsSetter)
            {
                PotentialBdts.Add(new PotentialBdt(newBdtName));   
            }            
        }

        public bool ItemReadOnly
        {
            get { return mItemReadOnly; }
        }

        public Cursor ItemCursor
        {
            get { return mItemCursor; }
        }

        public bool ItemFocusable
        {
            get { return mItemFocusable; }
        }
    }
}