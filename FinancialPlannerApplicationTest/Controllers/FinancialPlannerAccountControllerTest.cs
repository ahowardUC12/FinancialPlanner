using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FinancialPlannerApplication.Controllers;
using FinancialPlannerApplication.Models.DataAccess;
using FinancialPlannerApplication.Models.Services;
using FinancialPlannerApplication.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPlannerApplicationTest.Controllers
{
    [TestClass]
    public class FinancialPlannerAccountControllerTest
    {
        private Mock<IFinancialPlannerRepository> FinancialPlannerAccountRepository;
        private Mock<IFinancialPlannerAccountService> FinancialPlannerAccountService;
        private FinancialPlannerAccountController Controller;
        private Mock<ControllerContext> ControllerContext;
        private Mock<IPrincipal> Principal;
        private Mock<IIdentity> Identity;

        [TestInitialize]
        public void Setup()
        {
            FinancialPlannerAccountService= new Mock<IFinancialPlannerAccountService>();
            FinancialPlannerAccountRepository = new Mock<IFinancialPlannerRepository>();
            ControllerContext = new Mock<ControllerContext>();
            Principal = new Mock<IPrincipal>();
            Identity = new Mock<IIdentity>();
            Controller = new FinancialPlannerAccountController(FinancialPlannerAccountRepository.Object,
                FinancialPlannerAccountService.Object);
            ControllerContext.Setup(c => c.HttpContext.User).Returns(Principal.Object);
            Principal.Setup(p => p.Identity).Returns(Identity.Object);
            Controller.ControllerContext = ControllerContext.Object;
        }

        [TestClass]
        public class IndexMethod : FinancialPlannerAccountControllerTest
        {
            [TestMethod]
            public void ReturnsView_AccountIndexViewModel()
            {
                FinancialPlannerAccountService.Setup(m => m.MapAccountIndexViewModel(It.IsAny<string>()))
                    .Returns(new AccountIndexViewModel());

                var result = Controller.Index();

                Assert.IsInstanceOfType(result, typeof(ViewResult));
                Assert.IsInstanceOfType(((ViewResult)result).Model, typeof(AccountIndexViewModel));
            }
        }

        [TestClass]
        public class AddAccountMethod : FinancialPlannerAccountControllerTest
        {
            [TestMethod]
            public void Returns()
            {
                FinancialPlannerAccountService.Setup(m => m.MapAccountIndexViewModel(It.IsAny<string>()))
                    .Returns(new AccountIndexViewModel());

                var result = Controller.Index();

                Assert.IsInstanceOfType(result, typeof(ViewResult));
                Assert.IsInstanceOfType(((ViewResult)result).Model, typeof(AccountIndexViewModel));
            }
        }
    }
}
