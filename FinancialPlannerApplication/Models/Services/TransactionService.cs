using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinancialPlannerApplication.Models.DataAccess;

namespace FinancialPlannerApplication.Models.Services
{
    public class SharedOperationsService : ISharedOperationsService
    {
        private readonly IFinancialPlannerRepository FinancialPlannerRepository;
        public SharedOperationsService(IFinancialPlannerRepository financialPlannerRepository)
        {
            FinancialPlannerRepository = financialPlannerRepository;
        }

        public void AdjustTransactionBalances(int? accountId, Account account)
        {
            var transactions = FinancialPlannerRepository.GetTransactions().Where(m => m.AccountId == accountId).ToList();

            var transactionsOrdered =
                transactions
                    .OrderBy(m => m.PaymentDate);

            var firstTransaction = transactionsOrdered.FirstOrDefault();

            if (firstTransaction != null)
            {
                var previousTransaction = new Transaction();

                foreach (var tran in transactionsOrdered)
                {
                    if (tran.PaymentDate == firstTransaction.PaymentDate)
                    {
                        tran.Balance = tran.IsWithdrawal
                            ? account.InitialAmount - tran.Amount
                            : account.InitialAmount + tran.Amount;
                    }
                    else
                    {
                        tran.Balance = tran.IsWithdrawal
                            ? previousTransaction.Balance - tran.Amount
                            : previousTransaction.Balance + tran.Amount;
                    }

                    previousTransaction = tran;
                }

                FinancialPlannerRepository.Save();
            }
        }

        public void AdjustExpenseBalance(Expense expense)
        {
            var transactions =
               FinancialPlannerRepository.GetTransactions().Where(m => m.ExpenseId == expense.Id).ToList();

            var newWithdrawalAmount = transactions.Where(m => m.IsWithdrawal).Sum(m => m.Amount);
            var newDepositAmount = transactions.Where(m => !m.IsWithdrawal).Sum(m => m.Amount);

            expense.Balance = expense.Amount - newWithdrawalAmount + newDepositAmount;

            FinancialPlannerRepository.EditExpense(expense);
            FinancialPlannerRepository.Save();
        }

        public void AdjustExpenseBalance(Expense expense, Transaction transaction)
        {
            expense.Balance = transaction.IsWithdrawal
                       ? expense.Balance - transaction.Amount
                       : expense.Balance + transaction.Amount;

            FinancialPlannerRepository.EditExpense(expense);
            FinancialPlannerRepository.Save();
        }

        public void AdjustAccountAmount(Account account)
        {
            var transactions =
              FinancialPlannerRepository.GetTransactions().Where(m => m.AccountId == account.Id).ToList();

            transactions = transactions.ToList();
            var withdrawalAmount = transactions.Where(m => m.IsWithdrawal).Sum(m => m.Amount);
            var depositAmount = transactions.Where(m => !m.IsWithdrawal).Sum(m => m.Amount);

            account.Amount = account.InitialAmount + depositAmount - withdrawalAmount;

            FinancialPlannerRepository.EditAccount(account);
            FinancialPlannerRepository.Save();
        }

        public void EditBudgetItemBalance(BudgetItem budgetItem)
        {
            var budgetItemTransactions =
                FinancialPlannerRepository.GetTransactions().Where(m => m.BudgetItemId == budgetItem.Id).ToList();

            var budgetItemWithdrawalSum = budgetItemTransactions.Where(m => m.IsWithdrawal).Sum(m => m.Amount);
            var budgetItemDepositSum = budgetItemTransactions.Where(m => !m.IsWithdrawal).Sum(m => m.Amount);

            budgetItem.Balance = budgetItem.Amount + budgetItemDepositSum - budgetItemWithdrawalSum;

            FinancialPlannerRepository.EditBudgetItem(budgetItem);
            FinancialPlannerRepository.Save();
        }

        public void AdjustOldBudgetItemBalance(int? oldBudgetItemId)
        {
            var oldBudgetItem = FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == oldBudgetItemId);

            if (oldBudgetItem != null)
            {
                EditBudgetItemBalance(oldBudgetItem);
            }
        }

        public void AdjustNewBudgetItemBalance(int? budgetItemId)
        {
            var budgetItem = FinancialPlannerRepository.GetBudgetItems().FirstOrDefault(m => m.Id == budgetItemId);

            if (budgetItem != null)
            {
                EditBudgetItemBalance(budgetItem);
            }
        }

        public void AdjustBudgetItemBalance(BudgetItem budgetItem, bool isWithdrawal, decimal amount)
        {
            budgetItem.Balance = isWithdrawal
               ? budgetItem.Balance - amount
               : budgetItem.Balance + amount;

            if (budgetItem.Balance > budgetItem.Amount)
                budgetItem.Amount = budgetItem.Balance;

            FinancialPlannerRepository.EditBudgetItem(budgetItem);
            FinancialPlannerRepository.Save();
        }

        public void AdjustAccountBalance(Account account, bool isWithdrawal, decimal amount)
        {
            if (isWithdrawal)
                account.Amount = account.Amount - amount;
            else
                account.Amount = account.Amount + amount;

            FinancialPlannerRepository.EditAccount(account);
            FinancialPlannerRepository.Save();
        }
    }
}