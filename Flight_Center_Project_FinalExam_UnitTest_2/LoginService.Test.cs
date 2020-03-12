using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_UnitTest_2
{
    [TestClass]
    class LoginService
    {
        [TestMethod]
        public void TryUserLogin_Customer_Test()
        {
            //Arrage
            string userName = "DnblgVP";
            string passWord = "VF7jJv";
            LoginToken<Customer> loginTokenExpected = new LoginToken<Customer>();
            loginTokenExpected.ActualUser = new Customer("S2r80C0X", "RzjDZy", "Ls0RDh", "K57xv2", "ZQna3uR", 98);
            loginTokenExpected.ActualUser.ID = 10108;
            loginTokenExpected.UserAsUser = new Utility_class_User("DnblgVP", "VF7jJv", "Customer", -9999, 0, -9999);
            loginTokenExpected.UserAsUser.ID = 98;


            //Act
            LoginService<Customer> loginService = new LoginService<Customer>();
            loginService.TryUserLogin(userName, passWord, out LoginToken<Customer> loginTokenActual);


            Assert.AreEqual(loginTokenExpected.ActualUser, loginTokenActual.ActualUser);

        }
    }
}
