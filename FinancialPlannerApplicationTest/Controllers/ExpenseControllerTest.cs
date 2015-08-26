using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using FinancialPlannerApplication.Controllers;
using FinancialPlannerApplication.Models;
using FinancialPlannerApplication.Models.DataAccess;
using FinancialPlannerApplication.Models.Services;
using FinancialPlannerApplication.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPlannerApplicationTest.Controllers
{
    [TestClass]
    public class ExpenseControllerTest
    {
        private Mock<IFinancialPlannerRepository> FinancialPlannerRepository;
        private Mock<IExpenseService> ExpenseService;
        private ExpenseController Controller;
        private Mock<ControllerContext> ControllerContext;
        private Mock<IPrincipal> Principal;
        private Mock<IIdentity> Identity;

        [TestInitialize]
        public void Setup()
        {
            FinancialPlannerRepository = new Mock<IFinancialPlannerRepository>();
            ExpenseService = new Mock<IExpenseService>();

            ControllerContext = new Mock<ControllerContext>();
            Principal = new Mock<IPrincipal>();
            Identity = new Mock<IIdentity>();

            Controller = new ExpenseController(FinancialPlannerRepository.Object, ExpenseService.Object);

            ControllerContext.Setup(c => c.HttpContext.User).Returns(Principal.Object);
            Principal.Setup(p => p.Identity).Returns(Identity.Object);
            Controller.ControllerContext = ControllerContext.Object;
        }

        [TestClass]
        public class IndexMethod : ExpenseControllerTest
        {
            [TestMethod]
            public void ReturnsView_ExpenseIndexViewModel()
            {
                ExpenseService.Setup(m => m.MapExpenseIndexViewModel(It.IsAny<string>()))
                    .Returns(new ExpenseIndexViewModel());

                var result = Controller.Index();

                Assert.IsInstanceOfType(result, typeof(ViewResult));
                Assert.IsInstanceOfType(((ViewResult)result).Model, typeof(ExpenseIndexViewModel));
            }
        }

        [TestClass]
        public class LoadExpenseTransactionsMethod : ExpenseControllerTest
        {
            [TestMethod]
            public void ReturnsPartialView_ExpenseTransactionsViewModel()
            {
                ExpenseService.Setup(m => m.MapExpenseTransactionsViewModel(It.IsAny<int>()))
                    .Returns(new ExpenseTransactionsViewModel());

                var result = Controller.LoadExpenseTransactions(1);

                Assert.IsInstanceOfType(result, typeof(PartialViewResult));
                Assert.IsInstanceOfType(((PartialViewResult)result).Model, typeof(ExpenseTransactionsViewModel));
            }
        }

        [TestClass]
        public class AddExpenseMethod : ExpenseControllerTest
        {
            [TestMethod]
            public void ReturnsPartialView()
            {
                var result = Controller.AddExpense();

                Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            }
        }

        [TestClass]
        public class AddExpensePostMethod : ExpenseControllerTest
        {
            [TestMethod]
            public void IdIsZero_ValidModel_AddsExpense_ReturnsView()
            {
                var result = Controller.AddExpense(new EditExpenseViewModel());

                Assert.IsInstanceOfType(result, typeof(PartialViewResult));
                ExpenseService.Verify(m => m.AddExpense(It.IsAny<EditExpenseViewModel>(), It.IsAny<string>()), Times.Once);
            }

            [TestMethod]
            public void IdIsZero_InvalidModel_ReturnsView()
            {
                Controller.ModelState.AddModelError("Error", "Error");
                var result = Controller.AddExpense(new EditExpenseViewModel());

                Assert.IsInstanceOfType(result, typeof(ViewResult));
                ExpenseService.Verify(m => m.AddExpense(It.IsAny<EditExpenseViewModel>(), It.IsAny<string>()), Times.Never);
            }

            [TestMethod]
            public void IdNotZero_ReturnsEditExpense()
            {
                var result = Controller.AddExpense(new EditExpenseViewModel{ExpenseId = 1});

                Assert.IsInstanceOfType(result, typeof(ActionResult));
                ExpenseService.Verify(m => m.AddExpense(It.IsAny<EditExpenseViewModel>(), It.IsAny<string>()), Times.Never);
            }
        }

        [TestClass]
        public class EditExpenseMethod : ExpenseControllerTest
        {
            [TestMethod]
            public void NullViewModel_ErrorView()
            {
                var result = Controller.EditExpense(1);

                Assert.IsInstanceOfType(result, typeof(ViewResult));
            }

            [TestMethod]
            public void ReturnsView_EditExpenseViewModel()
            {
                ExpenseService.Setup(m => m.MapEditExpenseViewModel(It.IsAny<int>()))
                    .Returns(new EditExpenseViewModel());

                var result = Controller.EditExpense(1);

                Assert.IsInstanceOfType(result, typeof(PartialViewResult));
                Assert.IsInstanceOfType(((PartialViewResult)result).Model, typeof(EditExpenseViewModel));
            }
        }

        [TestClass]
        public class EditExpensePostMethod : ExpenseControllerTest
        {
            [TestMethod]
            public void InvalidModel_ReturnsView_EditExpenseViewModel()
            {
                Controller.ModelState.AddModelError("Error", "Error");
                var result = Controller.EditExpense(new EditExpenseViewModel());

                Assert.IsInstanceOfType(result, typeof(ViewResult));
            }

            [TestMethod]
            public void NotSaved_ReturnsErrorView()
            {
                ExpenseService.Setup(m => m.EditExpense(new EditExpenseViewModel())).Returns(false);

                var result = Controller.EditExpense(new EditExpenseViewModel());

                Assert.IsInstanceOfType(result, typeof(ViewResult));
                Assert.AreEqual("Error", ((ViewResult)result).ViewName);
            }

            [TestMethod]
            public void Saved_ReturnsRedirect()
            {
                ExpenseService.Setup(m => m.EditExpense(It.IsAny<EditExpenseViewModel>())).Returns(true);

                var result = Controller.EditExpense(new EditExpenseViewModel());

                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            }
        }

        [TestClass]
        public class EditTransactionMethod : ExpenseControllerTest
        {
            [TestMethod]
            public void NullViewModel_ErrorView()
            {
                var result = Controller.EditTransaction(1, 1);

                Assert.IsInstanceOfType(result, typeof(ViewResult));
            }

            [TestMethod]
            public void ReturnsView_EditExpenseTransactionViewModel()
            {
                ExpenseService.Setup(m => m.MapEditTransactionViewModelForEdit(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(new EditExpenseTransactionViewModel());

                var result = Controller.EditTransaction(1, 1);

                Assert.IsInstanceOfType(result, typeof(PartialViewResult));
                Assert.IsInstanceOfType(((PartialViewResult)result).Model, typeof(EditExpenseTransactionViewModel));
            }
        }

        [TestClass]
        public class AddTransactionMethod : ExpenseControllerTest
        {
            [TestMethod]
            public void NullViewModel_ErrorView()
            {
                var result = Controller.AddTransaction(1);

                Assert.IsInstanceOfType(result, typeof(ViewResult));
            }

            [TestMethod]
            public void ReturnsView_EditExpenseTransactionViewModel()
            {
                ExpenseService.Setup(m => m.MapEditTransactionViewModelForAdd(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(new EditExpenseTransactionViewModel());

                var result = Controller.AddTransaction(1);

                Assert.IsInstanceOfType(result, typeof(PartialViewResult));
                Assert.IsInstanceOfType(((PartialViewResult)result).Model, typeof(EditExpenseTransactionViewModel));
            }
        }

        [TestClass]
        public class AddTransactionPostMethod : ExpenseControllerTest
        {
            [TestMethod]
            public void FindingBudgetItemsTrue_ReturnsActionResult()
            {
                var result = Controller.AddTransaction(new EditExpenseTransactionViewModel{FindingBudgetItems = true}, false);

                Assert.IsInstanceOfType(result, typeof(ActionResult));
            }

            [TestMethod]
            public void InvalidModel_ReturnsPartialView_EditExpenseTransactionViewModel()
            {
                ExpenseService.Setup(m => m.MapEditTransactionViewModelForAdd(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(new EditExpenseTransactionViewModel());

                var result = Controller.AddTransaction(new EditExpenseTransactionViewModel{SelectedExpenseId = -1}, false);

                Assert.IsInstanceOfType(result, typeof(PartialViewResult));
                Assert.IsInstanceOfType(((PartialViewResult)result).Model, typeof(EditExpenseTransactionViewModel));
            }

            [TestMethod]
            public void ValidModel_IdIsZero_AddsExpense_ReturnsPartialView_EditExpenseTransactionViewModel()
            {
                FinancialPlannerRepository.Setup(m => m.GetExpenses()).Returns(new List<Expense>
                {
                    new Expense()
                });
                ExpenseService.Setup(
                    m => m.AddTransaction(It.IsAny<Expense>(), It.IsAny<EditExpenseTransactionViewModel>()))
                    .Returns(new Transaction());
                    
                var result = Controller.AddTransaction(new EditExpenseTransactionViewModel { SelectedExpenseId = 1, ExpenseTransactionId = 0}, false);

                Assert.IsInstanceOfType(result, typeof(PartialViewResult));
                Assert.IsInstanceOfType(((PartialViewResult)result).Model, typeof(EditExpenseTransactionViewModel));
                ExpenseService.Verify(m => m.AddTransaction(It.IsAny<Expense>(), It.IsAny<EditExpenseTransactionViewModel>()), Times.Once);
            }
        }

        [TestClass]
        public class ExpenseTransactionPostMethod : ExpenseControllerTest
        {
            [TestMethod]
            public void FindingBudgetItemsTrue_ReturnsActionResult()
            {
                var result = Controller.EditTransaction(new EditExpenseTransactionViewModel { FindingBudgetItems = true }, false);

                Assert.IsInstanceOfType(result, typeof(ActionResult));
            }

            [TestMethod]
            public void InvalidModel_ReturnsPartialView_EditExpenseTransactionViewModel()
            {
                ExpenseService.Setup(m => m.MapEditTransactionViewModelForAdd(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(new EditExpenseTransactionViewModel());

                var result = Controller.EditTransaction(new EditExpenseTransactionViewModel { SelectedExpenseId = -1 }, false);

                Assert.IsInstanceOfType(result, typeof(PartialViewResult));
                Assert.IsInstanceOfType(((PartialViewResult)result).Model, typeof(EditExpenseTransactionViewModel));
            }

            [TestMethod]
            public void ValidModel_IdIsZero_AddsExpense_ReturnsPartialView_EditExpenseTransactionViewModel()
            {
                FinancialPlannerRepository.Setup(m => m.GetExpenses()).Returns(new List<Expense>{new Expense{Id = 1}});
                FinancialPlannerRepository.Setup(m => m.GetTransactions()).Returns(new List<Transaction>{new Transaction{Id = 1}                });
                ExpenseService.Setup(
                    m => m.AddTransaction(It.IsAny<Expense>(), It.IsAny<EditExpenseTransactionViewModel>()))
                    .Returns(new Transaction());

                var result = Controller.EditTransaction(new EditExpenseTransactionViewModel { SelectedExpenseId = 1, ExpenseTransactionId = 1 }, false);

                Assert.IsInstanceOfType(result, typeof(PartialViewResult));
                Assert.IsInstanceOfType(((PartialViewResult)result).Model, typeof(EditExpenseTransactionViewModel));
                ExpenseService.Verify(m => m.EditTransaction(It.IsAny<EditExpenseTransactionViewModel>()), Times.Once);
            }
        }
    }
}
