using System.Windows.Input;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialBdt
    {
        private string mName;
        private bool mChecked;
        private bool mSelected;
        private IBdt mOriginalBDT;
        private bool mItemReadOnly;
        private Cursor mItemCursor;
        private bool mItemFocusable;

        public PotentialBdt(string newBdtName, bool isChecked)
        {
            mName = newBdtName;
            mChecked = isChecked;
            mOriginalBDT = null;

            mItemReadOnly = false;
            mItemCursor = Cursors.IBeam;
            mItemFocusable = true;
        }

        public PotentialBdt(IBdt originalBdt)
        {
            mName = originalBdt.Name;
            mChecked = false;
            mOriginalBDT = originalBdt;

            mItemReadOnly = true;
            mItemCursor = Cursors.Arrow;
            mItemFocusable = false;
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

        public IBdt OriginalBdt
        {
            get { return mOriginalBDT; }
            set { mOriginalBDT = value; }
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