using System;
using System.Collections.Generic;
using System.ComponentModel;
using CctsRepository;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.binding;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class TemporaryAbieModel : TemporaryModel, INotifyPropertyChanged
    {
        // Backing Fields for Binding Properties 
        private string mAbieName;
        private string mAbiePrefix;
        private List<string> mCandidateCcLibraryNames;
        private List<string> mCandidateAccNames;
        private List<string> mCandidateBdtLibraryNames;
        private List<string> mCandidateBieLibraryNames;
        private List<CheckableText> mCandidateBccItems;
        private List<CheckableText> mPotentialBbieItems;
        private List<CheckableText> mPotentialBdtItems;
        private List<CheckableText> mCandidateAsccItems;
        private List<CheckableText> mPotentialAsbieItems;
        

        // Class Fields
        private readonly CcCache ccCache;
        private List<CandidateCcLibrary> mCandidateCcLibraries;
        private List<CandidateBdtLibrary> mCandidateBdtLibraries;
        private List<CandidateBieLibrary> mCandidateBieLibraries;

        // Constructor
        public TemporaryAbieModel(ICctsRepository cctsRepository)
        {
            ccCache = CcCache.GetInstance(cctsRepository);

            mCandidateCcLibraries = new List<CandidateCcLibrary>(ccCache.GetCCLibraries().ConvertAll(ccl => new CandidateCcLibrary(ccl)));
            CandidateCcLibraryNames = new List<string>(mCandidateCcLibraries.ConvertAll(new Converter<CandidateCcLibrary, string>(CandidateCcLibraryToString)));

            mCandidateBdtLibraries = new List<CandidateBdtLibrary>(ccCache.GetBDTLibraries().ConvertAll(bdtl => new CandidateBdtLibrary(bdtl)));
            CandidateBdtLibraryNames = new List<string>(mCandidateBdtLibraries.ConvertAll(new Converter<CandidateBdtLibrary, string>(CandidateBdtLibraryToString)));

            mCandidateBieLibraries = new List<CandidateBieLibrary>(ccCache.GetBIELibraries().ConvertAll(biel => new CandidateBieLibrary(biel)));
            CandidateBieLibraryNames = new List<string>(mCandidateBieLibraries.ConvertAll(new Converter<CandidateBieLibrary, string>(CandidateBieLibraryToString)));
        }


        #region Binding Properties

        public string AbieName
        {
            get { return mAbieName; }
            set
            {
                mAbieName = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.AbieName);                
            }
        }

        public string AbiePrefix
        {
            get { return mAbiePrefix; }
            set
            {
                mAbiePrefix = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.AbiePrefix);
            }
        }

        public List<string> CandidateCcLibraryNames
        {
            set
            {
                mCandidateCcLibraryNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateCcLibraryNames);
            }

            get
            {
                return mCandidateCcLibraryNames;
            }
        }

        public List<string> CandidateAccNames
        {
            set
            {
                mCandidateAccNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateAccNames);
            }

            get
            {
                return mCandidateAccNames;
            }
        }

        public List<string> CandidateBdtLibraryNames
        {
            set
            {
                mCandidateBdtLibraryNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateBdtLibraryNames);
            }

            get
            {
                return mCandidateBdtLibraryNames;                
            }
        }

        public List<string> CandidateBieLibraryNames
        {
            set
            {
                mCandidateBieLibraryNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateBieLibraryNames);
            }

            get
            {
                return mCandidateBieLibraryNames;
            }
        }

        public List<CheckableText> CandidateBccItems
        {
            set
            {
                mCandidateBccItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateBccItems);
            }

            get
            {
                return mCandidateBccItems;
            }
        }

        public List<CheckableText> PotentialBbieItems
        {
            set
            {
                mPotentialBbieItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.PotentialBbieItems);
            }

            get
            {
                return mPotentialBbieItems;
            }
        }

        public List<CheckableText> PotentialBdtItems
        {
            set
            {
                mPotentialBdtItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.PotentialBdtItems);
            }

            get
            {
                return mPotentialBdtItems;
            }
        }

        public List<CheckableText> CandidateAsccItems
        {
            set
            {
                mCandidateAsccItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateAsccItems);
            }

            get
            {
                return mCandidateAsccItems;
            }
        }

        public List<CheckableText> PotentialAsbieItems
        {
            set
            {
                mPotentialAsbieItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.PotentialAsbieItems);
            }

            get
            {
                return mPotentialAsbieItems;
            }
        }

        #endregion


        #region ConvertAll Methods

        private static string CandidateCcLibraryToString(CandidateCcLibrary candidateCcLibrary)
        {
            return candidateCcLibrary.OriginalCcLibrary.Name;
        }

        private static string CandidateBdtLibraryToString(CandidateBdtLibrary candidateBdtLibrary)
        {
            return candidateBdtLibrary.OriginalBdtLibrary.Name;
        }

        private static string CandidateBieLibraryToString(CandidateBieLibrary candidateBieLibrary)
        {
            return candidateBieLibrary.OriginalBieLibrary.Name;
        }

        private static CheckableText CandidateBccToCheckableItem(CandidateBcc candidateBcc)
        {
            return new CheckableText(candidateBcc.Checked, candidateBcc.OriginalBcc.Name);
        }

        private static string CandidateAccToString(CandidateAcc candidateAcc)
        {
            return candidateAcc.OriginalAcc.Name;
        }

        #endregion


        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(Enum fieldName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(fieldName.ToString()));
            }
        }

        #endregion


        // Methods to update the TemporaryABIEModel
        public void SetSelectedCandidateCcLibrary(string selectedCcLibrary)
        {
            foreach (CandidateCcLibrary candidateCCLibrary in mCandidateCcLibraries)
            {
                if (candidateCCLibrary.OriginalCcLibrary.Name.Equals(selectedCcLibrary))
                {
                    candidateCCLibrary.Selected = true;

                    CandidateAccNames = new List<string>(candidateCCLibrary.CandidateAccs.ConvertAll(new Converter<CandidateAcc, string>(CandidateAccToString)));                        
                }
                else
                {
                    candidateCCLibrary.Selected = false;
                }
            }
        }	        

        public void SetSelectedCandidateAcc(string selectedAcc)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.OriginalAcc.Name.Equals(selectedAcc))
                        {
                            candidateAcc.Selected = true;

                            CandidateBccItems = new List<CheckableText>(candidateAcc.CandidateBccs.ConvertAll(new Converter<CandidateBcc, CheckableText>(CandidateBccToCheckableItem)));
                        }
                        else
                        {
                            candidateAcc.Selected = false;
                        }
                    }
                }
            }                
        }

        public void SetSelectedCandidateBcc(string selectedBcc)
        {
            throw new NotImplementedException();
        }

        public void SetSelectedPotentialBbie(string selectedBbie)
        {
            throw new NotImplementedException();
        }

        public void SetSelectedCandidateAbie(string selectedAbcc)
        {
            throw new NotImplementedException();
        }

        public void SetSelectedCandidateBdtLibrary(string selectedBdtLibrary)
        {
            foreach (CandidateBdtLibrary candidateBdtLibrary in mCandidateBdtLibraries)
            {
                if (candidateBdtLibrary.OriginalBdtLibrary.Name.Equals(selectedBdtLibrary))
                {
                    candidateBdtLibrary.Selected = true;
                }
                else
                {
                    candidateBdtLibrary.Selected = false;
                }
            }
        }

        public void SetSelectedCandidateBieLibrary(string selectedBieLibrary)
        {
            foreach (CandidateBieLibrary candidateBieLibrary in mCandidateBieLibraries)
            {
                if (candidateBieLibrary.OriginalBieLibrary.Name.Equals(selectedBieLibrary))
                {
                    candidateBieLibrary.Selected = true;
                }
                else
                {
                    candidateBieLibrary.Selected = false;
                }
            }
        }

        public void AddBbie()
        {
            throw new NotImplementedException();
        }

        public void UpdateBbieName()
        {
            throw new NotImplementedException();
        }

        public void UpdateBdtName()
        {
            throw new NotImplementedException();
        }
    }
}