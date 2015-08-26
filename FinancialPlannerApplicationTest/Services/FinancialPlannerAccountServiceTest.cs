using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialPlannerApplication.Models.DataAccess;
using FinancialPlannerApplication.Models.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPlannerApplicationTest.Services
{
    [TestClass]
    public class FinancialPlannerAccountServiceTest
    {
        private Mock<IFinancialPlannerRepository> FinancialPlannerRepository;
        private Mock<ISetViewModelsService> SetViewModelsService;
    }
}
