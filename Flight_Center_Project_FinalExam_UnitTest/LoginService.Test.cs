using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flight_Center_Project_FinalExam_UnitTest
{
    /// <summary>
    /// Unit tests explaination in Microsoft Docs:
    ///Unit test basics - https://docs.microsoft.com/en-us/visualstudio/test/unit-test-basics?view=vs-2019
    ///Walkthrough: Test-driven development using Test Explorer - https://docs.microsoft.com/en-us/visualstudio/test/quick-start-test-driven-development-with-test-explorer?view=vs-2019
    /// </summary>

    [TestClass]
    public class LoginService_Test
    {
        [TestMethod]
        public void TryUserLogin_Customer_Test()
        {
            //Arrage
            string base64stringImage =  "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCAAQABADASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwDj/DegWmvahLHqUs0VsqqqbDjcxznmmeINFt9D1s2OnSyS27plVdslWzggHvV7wBfXET3dtLJuQqGGBymTyfp79qoeNNQa91VY4WDJaKY8svDEnOPpjArlSlzWO18nsVLqf//Z";            
            string userName = "DnblgVP";
            string passWord = "VF7jJv";
            LoginToken<Customer> loginTokenExpected = new LoginToken<Customer>();
            loginTokenExpected.ActualUser = new Customer("S2r80C0X", "RzjDZy", "Ls0RDh", "K57xv2", "ZQna3uR", base64stringImage, 98);
            loginTokenExpected.ActualUser.ID = 10108;            
            loginTokenExpected.UserAsUser = new Utility_class_User(userName, passWord, "Customer", -9999, 0, -9999);
            loginTokenExpected.UserAsUser.ID = 98;


            //Act
            LoginService<Customer> loginService = new LoginService<Customer>();
            loginService.TryUserLogin(userName, passWord, out LoginToken<Customer> loginTokenActual);

            
            Assert.AreEqual(loginTokenExpected.ActualUser, loginTokenActual.ActualUser);
            //Assert.AreEqual(airline1 == airline2, true);

        }


    }
}
