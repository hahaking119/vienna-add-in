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
        private int idCounter;
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
                    var currentItem = new CheckableTreeViewItem(asma.AssociatedBieAggregator.Ma.Name, idCounter++)
                                          {Parent = null};
                    currentItem.Children = DFS(currentItem, asma.AssociatedBieAggregator.Ma, new Collection<IMa>());

                    checkableTreeViewItems.Add(currentItem);
                }
                else if (asma.AssociatedBieAggregator.IsAbie)
                {
                    rootElement.CandidateAbies.Add(new CandidateAbie(asma.AssociatedBieAggregator.Abie,
                                                                     AbieDFS(asma.AssociatedBieAggregator.Abie,
                                                                             new Collection<IAbie>())));
                    var currentItem = new CheckableTreeViewItem(true, asma.AssociatedBieAggregator.Abie.Name,
                                                                idCounter++)
                                          {Parent = null};
                    currentItem.Children = DFS(currentItem, asma.AssociatedBieAggregator.Abie,
                                               new Collection<IAbie>());

                    checkableTreeViewItems.Add(currentItem);
                }
            }
            //also visualize Root Element in Tree!
            var root = new CheckableTreeViewItem(ma.OriginalMa.Name, idCounter++)
                           {
                               Children = new ObservableCollection<CheckableTreeViewItem>(checkableTreeViewItems)
                           };
            return new List<CheckableTreeViewItem> {root};
        }

        private ObservableCollection<CheckableTreeViewItem> DFS(CheckableTreeViewItem parent, IMa root,
                                                                ICollection<IMa> visited)
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
                                                                         AbieDFS(currentAbie,
                                                                                 new Collection<IAbie> {currentAbie})));
                    }
                    var currentItem = new CheckableTreeViewItem(true, currentAbie.Name, idCounter++);
                    currentItem.Children = DFS(currentItem, currentAbie, new Collection<IAbie> {currentAbie});
                    currentItem.Parent = parent;
                    tempList.Add(currentItem);
                }
                else if (currentAsma.AssociatedBieAggregator.IsMa)
                {
                    var currentMa = currentAsma.AssociatedBieAggregator.Ma;
                    if (!visited.Contains(currentMa))
                    {
                        visited.Add(currentMa);
                        var currentItem = new CheckableTreeViewItem(currentMa.Name, idCounter++);
                        currentItem.Children = DFS(currentItem, currentMa, visited);
                        currentItem.Parent = parent;
                        tempList.Add(currentItem);
                    }
                }
            }
            return tempList;
        }

        private ObservableCollection<CheckableTreeViewItem> DFS(CheckableTreeViewItem parent, IAbie root,
                                                                ICollection<IAbie> visited)
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
                    var currentItem = new CheckableTreeViewItem(true, currentAbie.Name, idCounter++);
                    currentItem.Children = DFS(currentItem, currentAbie, visited);
                    currentItem.Parent = parent;
                    tempList.Add(currentItem);
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
                var currentAbie = currentAsbie.AssociatedAbie;
                if (!visited.Contains(currentAbie))
                {
                    visited.Add(currentAbie);
                    tempList.Add(new CandidateAbie(currentAbie, AbieDFS(currentAbie, visited)));
                }
            }
            return tempList;
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
            foreach (var candidateDocLibrary in mCandidateDocLibraries)
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
            foreach (var candidateDocLibrary in mCandidateDocLibraries)
            {
                if (candidateDocLibrary.Selected)
                {
                    foreach (var candidateRootElement in candidateDocLibrary.CandidateRootElements)
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

        public void SetCheckedPotentialBbie(CheckableItem checkableItem)
        {
            var abie = FindSelectedAbie(rootElement.CandidateAbies);

            foreach (var potentialBbie in abie.PotentialBbies)
            {
                if (potentialBbie.Name.Equals(checkableItem.Text))
                {
                    potentialBbie.Checked = checkableItem.Checked;
                }
            }
            PotentialBbieItems = mPotentialBbieItems;
        }

        public void SetSelectedCandidateAbie(CheckableTreeViewItem checkableTreeViewItem)
        {
            mPotentialBbieItems = new List<CheckableItem>();
            var abie = findAbie(checkableTreeViewItem.Text, rootElement.CandidateAbies);
            if (abie != null)
            {
                if (abie.PotentialBbies == null)
                {
                    var bbies = new List<IBbie>(abie.OriginalAbie.Bbies);
                    abie.PotentialBbies =
                        new List<PotentialBbie>(
                            bbies.ConvertAll(potBbie => new PotentialBbie(potBbie.Name)));
                }
                mPotentialBbieItems =
                    new List<CheckableItem>(
                        abie.PotentialBbies.ConvertAll(
                            potBbie => new CheckableItem(potBbie.Checked, potBbie.Name, true, false, Cursors.Arrow)));
                abie.Selected = true;
                UnSelectAllButAbie(abie.Name, rootElement.CandidateAbies);
            }
            PotentialBbieItems = mPotentialBbieItems;
        }

        private static CandidateAbie FindSelectedAbie(IEnumerable<CandidateAbie> abies)
        {
            CandidateAbie returnAbie = null;
            if (abies != null)
            {
                foreach (var abie in abies)
                {
                    if (abie.Selected)
                    {
                        return abie;
                    }
                    returnAbie = FindSelectedAbie(abie.PotentialAbies);
                }
            }
            return returnAbie;
        }

        private static void UnSelectAllButAbie(string selectedAbie, IEnumerable<CandidateAbie> abies)
        {
            if (abies != null)
            {
                foreach (var abie in abies)
                {
                    if (!abie.Name.Equals(selectedAbie))
                    {
                        abie.Selected = false;
                    }
                    UnSelectAllButAbie(selectedAbie, abie.PotentialAbies);
                }
            }
        }

        public void SetExpandedCheckableTreeViewItem(string selectedCheckableTreeViewItem, bool isExpanded)
        {
            var checkableTreeViewItem = findCheckableTreeViewItem(selectedCheckableTreeViewItem,
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
                foreach (var abie in abies)
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

        public void SetCheckedCandidateAbie(CheckableTreeViewItem checkableTreeViewItem)
        {
            if (checkableTreeViewItem != null)
            {
                //check all related Nodes from tree if checked is true
                if (checkableTreeViewItem.Checked)
                {
                    checkAllParents(checkableTreeViewItem);
                }
                else
                {
                    checkAllChildren(checkableTreeViewItem);
                }
                //checkAllChildren(result, checkedValue); change from 08.04.2010 => don't check children
                var abie = findAbie(checkableTreeViewItem.Text, rootElement.CandidateAbies);

                if(abie.PotentialBbies==null)
                {
                    var bbies = new List<IBbie>(abie.OriginalAbie.Bbies);
                    abie.PotentialBbies = new List<PotentialBbie>(bbies.ConvertAll(potBbie => new PotentialBbie(potBbie.Name)));
                }

                foreach (var bbie in abie.PotentialBbies)
                {
                    bbie.Checked = checkableTreeViewItem.Checked;
                }

                abie.Checked = checkableTreeViewItem.Checked;
                CandidateAbieItems = new ObservableCollection<CheckableTreeViewItem>(mCandidateAbieItems);
            }
            //PotentialBbieItems = new List<CheckableItem>(result.);
        }

        private void checkAllParents(CheckableTreeViewItem itemToCheck)
        {
            if (itemToCheck.Parent != null)
            {
                if (itemToCheck.Checked)
                {
                    var abie = findAbie(itemToCheck.Parent.Text, rootElement.CandidateAbies);
                    if (abie != null)
                    {
                        abie.Checked = itemToCheck.Checked;
                        itemToCheck.Parent.Checked = itemToCheck.Checked;
                    }
                }
                checkAllParents(itemToCheck.Parent);
            }
        }

        private void checkAllChildren(CheckableTreeViewItem itemToCheck)
        {
            foreach (var child in itemToCheck.Children)
            {
                var abie = findAbie(child.Text, rootElement.CandidateAbies);
                abie.Checked = itemToCheck.Checked;
                child.Checked = itemToCheck.Checked;
                if (child.Children != null)
                {
                    checkAllChildren(child);
                }
            }
        }

        private static CheckableTreeViewItem findCheckableTreeViewItem(string selectedAbie,
                                                                       IEnumerable<CheckableTreeViewItem> listToSearch)
        {
            foreach (var candidateAbieItem in listToSearch)
            {
                if (candidateAbieItem.Text.Equals(selectedAbie))
                {
                    return candidateAbieItem;
                }
                if (candidateAbieItem.Children != null)
                {
                    var tempItem = findCheckableTreeViewItem(selectedAbie, candidateAbieItem.Children);
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
            followAsmasAndRemoveUnused(rootElement.OriginalMa);
            foreach (var bieLibrary in ccCache.GetBieLibraries())
            {
                var abies = new List<IAbie>(bieLibrary.Abies);
                foreach (var abie in abies)
                {
                    var result = findCheckableTreeViewItem(abie.Name, mCandidateAbieItems);
                    if (result == null)
                    {
                        bieLibrary.RemoveAbie(abie);
                    }
                    else
                    {
                        if (!result.Checked)
                        {
                            bieLibrary.RemoveAbie(abie);
                        }
                        else
                        {
                            var candidateAbie = findAbie(abie.Name, rootElement.CandidateAbies);
                            var actualBbies = new List<IBbie>(abie.Bbies);

                            if (candidateAbie.PotentialBbies != null)
                            {
                                foreach (var bbie in actualBbies)
                                {
                                    foreach (var potentialBbie in candidateAbie.PotentialBbies)
                                    {
                                        if (bbie.Name.Equals(potentialBbie.Name))
                                        {
                                            if (!potentialBbie.Checked)
                                            {
                                                abie.RemoveBbie(bbie);
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

        private void followAsmasAndRemoveUnused(IMa rootMa)
        {
            foreach (IAsma asma in rootMa.Asmas)
            {
                if (asma.AssociatedBieAggregator.IsMa)
                {
                    followAsmasAndRemoveUnused(asma.AssociatedBieAggregator.Ma);
                }
                if (asma.AssociatedBieAggregator.IsAbie)
                {
                    var result = findCheckableTreeViewItem(asma.AssociatedBieAggregator.Abie.Name,
                                                                             mCandidateAbieItems);
                    if (result == null)
                    {
                        rootMa.RemoveAsma(asma);
                    }
                    else
                    {
                        if (!result.Checked)
                        {
                            rootMa.RemoveAsma(asma);
                        }
                    }
                }
            }
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