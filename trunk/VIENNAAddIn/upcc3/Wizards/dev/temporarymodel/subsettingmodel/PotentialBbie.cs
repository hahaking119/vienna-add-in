// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Windows.Input;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel
{
    public class PotentialBbie
    {
        private readonly Cursor mItemCursor;
        private readonly bool mItemFocusable;
        private readonly bool mItemReadOnly;
        private ICdt mCdtUsedInBcc;

        public PotentialBbie(string bbieName, ICdt cdtOfTheBccWhichTheBbieIsBasedOn)
        {
            Name = bbieName;
            mCdtUsedInBcc = cdtOfTheBccWhichTheBbieIsBasedOn;
            Checked = false;
            Selected = false;
            mItemReadOnly = false;
            mItemCursor = Cursors.IBeam;
            mItemFocusable = true;
        }

        public string Name { get; set; }

        public bool Checked { get; set; }

        public bool Selected { get; set; }

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