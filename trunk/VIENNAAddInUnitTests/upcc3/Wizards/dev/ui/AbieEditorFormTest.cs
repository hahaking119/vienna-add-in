// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Threading;
using System.Windows;
using CctsRepository;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.Wizards.dev.ui;
using VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.ui
{
    [TestFixture]
    public class AbieEditorFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateAbieEditorForm()
        {
            var t = new Thread(() => new Application().Run(new AbieEditor(CctsRepositoryFactory.CreateCctsRepository(new EARepositoryAbieEditor()))));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }

        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateAbieEditorFormForParticularAbie()
        {
            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(new EARepositoryAbieEditor());

            //var t = new Thread(() => new Application().Run(new AbieEditor(cctsRepository, cctsRepository.GetAbieByPath(EARepositoryAbieEditor.PathToBIEPerson()))));
            var t = new Thread(() => new Application().Run(new AbieEditor(cctsRepository, null)));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}