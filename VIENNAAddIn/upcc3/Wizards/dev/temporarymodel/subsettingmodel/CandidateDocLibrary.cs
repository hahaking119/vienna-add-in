using System.Collections.Generic;
using CctsRepository.DocLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.cache;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel
{
    public class CandidateDocLibrary
    {
        private IDocLibrary mOriginalDocLibrary;
        private bool mSelected;
        private List<CandidateRootElement> mCandidateRootElements;

        public CandidateDocLibrary(IDocLibrary docLibrary)
        {
            mOriginalDocLibrary = docLibrary;
            mSelected = false;
            mCandidateRootElements = null;
        }

        public IDocLibrary OriginalDocLibrary
        {
            set { mOriginalDocLibrary = value; }
            get { return mOriginalDocLibrary; }
        }

        public bool Selected
        {
            set { mSelected = value; }
            get { return mSelected; }
        }

        public List<CandidateRootElement> CandidateRootElements
        {
            set
            {
                mCandidateRootElements = value;    
            }

            get
            {
                if (mCandidateRootElements == null)
                {
                    mCandidateRootElements = new List<CandidateRootElement>(CcCache.GetInstance().GetMasFromDocLibrary(OriginalDocLibrary.Name).ConvertAll(ma => new CandidateRootElement(ma)));
                }

                return mCandidateRootElements;
            }
        }
    }
}