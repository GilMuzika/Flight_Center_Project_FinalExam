using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_UnitTest
{
    [TestClass]
    public class FlyingCenterSystem_Test
    {
        [TestMethod]
        public void GetProperFacade_Test()
        {
            //Arrange
            FlyingCenterSystem fsc = FlyingCenterSystem.GetInstance();
            LoggedInAdministratorFacade adminFacade_expected = new LoggedInAdministratorFacade();

            //Act
            LoggedInAdministratorFacade adminfacade_actual = fsc.GetProperFacade(typeof(Administrator)) as LoggedInAdministratorFacade;

            //Assert
            adminFacade_expected.Should().BeOfType(typeof(LoggedInAdministratorFacade));
            adminfacade_actual.Should().BeOfType(typeof(LoggedInAdministratorFacade));














        }
    }
}
