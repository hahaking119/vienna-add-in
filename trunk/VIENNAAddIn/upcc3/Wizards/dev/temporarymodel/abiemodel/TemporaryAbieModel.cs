using System;
using System.Collections.Generic;
using System.ComponentModel;
using CctsRepository;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.binding;
using VIENNAAddIn.upcc3.Wizards.dev.cache;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class TemporaryAbieModel : TemporaryModel, INotifyPropertyChanged
    {
        // Backing Fields for Binding Properties 
        private string mAbieName;
        private string mAbiePrefix;
        private List<string> mCandidateCcLibraryNames;
        private List<string> mCandidateAccNames;
        private CcCache ccCache;

        // Class Fields
        private List<CandidateCcLibrary> mCandidateCcLibraries;
        
        // Constructor
        public TemporaryAbieModel(ICctsRepository cctsRepository)
        {
            ccCache = CcCache.GetInstance(cctsRepository);

            mCandidateCcLibraries = new List<CandidateCcLibrary>(ccCache.GetCCLibraries().ConvertAll(ccl => new CandidateCcLibrary(ccl)));

            CandidateCcLibraryNames = new List<string>(mCandidateCcLibraries.ConvertAll(new Converter<CandidateCcLibrary, string>(CandidateCcLibraryToString)));
        }

        // ConvertAll Methods
        private static string CandidateCcLibraryToString(CandidateCcLibrary candidateCcLibrary)
        {
            return candidateCcLibrary.OriginalCcLibrary.Name;
        }	

        // Binding Properties
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

        private static string CandidateAccToString(CandidateAcc candidateAcc)
        {
            return candidateAcc.OriginalAcc.Name;
        }

        public void SetSelectedCandidateAcc(string s)
        {
            throw new NotImplementedException();
        }


        // INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(Enum fieldName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(fieldName.ToString()));
            }
        }
    }
}