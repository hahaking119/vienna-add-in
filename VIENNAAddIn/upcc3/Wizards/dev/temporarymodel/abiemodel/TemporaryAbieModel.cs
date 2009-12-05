﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.binding;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class TemporaryAbieModel : TemporaryModel, INotifyPropertyChanged
    {
        #region Backing Fields for Binding Properties

        private string mAbieName;
        private string mAbiePrefix;
        private List<string> mCandidateCcLibraryNames;
        private List<string> mCandidateAccNames;
        private List<string> mCandidateBdtLibraryNames;
        private List<string> mCandidateBieLibraryNames;
        private List<CheckableItem> mCandidateBccItems;
        private List<CheckableItem> mPotentialBbieItems;
        private List<CheckableItem> mPotentialBdtItems;
        private List<CheckableItem> mCandidateAbieItems;
        private List<CheckableItem> mPotentialAsbieItems;
        #endregion


        #region Class Fields

        private readonly CcCache ccCache;
        private List<CandidateCcLibrary> mCandidateCcLibraries;
        private List<CandidateBdtLibrary> mCandidateBdtLibraries;
        private List<CandidateBieLibrary> mCandidateBieLibraries;

        #endregion


        #region Constructor

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

        #endregion


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

        public List<CheckableItem> CandidateBccItems
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

        public List<CheckableItem> PotentialBbieItems
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

        public List<CheckableItem> PotentialBdtItems
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

        public List<CheckableItem> CandidateAbieItems
        {
            set
            {
                mCandidateAbieItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateAbieItems);
            }

            get
            {
                return mCandidateAbieItems;
            }
        }

        public List<CheckableItem> PotentialAsbieItems
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
        // TODO: eliminate all convert methods with the "=>" expression

        private static string CandidateCcLibraryToString(CandidateCcLibrary candidateCcLibrary)
        {
            return candidateCcLibrary.OriginalCcLibrary.Name;
        }

        private static string CandidateAccToString(CandidateAcc candidateAcc)
        {
            return candidateAcc.OriginalAcc.Name;
        }

        private static CheckableItem CandidateBccToCheckableItem(CandidateBcc candidateBcc)
        {
            return new CheckableItem(candidateBcc.Checked, candidateBcc.OriginalBcc.Name);
        }

        private static CheckableItem PotentialBbieToCheckableText(PotentialBbie potentialBbie)
        {
            return new CheckableItem(potentialBbie.Checked, potentialBbie.Name);
        }

        private static CheckableItem PotentialBdtToCheckableText(PotentialBdt potentialBdt)
        {
            return new CheckableItem(potentialBdt.Checked, potentialBdt.Name);
        }

        private static CheckableItem CandidateAbieToCheckableItem(CandidateAbie candidateAbie)
        {
            return new CheckableItem(candidateAbie.Checked, candidateAbie.OriginalAbie.Name);
        }

        private static CheckableItem PotentialAsbieToCheckableItem(PotentialAsbie potentialAsbie)
        {
            return new CheckableItem(potentialAsbie.Checked, potentialAsbie.Name);
        }

        private static string CandidateBdtLibraryToString(CandidateBdtLibrary candidateBdtLibrary)
        {
            return candidateBdtLibrary.OriginalBdtLibrary.Name;
        }

        private static string CandidateBieLibraryToString(CandidateBieLibrary candidateBieLibrary)
        {
            return candidateBieLibrary.OriginalBieLibrary.Name;
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

                            CandidateBccItems = new List<CheckableItem>(candidateAcc.CandidateBccs.ConvertAll(new Converter<CandidateBcc, CheckableItem>(CandidateBccToCheckableItem)));
                            CandidateAbieItems = new List<CheckableItem>(candidateAcc.CandidateAbies.ConvertAll(new Converter<CandidateAbie, CheckableItem>(CandidateAbieToCheckableItem)));
                        }
                        else
                        {
                            candidateAcc.Selected = false;
                        }
                    }
                }
            }                
        }

        public void SetSelectedAndCheckedCandidateBcc(string selectedBcc, bool? checkedValue)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.OriginalBcc.Name.Equals(selectedBcc))
                                {
                                    candidateBcc.Selected = true;

                                    if (checkedValue.HasValue)
                                    {
                                        candidateBcc.Checked = checkedValue.Value;
                                    }

                                    PotentialBbieItems = new List<CheckableItem>(candidateBcc.PotentialBbies.ConvertAll(new Converter<PotentialBbie, CheckableItem>(PotentialBbieToCheckableText)));
                                }
                                else
                                {
                                    candidateBcc.Selected = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetCheckedForAllCandidateBccs(bool checkedValue)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                candidateBcc.Checked = checkedValue;
                            }

                            CandidateBccItems = new List<CheckableItem>(candidateAcc.CandidateBccs.ConvertAll(new Converter<CandidateBcc, CheckableItem>(CandidateBccToCheckableItem)));
                        }
                    }
                }
            }
        }

        //public void SetCheckedDefaultsForPotentialBdtsAndPotentialBdts()
        //{
        //    foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
        //    {
        //        if (candidateCcLibrary.Selected)
        //        {
        //            foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
        //            {
        //                if (candidateAcc.Selected)
        //                {
        //                    foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
        //                    {
        //                        if (candidateBcc.Selected)
        //                        {
        //                            PotentialBbie potentialBbieToBeChecked = (candidateBcc.PotentialBbies.Count > 0) ? candidateBcc.PotentialBbies[0] : null;
        //                            int numberOfCheckedBbies = 0;

        //                            foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
        //                            {
        //                                if (potentialBbie.Checked)
        //                                {
        //                                    numberOfCheckedBbies++;
        //                                }
        //                            }

        //                            if ((potentialBbieToBeChecked != null) && (numberOfCheckedBbies == 0))
        //                            {
        //                                potentialBbieToBeChecked.Checked = true;                                        
        //                            }

        //                            PotentialBbieItems = new List<CheckableItem>(candidateBcc.PotentialBbies.ConvertAll(new Converter<PotentialBbie, CheckableItem>(PotentialBbieToCheckableText)));
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}


        public void SetSelectedAndCheckedPotentialBbie(string selectedBbie, bool? checkedValue)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.Selected)
                                {
                                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                    {
                                        if (potentialBbie.Name.Equals(selectedBbie))
                                        {
                                            potentialBbie.Selected = true;

                                            if (checkedValue.HasValue)
                                            {
                                                potentialBbie.Checked = checkedValue.Value;
                                            }

                                            PotentialBdtItems = new List<CheckableItem>(potentialBbie.PotentialBdts.ConvertAll(new Converter<PotentialBdt, CheckableItem>(PotentialBdtToCheckableText)));
                                        }
                                        else
                                        {
                                            potentialBbie.Selected = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetSelectedAndCheckedPotentialBdt(string selectedBdt, bool? checkedValue)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.Selected)
                                {
                                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                    {
                                        if (potentialBbie.Selected)
                                        {
                                            foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
                                            {
                                                if (potentialBdt.Name.Equals(selectedBdt))
                                                {
                                                    potentialBdt.Selected = true;

                                                    if (checkedValue.HasValue)
                                                    {
                                                        potentialBdt.Checked = checkedValue.Value;
                                                    }
                                                }
                                                else
                                                {
                                                    if ((checkedValue.HasValue) && (checkedValue.Value))
                                                    {
                                                        potentialBdt.Checked = false;
                                                    }
                                                }
                                            }                                            
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetSelectedAndCheckedCandidateAbie(string selectedAbie, bool? checkedValue)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateAbie candidateAbie in candidateAcc.CandidateAbies)
                            {
                                if (candidateAbie.OriginalAbie.Name.Equals(selectedAbie))
                                {
                                    candidateAbie.Selected = true;

                                    if (checkedValue.HasValue)
                                    {
                                        candidateAbie.Checked = checkedValue.Value;
                                    }
                                    
                                    PotentialAsbieItems = new List<CheckableItem>(candidateAbie.PotentialAsbies.ConvertAll(new Converter<PotentialAsbie, CheckableItem>(PotentialAsbieToCheckableItem)));
                                }
                                else
                                {
                                    candidateAbie.Selected = false;
                                }
                            }
                        }
                    }
                }
            }
        }


        public void SetNoSelectedCandidateAbie()
        {
            CandidateAbieItems = new List<CheckableItem>();
            PotentialAsbieItems = new List<CheckableItem>();            
        }

        public void SetSelectedAndCheckedPotentialAsbie(string selectedAsbie, bool? checkedValue)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateAbie candidateAbie in candidateAcc.CandidateAbies)
                            {
                                foreach (PotentialAsbie potentialAsbie in candidateAbie.PotentialAsbies)
                                {
                                    if (potentialAsbie.Name.Equals(selectedAsbie))
                                    {
                                        potentialAsbie.Selected = true;

                                        if (checkedValue.HasValue)
                                        {
                                            potentialAsbie.Checked = checkedValue.Value;
                                        }                                        
                                    }
                                    else
                                    {
                                        potentialAsbie.Selected = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
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

        public void AddPotentialBbie()
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.Selected)
                                {
                                    candidateBcc.AddPotentialBbie();
                                    PotentialBbieItems = new List<CheckableItem>(candidateBcc.PotentialBbies.ConvertAll(new Converter<PotentialBbie, CheckableItem>(PotentialBbieToCheckableText)));
                                    
                                    return;
                                }
                            }
                        }
                    }
                }
            }            
        }

        public void UpdateBbieName(string updatedBbieName)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.Selected)
                                {
                                    if (updatedBbieName.EndsWith(candidateBcc.OriginalBcc.Name))
                                    {
                                        foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                        {
                                            if (potentialBbie.Name.Equals(updatedBbieName))
                                            {
                                                throw new TemporaryAbieModelException(String.Format("The name of the BBIE is invalid since another BBIE with the same name already exists. An example for a valid BBIE name would be \"My{0}\".", updatedBbieName));                                                
                                            }
                                        }

                                        foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                        {
                                            if (potentialBbie.Selected)
                                            {
                                                potentialBbie.Name = updatedBbieName;
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new TemporaryAbieModelException(String.Format("The name of the BBIE is invalid since it must end with the name of the BCC that it is based on. An example for a valid BBIE name would be \"My{0}\".", candidateBcc.OriginalBcc.Name));
                                    }                                    
                                }
                            }
                        }
                    }
                }
            }            
        }

        public bool ContainsValidConfiguration()
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.Checked)
                                {
                                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                    {
                                        if (potentialBbie.Checked)
                                        {
                                            foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
                                            {
                                                if (potentialBdt.Checked)
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public void CreateAbie()
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            List<BbieSpec> bbieSpecs = CumulateBbieSpecs(candidateAcc);
                            List<AsbieSpec> asbieSpecs = CumulateAsbieSpecs(candidateAcc);
                            AbieSpec abieSpec = CumulateAbieSpec(candidateAcc);

                            abieSpec.Name = AbiePrefix + AbieName;
                            abieSpec.Bbies = bbieSpecs;
                            abieSpec.Asbies = asbieSpecs;

                            CreateAbieInBieLibrary(abieSpec);                                    
                        }
                    }
                }
            }            
        }

        private void CreateAbieInBieLibrary(AbieSpec abieSpec)
        {
            foreach (CandidateBieLibrary candidateBieLibrary in mCandidateBieLibraries)
            {
                if (candidateBieLibrary.Selected)
                {
                    foreach (IAbie abie in ccCache.GetBiesFromBieLibrary(candidateBieLibrary.OriginalBieLibrary.Name))
                    {
                        if (abie.Name.Equals(abieSpec.Name))
                        {
                            throw new TemporaryAbieModelException(
                                String.Format(
                                    "An ABIE named \"{0}\" already exists in the BIE Library currently selected. Choose a different name for the ABIE or select a different BIE Library which does not contain a ABIE with the same name either.",
                                    abieSpec.Name));
                        }
                    }

                    candidateBieLibrary.OriginalBieLibrary.CreateAbie(abieSpec);
                }
            }                        
        }

        private List<AsbieSpec> CumulateAsbieSpecs(CandidateAcc candidateAcc)
        {
            List<AsbieSpec> asbieSpecs = new List<AsbieSpec>();

            foreach (CandidateAbie candidateAbie in candidateAcc.CandidateAbies)
            {
                foreach (PotentialAsbie potentialAsbie in candidateAbie.PotentialAsbies)
                {                    
                    asbieSpecs.Add(AsbieSpec.CloneASCC(potentialAsbie.BasedOn, potentialAsbie.Name, candidateAbie.OriginalAbie.Id));
                }
            }

            return asbieSpecs;
        }

        private static List<BbieSpec> CumulateBbieSpecs(CandidateAcc candidateAcc)
        {
            List<BbieSpec> bbieSpecs = new List<BbieSpec>();

            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
            {
                if (candidateBcc.Checked)
                {
                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                    {
                        if (potentialBbie.Checked)
                        {
                            IBdt bdtTypifingTheBbie = null;

                            foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
                            {
                                if (potentialBdt.Checked)
                                {
                                    bdtTypifingTheBbie = potentialBdt.OriginalBdt;
                                    break;
                                }
                            }

                            BbieSpec bbieSpec = BbieSpec.CloneBCC(candidateBcc.OriginalBcc, bdtTypifingTheBbie);
                            bbieSpec.Name = potentialBbie.Name;
                            bbieSpecs.Add(bbieSpec);
                        }
                    }
                }
            }

            return bbieSpecs;
        }

        private static AbieSpec CumulateAbieSpec(CandidateAcc candidateAcc)
        {
            AbieSpec abieSpec = new AbieSpec
            {
                DictionaryEntryName = candidateAcc.OriginalAcc.DictionaryEntryName,
                Definition = candidateAcc.OriginalAcc.Definition,
                UniqueIdentifier = candidateAcc.OriginalAcc.UniqueIdentifier,
                VersionIdentifier = candidateAcc.OriginalAcc.VersionIdentifier,
                LanguageCode = candidateAcc.OriginalAcc.LanguageCode,
                BusinessTerms = candidateAcc.OriginalAcc.BusinessTerms,
                UsageRules = candidateAcc.OriginalAcc.UsageRules,
                BasedOn = candidateAcc.OriginalAcc,
            };

            return abieSpec;
        }
    }
}