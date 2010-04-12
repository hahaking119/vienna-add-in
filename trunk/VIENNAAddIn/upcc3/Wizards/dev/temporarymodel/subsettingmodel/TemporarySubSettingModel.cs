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
using System.Windows.Input;
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

        private List<CheckableTreeViewItem> mCandidateAbieItems;
        private List<string> mCandidateDocLibraryNames;
        private List<CheckableItem> mCandidateRootElementItems;
        private List<string> mCandidateRootElementNames;
        private List<CheckableItem> mPotentialBbieItems;
        private string mRootElementName;

        #endregion

        #region Class Fields

        private readonly CcCache ccCache;
        private readonly List<CandidateDocLibrary> mCandidateDocLibraries;
        private CandidateRootElement rootElement;

        #endregion

        public TemporarySubSettingModel(ICctsRepository cctsRepository)
        {
            mCandidateAbieItems = new List<CheckableTreeViewItem>();

            ccCache = CcCache.GetInstance(cctsRepository);

            mCandidateDocLibraries =
                new List<CandidateDocLibrary>(
                    ccCache.GetDocLibraries().ConvertAll(doclib => new CandidateDocLibrary(doclib)));
            CandidateDocLibraryNames =
                new List<string>(mCandidateDocLibraries.ConvertAll(doclib => doclib.OriginalDocLibrary.Name));
            // Populate the model with the appropriate DOC library which contains the root MA.
        }

        #region Binding Properties

        public string currentDocLibrary;

        public string RootElement
        {
            set
            {
                mRootElementName = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.RootElement);
            }
            get { return mRootElementName; }
        }

        public List<string> CandidateDocLibraryNames
        {
            set
            {
                mCandidateDocLibraryNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.CandidateDocLibraryNames);
            }

            get { return mCandidateDocLibraryNames; }
        }

        public List<string> CandidateRootElementNames
        {
            set
            {
                mCandidateRootElementNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.CandidateRootElementNames);
            }

            get { return mCandidateRootElementNames; }
        }

        public List<CheckableItem> PotentialBbieItems
        {
            set
            {
                mPotentialBbieItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.PotentialBbieItems);
            }

            get { return mPotentialBbieItems; }
        }

        public List<CheckableItem> CandidateRootElementItems
        {
            set
            {
                mCandidateRootElementItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.CandidateRootElementItems);
            }

            get { return mCandidateRootElementItems; }
        }

        public ObservableCollection<CheckableTreeViewItem> CandidateAbieItems
        {
            set
            {
                mCandidateAbieItems = new List<CheckableTreeViewItem>(value);
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.CandidateAbieItems);
            }

            get { return new ObservableCollection<CheckableTreeViewItem>(mCandidateAbieItems); }
        }

        #endregion

        #region buildCheckabelTreeView

        private List<CheckableTreeViewItem> buildCheckableTreeView(CandidateRootElement ma)
        {
            var checkableTreeViewItems = new List<CheckableTreeViewItem>();
            foreach (var asma in ma.OriginalMa.Asmas)
            {
                if (asma.AssociatedBieAggregator.IsMa)
                {
                    checkableTreeViewItems.Add(new CheckableTreeViewItem(asma.AssociatedBieAggregator.Ma.Name,
                                                                         DFS(asma.AssociatedBieAggregator.Ma,
                                                                             new Collection<IMa>())));
                }
                else if (asma.AssociatedBieAggregator.IsAbie)
                {
                    rootElement.CandidateAbies.Add(new CandidateAbie(asma.AssociatedBieAggregator.Abie,
                                                                     AbieDFS(asma.AssociatedBieAggregator.Abie,
                                                                             new Collection<IAbie>())));
                    checkableTreeViewItems.Add(new CheckableTreeViewItem(true, asma.AssociatedBieAggregator.Abie.Name,
                                                                         DFS(asma.AssociatedBieAggregator.Abie,
                                                                             new Collection<IAbie>())));
                }
            }
            return checkableTreeViewItems;
        }

        private ObservableCollection<CheckableTreeViewItem> DFS(IMa root, ICollection<IMa> visited)
        {
            var tempList = new ObservableCollection<CheckableTreeViewItem>();
            IEnumerator enumerator = root.Asmas.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentAsma = (IAsma) enumerator.Current;
                if (currentAsma.AssociatedBieAggregator.IsAbie)
                {
                    var currentAbie = currentAsma.AssociatedBieAggregator.Abie;
                    if (findAbie(currentAbie.Name, rootElement.CandidateAbies) == null)
                    {
                        rootElement.CandidateAbies.Add(new CandidateAbie(currentAbie,
                                                                         AbieDFS(currentAbie, new Collection<IAbie>{currentAbie})));
                    }
                    tempList.Add(new CheckableTreeViewItem(true, currentAbie.Name,
                                                           DFS(currentAbie, new Collection<IAbie> { currentAbie })));
                }
                else if (currentAsma.AssociatedBieAggregator.IsMa)
                {
                    var currentMa = currentAsma.AssociatedBieAggregator.Ma;
                    if (!visited.Contains(currentMa))
                    {
                        visited.Add(currentMa);
                        tempList.Add(new CheckableTreeViewItem(currentMa.Name,
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
                    tempList.Add(new CheckableTreeViewItem(true, currentAbie.Name, DFS(currentAbie, visited)));
                }
            }
            return tempList;
        }

        private static List<CandidateAbie> AbieDFS(IAbie root, ICollection<IAbie> visited)
        {
            var tempList = new List<CandidateAbie>();
            IEnumerator enumerator = root.Asbies.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentAsbie = (IAsbie) enumerator.Current;
                IAbie currentAbie = currentAsbie.AssociatedAbie;
                if (!visited.Contains(currentAbie))
                {
                    visited.Add(currentAbie);
                    tempList.Add(new CandidateAbie(currentAbie, AbieDFS(currentAbie, visited)));
                }
            }
            return tempList;
        }
        private static bool isRelated(CheckableTreeViewItem CandidateParent, CheckableTreeViewItem child)
        {
            foreach (var item in CandidateParent.Children)
            {
                if(item.Text.Equals(child.Text))
                {
                    return true;
                }
                return isRelated(item, child);
            }
            return false;
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
                    rootElement = new CandidateRootElement(candidateDocLibrary.OriginalDocLibrary.DocumentRoot)
                                      {CandidateAbies = new List<CandidateAbie>()};
                    RootElement = rootElement.OriginalMa.Name;
                    mCandidateAbieItems.Clear();
                    mCandidateAbieItems = buildCheckableTreeView(rootElement);
                    CandidateAbieItems = new ObservableCollection<CheckableTreeViewItem>(mCandidateAbieItems);
                    return;
                    //override selection of root Element!
                    //CandidateRootElementNames = new List<string>(candidateDocLibrary.CandidateRootElements.ConvertAll(new Converter<CandidateRootElement, string>(CandidateRootElementToString)));                        
                }
                candidateDocLibrary.Selected = false;
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

        public void SetCheckedPotentialBbie(string selectedBbie, bool checkedValue)
        {
            foreach (CheckableItem bbieItem in mPotentialBbieItems)
            {
                if (bbieItem.Text.Equals(selectedBbie))
                {
                    bbieItem.Checked = checkedValue;
                }
            }
            PotentialBbieItems = mPotentialBbieItems;
        }

        public void SetSelectedCandidateAbie(string selectedAbie, bool checkedValue)
        {
            mPotentialBbieItems = new List<CheckableItem>();
            CandidateAbie abie = findAbie(selectedAbie, rootElement.CandidateAbies);
            if (abie != null)
            {
                var bbies = new List<IBbie>(abie.OriginalAbie.Bbies);
                mPotentialBbieItems =
                    new List<CheckableItem>(
                        bbies.ConvertAll(potBbie => new CheckableItem(true, potBbie.Name, true, false, Cursors.Arrow)));
                abie.PotentialBbies = new List<PotentialBbie>(bbies.ConvertAll(potBbie => new PotentialBbie(potBbie.Name,potBbie.Bdt.BasedOn)));
            }
            PotentialBbieItems = mPotentialBbieItems;
        }

        public void SetExpandedCheckableTreeViewItem(string selectedCheckableTreeViewItem, bool isExpanded)
        {
            CheckableTreeViewItem checkableTreeViewItem = findCheckableTreeViewItem(selectedCheckableTreeViewItem,
                                                                                    CandidateAbieItems);
            if (checkableTreeViewItem != null)
            {
                checkableTreeViewItem.IsExpanded = isExpanded;
            }
        }

        private static CandidateAbie findAbie(string selectedAbie, IEnumerable<CandidateAbie> abies)
        {
            CandidateAbie returnAbie = null;
            if (abies != null)
            {
                foreach (CandidateAbie abie in abies)
                {
                    if (abie.Name.Equals(selectedAbie))
                    {
                        return abie;
                    }
                    returnAbie = findAbie(selectedAbie, abie.PotentialAbies);
                }
            }
            return returnAbie;
        }

        public void SetCheckedCandidateAbie(string selectedAbie, bool checkedValue)
        {
            var result = findCheckableTreeViewItem(selectedAbie, CandidateAbieItems);
            if (result != null)
            {
                result.Checked = checkedValue;

                //check all related Nodes from tree
                checkAllParents(mCandidateAbieItems,result);
                //checkAllChildren(result, checkedValue); change from 08.04.2010 => don't check children
            }
            if (rootElement.CandidateAbies != null)
            {
                foreach (CandidateAbie abie in rootElement.CandidateAbies)
                {
                    if (abie.Name.Equals(selectedAbie))
                    {
                        abie.Checked = checkedValue;
                        //PotentialBbieItems = new List<CheckableItem>(abie.OriginalAbie.Bbies); TODO: add Bbies!
                    }
                }
            }
            CandidateAbieItems = new ObservableCollection<CheckableTreeViewItem>(mCandidateAbieItems);
            //PotentialBbieItems = new List<CheckableItem>(result.);
        }

        private static void checkAllParents(List<CheckableTreeViewItem> treeViewItems, CheckableTreeViewItem itemToCheck)
        {
            foreach (var item in treeViewItems)
            {
                if (isRelated(item, itemToCheck))
                {
                    item.Checked = itemToCheck.Checked;
                }
                else
                {
                    checkAllParents(new List<CheckableTreeViewItem>(item.Children),itemToCheck);
                }
            }
        }
        private static void checkAllChildren(CheckableTreeViewItem itemToCheck, bool checkedValue)
        {
            foreach (CheckableTreeViewItem child in itemToCheck.Children)
            {
                child.Checked = checkedValue;
                if (child.Children != null)
                {
                    checkAllChildren(child, checkedValue);
                }
            }
        }

        private static CheckableTreeViewItem findCheckableTreeViewItem(string selectedAbie,
                                                                       IEnumerable<CheckableTreeViewItem> listToSearch)
        {
            foreach (CheckableTreeViewItem candidateAbieItem in listToSearch)
            {
                if (candidateAbieItem.Text.Equals(selectedAbie))
                {
                    return candidateAbieItem;
                }
                if (candidateAbieItem.Children != null)
                {
                    CheckableTreeViewItem tempItem = findCheckableTreeViewItem(selectedAbie, candidateAbieItem.Children);
                    if (tempItem != null)
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

        public void createSubSet()
        {
            //var modelCreator = new ModelCreator(repository.);
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