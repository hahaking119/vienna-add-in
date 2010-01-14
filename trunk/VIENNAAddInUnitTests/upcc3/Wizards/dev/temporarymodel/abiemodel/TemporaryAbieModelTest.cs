﻿// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using CctsRepository.CcLibrary;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel;
using CctsRepository;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using System.Collections.Generic;
using CctsRepository.BieLibrary;
using System;
using VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository;
using VIENNAAddInUtils;
using Assert=NUnit.Framework.Assert;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    /// <summary>
    ///This is a test class for TemporaryAbieModelTest and is intended
    ///to contain all TemporaryAbieModelTest Unit Tests
    ///</summary>
    [TestFixture]
    public class TemporaryAbieModelTest
    {

        private ICctsRepository cctsRepository;
        private TemporaryAbieModel target;

        [SetUp]
        public void SetUp()
        {
            cctsRepository = new CCRepository(new EARepositoryAbieEditor());
            target = new TemporaryAbieModel(cctsRepository);
        }

        /// <summary>
        ///A test for CreateAbie
        ///</summary>
        [Test]
        public void ShouldCreateAbieInRepository()
        {
            target.SetSelectedCandidateCcLibrary("cclib1");
            target.SetSelectedCandidateAcc("Address");
            target.SetSelectedAndCheckedCandidateBcc("CountryName", true);
            target.SetSelectedAndCheckedPotentialBbie("CountryName", true);
            target.SetSelectedAndCheckedPotentialBdt("Text", true);
            target.AbiePrefix = "MyPrefix";
            target.AbieName = "MyAbie";
            target.SetSelectedCandidateBdtLibrary("bdtlib1");
            target.SetSelectedCandidateBieLibrary("bielib1");
            target.CreateAbie();
            IAbie testAbie = cctsRepository.GetAbieByPath((Path) "test model"/"blib1"/"bielib1"/"MyPrefixMyAbie");
            Assert.IsNotNull(testAbie);
            Assert.That(testAbie.BasedOn.Name, Is.EqualTo("Address"));
            foreach (IBbie bbie in testAbie.Bbies)
            {
                Assert.That(bbie.Name, Is.EqualTo("CountryName"));
                Assert.That(bbie.Bdt.Name, Is.EqualTo("Text"));
            }
            Assert.That(testAbie.BieLibrary.Name, Is.EqualTo("bielib1"));
            Assert.That(testAbie.BasedOn.CcLibrary.Name, Is.EqualTo("cclib1"));
        }

        #region notyetimplemented
        /// <summary>
        ///A test for TemporaryAbieModel Constructor
        ///</summary>
        [Test]
        public void ShouldLoadTemporaryAbieModelWithExistingAbieInEditMode()
        {
            IAbie abieToBeUpdated = null; // TODO: Initialize to an appropriate value
            target = new TemporaryAbieModel(cctsRepository, abieToBeUpdated);
            Assert.Fail("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for AddPotentialBbie
        ///</summary>
        [Test]
        public void ShouldAddPotentialBbieToTemporaryAbieModel()
        {
            target.AddPotentialBbie();
            Assert.Fail("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for AddPotentialBdt
        ///</summary>
        [Test]
        public void AddPotentialBdtTest()
        {
            target.AddPotentialBdt();
            Assert.Fail("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetNoSelectedCandidateAbie
        ///</summary>
        [Test]
        public void SetNoSelectedCandidateAbieTest()
        {
            ICctsRepository cctsRepository = null; // TODO: Initialize to an appropriate value
            TemporaryAbieModel target = new TemporaryAbieModel(cctsRepository); // TODO: Initialize to an appropriate value
            target.SetNoSelectedCandidateAbie();
            Assert.Fail("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetSelectedAndCheckedCandidateAbie
        ///</summary>
        [Test]
        public void SetSelectedAndCheckedCandidateAbieTest()
        {
            string selectedAbie = string.Empty; // TODO: Initialize to an appropriate value
            bool checkedValue = false; // TODO: Initialize to an appropriate value
            target.SetSelectedAndCheckedCandidateAbie(selectedAbie, checkedValue);
            Assert.Fail("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetSelectedAndCheckedCandidateBcc
        ///</summary>
        [Test]
        public void SetSelectedAndCheckedCandidateBccTest()
        {
            string selectedBcc = string.Empty; // TODO: Initialize to an appropriate value
            bool checkedValue = false; // TODO: Initialize to an appropriate value
            target.SetSelectedAndCheckedCandidateBcc(selectedBcc, checkedValue);
            Assert.Fail("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetSelectedAndCheckedPotentialAsbie
        ///</summary>
        [Test]
        public void SetSelectedAndCheckedPotentialAsbieTest()
        {
            string selectedAsbie = string.Empty; // TODO: Initialize to an appropriate value
            bool checkedValue = false; // TODO: Initialize to an appropriate value
            target.SetSelectedAndCheckedPotentialAsbie(selectedAsbie, checkedValue);
            Assert.Fail("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetSelectedAndCheckedPotentialBbie
        ///</summary>
        [Test]
        public void SetSelectedAndCheckedPotentialBbieTest()
        {
            string selectedBbie = string.Empty; // TODO: Initialize to an appropriate value
            bool checkedValue = false; // TODO: Initialize to an appropriate value
            target.SetSelectedAndCheckedPotentialBbie(selectedBbie, checkedValue);
            Assert.Fail("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetSelectedAndCheckedPotentialBdt
        ///</summary>
        [Test]
        public void SetSelectedAndCheckedPotentialBdtTest()
        {
            string selectedBdt = string.Empty; // TODO: Initialize to an appropriate value
            Nullable<bool> checkedValue = new Nullable<bool>(); // TODO: Initialize to an appropriate value
            target.SetSelectedAndCheckedPotentialBdt(selectedBdt, checkedValue);
            Assert.Fail("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateAbie
        ///</summary>
        [Test]
        public void UpdateAbieTest()
        {
            IAbie abieToBeUpdated = null; // TODO: Initialize to an appropriate value
            target.UpdateAbie(abieToBeUpdated);
            Assert.Fail("A method that does not return a value cannot be verified.");
        }



        /// <summary>
        ///A test for UpdateBbieName
        ///</summary>
        [Test]
        public void UpdateBbieNameTest()
        {
            string updatedBbieName = string.Empty; // TODO: Initialize to an appropriate value
            target.UpdateBbieName(updatedBbieName);
            Assert.Fail("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateBdtName
        ///</summary>
        [Test]
        public void UpdateBdtNameTest()
        {
            string updatedBdtName = string.Empty; // TODO: Initialize to an appropriate value
            target.UpdateBdtName(updatedBdtName);
            Assert.Fail("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for AbieName
        ///</summary>
        [Test]
        public void AbieNameTest()
        {
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.AbieName = expected;
            actual = target.AbieName;
            Assert.AreEqual(expected, actual);
            Assert.Fail("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AbiePrefix
        ///</summary>
        [Test]
        public void AbiePrefixTest()
        {
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.AbiePrefix = expected;
            actual = target.AbiePrefix;
            Assert.AreEqual(expected, actual);
            Assert.Fail("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CandidateAbieItems
        ///</summary>
        [Test]
        public void CandidateAbieItemsTest()
        {
            List<CheckableItem> expected = null; // TODO: Initialize to an appropriate value
            List<CheckableItem> actual;
            target.CandidateAbieItems = expected;
            actual = target.CandidateAbieItems;
            Assert.AreEqual(expected, actual);
            Assert.Fail("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CandidateAccNames
        ///</summary>
        [Test]
        public void CandidateAccNamesTest()
        {
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            target.CandidateAccNames = expected;
            actual = target.CandidateAccNames;
            Assert.AreEqual(expected, actual);
            Assert.Fail("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CandidateBccItems
        ///</summary>
        [Test]
        public void CandidateBccItemsTest()
        {
            List<CheckableItem> expected = null; // TODO: Initialize to an appropriate value
            List<CheckableItem> actual;
            target.CandidateBccItems = expected;
            actual = target.CandidateBccItems;
            Assert.AreEqual(expected, actual);
            Assert.Fail("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CandidateBdtLibraryNames
        ///</summary>
        [Test]
        public void CandidateBdtLibraryNamesTest()
        {
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            target.CandidateBdtLibraryNames = expected;
            actual = target.CandidateBdtLibraryNames;
            Assert.AreEqual(expected, actual);
            Assert.Fail("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CandidateBieLibraryNames
        ///</summary>
        [Test]
        public void CandidateBieLibraryNamesTest()
        {
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            target.CandidateBieLibraryNames = expected;
            actual = target.CandidateBieLibraryNames;
            Assert.AreEqual(expected, actual);
            Assert.Fail("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CandidateCcLibraryNames
        ///</summary>
        [Test]
        public void CandidateCcLibraryNamesTest()
        {
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            target.CandidateCcLibraryNames = expected;
            actual = target.CandidateCcLibraryNames;
            Assert.AreEqual(expected, actual);
            Assert.Fail("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PotentialAsbieItems
        ///</summary>
        [Test]
        public void PotentialAsbieItemsTest()
        {
            List<CheckableItem> expected = null; // TODO: Initialize to an appropriate value
            List<CheckableItem> actual;
            target.PotentialAsbieItems = expected;
            actual = target.PotentialAsbieItems;
            Assert.AreEqual(expected, actual);
            Assert.Fail("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PotentialBbieItems
        ///</summary>
        [Test]
        public void PotentialBbieItemsTest()
        {
            List<CheckableItem> expected = null; // TODO: Initialize to an appropriate value
            List<CheckableItem> actual;
            target.PotentialBbieItems = expected;
            actual = target.PotentialBbieItems;
            Assert.AreEqual(expected, actual);
            Assert.Fail("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PotentialBdtItems
        ///</summary>
        [Test]
        public void PotentialBdtItemsTest()
        {
            List<CheckableItem> expected = null; // TODO: Initialize to an appropriate value
            List<CheckableItem> actual;
            target.PotentialBdtItems = expected;
            actual = target.PotentialBdtItems;
            Assert.AreEqual(expected, actual);
            Assert.Fail("Verify the correctness of this test method.");
        }
        #endregion
    }
}