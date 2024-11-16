using Banking.UI.Controllers;
using BankingApplication.UI.EntityFramework.DBContext;
using BankingApplication.UI.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Banking.UI.EntityFramework.Dao
{
    public interface IInitiateTransactionDao
    {
        public void InitiateTransaction(long accountIdFrom, long accountIdTo, decimal amount);
    }

    public class InitiateTransactionDao : IInitiateTransactionDao
    {
        private readonly IAccountDao accountDao;
        private readonly ITransactionDao transactionDao;
        private readonly BankingDBContext bankingDBContext;

        public InitiateTransactionDao(IAccountDao accountDao, BankingDBContext bankingDBContext, ITransactionDao transactionDao)
        {
            this.accountDao = accountDao;
            this.bankingDBContext = bankingDBContext;
            this.transactionDao = transactionDao;
        }

        public void InitiateTransaction(long accountIdFrom, long accountIdTo, decimal amount)
        {
            using (var dbTransaction = bankingDBContext.Database.BeginTransaction())
            {
                try
                {
                    var accountFrom = accountDao.GetAccountByAccountId(accountIdFrom);
                    accountFrom.AccountBalance = accountFrom.AccountBalance - amount;
                    accountDao.UpdateAccount(accountFrom);

                    var accountTo = accountDao.GetAccountByAccountId(accountIdTo);
                    accountTo.AccountBalance = accountTo.AccountBalance + amount;
                    accountDao.UpdateAccount(accountTo);

                    var transaction = new Transaction
                    {
                        AccountBalance_From = accountFrom.AccountBalance,
                        AccountBalance_To = accountTo.AccountBalance,
                        AccountId_From = accountIdFrom,
                        AccountId_To = accountIdTo,
                        Amount = amount,
                        TransactionID = GenerateTransactionId(),
                        TransactionTime = DateTime.UtcNow
                    };
                    transactionDao.CreateTransaction(transaction);

                    dbTransaction.Commit();
                }
                catch(Exception ex)
                {
                    dbTransaction.Rollback();
                }
            }
        }

        private long GenerateTransactionId()
        {

            long minValue = 1000000000L;
            long maxValue = 9999999999L;

            long range = maxValue - minValue;

            var randomtransactionId = (long)(new Random().NextDouble() * range) + minValue;
            return randomtransactionId;
        }
    }
}
