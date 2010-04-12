// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Windows.Input;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel
{
    public class PotentialAsbie
    {
        private readonly Cursor mItemCursor;
        private readonly bool mItemFocusable;
        private readonly bool mItemReadOnly;

        public PotentialAsbie(IAscc originalAscc)
        {
            Name = originalAscc.Name;
            Checked = false;
            BasedOn = originalAscc;

            mItemReadOnly = true;
            mItemCursor = Cursors.Arrow;
            mItemFocusable = false;
        }

        public string Name { get; set; }

        public bool Checked { get; set; }

        public bool Selected { get; set; }

        public IAscc BasedOn { get; set; }

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