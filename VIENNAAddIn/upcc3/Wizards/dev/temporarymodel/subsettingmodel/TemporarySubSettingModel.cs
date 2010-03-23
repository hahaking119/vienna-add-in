// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CctsRepository;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.binding;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel
{
    public class TemporarySubSettingModel : TemporaryModel, INotifyPropertyChanged
    {
        #region Backing Fields for Binding Properties

        private List<string> mCandidateDocLibraryNames;
        private List<string> mCandidateRootElementNames;
        private List<CheckableItem> mPotentialBbieItems;
        private List<CheckableItem> mCandidateRootElementItems;
        private ObservableCollection<CheckableTreeViewItem> mCandidateAbieItems;
        private string mRootElementName;
        #endregion


        #region Class Fields

        private readonly CcCache ccCache;
        private List<CandidateDocLibrary> mCandidateDocLibraries;
        private CandidateRootElement rootElement;

        #endregion
        

        public TemporarySubSettingModel(ICctsRepository cctsRepository)
        {
            mCandidateAbieItems = new ObservableCollection<CheckableTreeViewItem>();
            
            ccCache = CcCache.GetInstance(cctsRepository);
            mCandidateDocLibraries = new List<CandidateDocLibrary>(ccCache.GetDocLibraries().ConvertAll(doclib => new CandidateDocLibrary(doclib)));
            CandidateDocLibraryNames =  new List<string>(mCandidateDocLibraries.ConvertAll(new Converter<CandidateDocLibrary,string>(CandidateDocLibraryToString)));
            // Populate the model with the appropriate DOC library which contains the root MA.
        }

        #region Binding Properties

        public string RootElement
        {
            set
            {
                mRootElementName = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.RootElement);
            }
            get
            {
                return mRootElementName;
            }
        }
        public List<string> CandidateDocLibraryNames
        {
            set
            {
                mCandidateDocLibraryNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.CandidateDocLibraryNames);
            }

            get
            {
                return mCandidateDocLibraryNames;
            }
        }

        public List<string> CandidateRootElementNames
        {
            set
            {
                mCandidateRootElementNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.CandidateRootElementNames);
            }

            get
            {
                return mCandidateRootElementNames;
            }
        }

        public List<CheckableItem> PotentialBbieItems
        {
            set
            {
                mPotentialBbieItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.PotentialBbieItems);
            }

            get
            {
                return mPotentialBbieItems;
            }
        }
        public List<CheckableItem> CandidateRootElementItems
        {
            set
            {
                mCandidateRootElementItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.CandidateRootElementItems);
            }

            get
            {
                return mCandidateRootElementItems;
            }
        }
        public ObservableCollection<CheckableTreeViewItem> CandidateAbieItems
        {
            set
            {
                mCandidateAbieItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.CandidateAbieItems);
            }

            get
            {
                return mCandidateAbieItems;
            }
        }

       #endregion

      
        #region ConvertAll Methods

        private static string CandidateDocLibraryToString(CandidateDocLibrary candidateDocLibrary)
        {
            return candidateDocLibrary.OriginalDocLibrary.Name;
        }

        private static CheckableItem PotentialBbieToCheckableText(PotentialBbie potentialBbie)
        {
            return new CheckableItem(potentialBbie.Checked, potentialBbie.Name, potentialBbie.ItemReadOnly, potentialBbie.ItemFocusable, potentialBbie.ItemCursor);
        }

        private static CheckableTreeViewItem CandidateAbieToCheckableTreeViewItem(CandidateAbie candidateAbie)
        {

            return new CheckableTreeViewItem(candidateAbie.Checked,candidateAbie.Name,DFS(candidateAbie.OriginalAbie,new ObservableCollection<IAbie>()));
        }


        private static ObservableCollection<CheckableTreeViewItem> buildCheckableTreeView (CandidateRootElement ma)
        {
            var checkableTreeViewItems = new ObservableCollection<CheckableTreeViewItem>();
            foreach (IAsma asma in ma.OriginalMa.Asmas)
            {
                if(asma.AssociatedBieAggregator.IsMa)
                {
                    checkableTreeViewItems.Add(new CheckableTreeViewItem(true,asma.AssociatedBieAggregator.Ma.Name,DFS(asma.AssociatedBieAggregator.Ma,new Collection<IMa>())));
                }
                else if(asma.AssociatedBieAggregator.IsAbie)
                {
                    checkableTreeViewItems.Add(new CheckableTreeViewItem(true,asma.AssociatedBieAggregator.Abie.Name,DFS(asma.AssociatedBieAggregator.Abie,new Collection<IAbie>())));
                }
            }
            return checkableTreeViewItems;
        }
        private static ObservableCollection<CheckableTreeViewItem> DFS(IMa root, ICollection<IMa> visited)
        {
            var tempList = new ObservableCollection<CheckableTreeViewItem>();
            IEnumerator enumerator = root.Asmas.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentAsma = (IAsma)enumerator.Current;
                if (currentAsma.AssociatedBieAggregator.IsAbie)
                {
                    var currentAbie = currentAsma.AssociatedBieAggregator.Abie;
                    tempList.Add(new CheckableTreeViewItem(true, currentAbie.Name, DFS(currentAbie, new Collection<IAbie>())));
                }
                else if (currentAsma.AssociatedBieAggregator.IsMa)
                {
                    var currentMa = currentAsma.AssociatedBieAggregator.Ma;
                    if (!visited.Contains(currentMa))
                    {
                        visited.Add(currentMa);
                        tempList.Add(new CheckableTreeViewItem(true, currentMa.Name,
                                                               DFS(currentMa, visited)));
                    }
                }
            }
            return tempList;
        }
        private static ObservableCollection<CheckableTreeViewItem> DFS(IAbie root, ICollection<IAbie> visited)
        {
            var tempList = new ObservableCollection<CheckableTreeViewItem>();
            IEnumerator enumerator = root.Asbies.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentAsbie = (IAsbie) enumerator.Current;
                var currentAbie = currentAsbie.AssociatedAbie;
                    if (!visited.Contains(currentAbie))
                    {
                        visited.Add(currentAbie);
                        tempList.Add(new CheckableTreeViewItem(true,currentAbie.Name,DFS(currentAbie,visited)));
                    }
            }
            return tempList;
        }
        private static string CandidateRootElementToString(CandidateRootElement candidateRootElement)
        {
            return candidateRootElement.OriginalMa.Name;
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

        


        public void SetSelectedCandidateDocLibrary(string selectedDocLibrary)
        {
            foreach (CandidateDocLibrary candidateDocLibrary in mCandidateDocLibraries)
            {
                if (candidateDocLibrary.OriginalDocLibrary.Name.Equals(selectedDocLibrary))
                {
                    candidateDocLibrary.Selected = true;
                    rootElement = new CandidateRootElement(candidateDocLibrary.OriginalDocLibrary.DocumentRoot);
                    RootElement = rootElement.OriginalMa.Name;
                    CandidateAbieItems.Clear();
                    CandidateAbieItems = buildCheckableTreeView(rootElement);
                    //override selection of root Element!
                    //CandidateRootElementNames = new List<string>(candidateDocLibrary.CandidateRootElements.ConvertAll(new Converter<CandidateRootElement, string>(CandidateRootElementToString)));                        
                }
                else
                {
                    candidateDocLibrary.Selected = false;
                }
            }
        }

        public void SetSelectedCandidateRootElement(string selectedRootElement)
        {

            mCandidateAbieItems.Clear();
            foreach (CandidateDocLibrary candidateDocLibrary in mCandidateDocLibraries)
            {
                if (candidateDocLibrary.Selected)
                {
                    foreach (CandidateRootElement candidateRootElement in candidateDocLibrary.CandidateRootElements)
                    {
                        if (candidateRootElement.OriginalMa.Name.Equals(selectedRootElement))
                        {
                            candidateRootElement.Selected = true;
                            //fills CandidateAbieItems with hierachical content
                            buildCheckableTreeView(candidateRootElement);
                        }
                        else
                        {
                            candidateRootElement.Selected = false;
                        }
                    }
                }
            }                
        }

        public void SetDefaultChecked(CandidateAbie candidateAbie)
        {
            int index = 0;
            int indexCheckedAbie = -1;

            foreach (CandidateAbie potentialAbie in candidateAbie.PotentialAbies)
            {
                if (potentialAbie.Checked)                
                {
                    indexCheckedAbie = index;
                    break;
                }

                index++;
            }

            if (indexCheckedAbie == -1)
            {
                indexCheckedAbie = 0;
                candidateAbie.PotentialAbies[indexCheckedAbie].Checked = true;                
            }
        }

       public void SetSelectedAndCheckedPotentialBbie(string selectedBbie, bool checkedValue)
        {
            
        }

       
        public void SetSelectedAndCheckedCandidateAbie(string selectedAbie, bool checkedValue)
        {
            var result = findAbie(selectedAbie, mCandidateAbieItems);
            if (result != null)
            {
               result.Checked = checkedValue;
               checkAllChildren(result,checkedValue);
            }
            foreach (CandidateAbie abie in rootElement.CandidateAbies)
            {
                if(abie.Name.Equals(selectedAbie))
                {
                    abie.Checked = checkedValue;
                    //PotentialBbieItems = new List<CheckableItem>(abie.OriginalAbie.Bbies); TODO: add Bbies!
                }
            }
            //PotentialBbieItems = new List<CheckableItem>(result.);
        }

        private static void checkAllChildren(CheckableTreeViewItem itemToCheck, bool checkedValue)
        {
            foreach (var child in itemToCheck.Children)
            {
                child.Checked = checkedValue;
                if(child.Children!=null)
                {
                    checkAllChildren(child,checkedValue);
                }
            }
        }

        private static CheckableTreeViewItem findAbie(string selectedAbie, Collection<CheckableTreeViewItem> listToSearch)
        {
           foreach (var candidateAbieItem in listToSearch)
            {
                if (candidateAbieItem.Text.Equals(selectedAbie))
                {
                    return candidateAbieItem;
                }
                if (candidateAbieItem.Children != null)
                {
                    CheckableTreeViewItem tempItem = findAbie(selectedAbie, candidateAbieItem.Children);
                    if(tempItem!=null)
                    {
                        return tempItem;
                    }
                }
            }
            return null;
        }


        public void SetNoSelectedCandidateAbie()
        {
            CandidateAbieItems = new ObservableCollection<CheckableTreeViewItem>();
            PotentialBbieItems = new List<CheckableItem>();
        }

        public void SetSelectedAndCheckedPotentialAsbie(string selectedAsbie, bool checkedValue)
        {
            //foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            //{
            //    if (candidateCcLibrary.Selected)
            //    {
            //        foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
            //        {
            //            if (candidateAcc.Selected)
            //            {
            //                foreach (CandidateAbie candidateAbie in candidateAcc.CandidateAbies)
            //                {
            //                    foreach (PotentialAsbie potentialAsbie in candidateAbie.PotentialAsbies)
            //                    {
            //                        if (potentialAsbie.Name.Equals(selectedAsbie))
            //                        {
            //                            potentialAsbie.Selected = true;

            //                            potentialAsbie.Checked = checkedValue;
            //                        }
            //                        else
            //                        {
            //                            potentialAsbie.Selected = false;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }



        //public bool ContainsValidConfiguration()
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
        //                        if (candidateBcc.Checked)
        //                        {
        //                            foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
        //                            {
        //                                if (potentialBbie.Checked)
        //                                {
        //                                    foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
        //                                    {
        //                                        if (potentialBdt.Checked) 
        //                                        {
        //                                            return true;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}
    }
}