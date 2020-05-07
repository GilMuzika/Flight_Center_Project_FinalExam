using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web_Api_interface;
using Web_Api_interface.Controllers;
using Web_Api_interface.MvcControllers;

namespace Web_Api_interface.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
