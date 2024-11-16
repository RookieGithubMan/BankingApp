using Banking.UI.EntityFramework.Dao;
using BankingApplication.UI.Controllers;
using BankingApplication.UI.EntityFramework.DBContext;
using BankingApplication.UI.EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using static System.TimeZoneInfo;

namespace Banking.UI.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> logger;
        private readonly ITransactionDao transactionDao;

        public TransactionController(ILogger<TransactionController> logger, BankingDBContext dbContext, ITransactionDao transactionDao)
        {
            this.logger = logger;
            this.transactionDao = transactionDao;
        }

        /// <summary>
        /// Show all transactions in Transaction View
        /// </summary>
        [HttpGet]
        public IActionResult GetTransactions()
        {
            var transactions = new List<TransactionDTO>();

            var transactionsFromDB = transactionDao.GetTransactions();

            foreach (var transactionFromDB in transactionsFromDB)
            {
                var transaction = DTOMapper.MapTransactionDTO(transactionFromDB);
                transactions.Add(transaction);
            }

            return Ok(transactions);
        }
    }

    #region DTO
    public class GetTransactionsResponseDTO
    {
        public List<TransactionDTO> Transactions { get; set; }
    }

    public class TransactionDTO
    {
        public long TransactionID { get; set; }
        public long AccountId_From { get; set; }
        public decimal AccountBalance_From { get; set; }
        public long AccountId_To { get; set; }
        public decimal AccountBalance_To { get; set; }
        public DateTime TransactionTime { get; set; }
        public decimal Amount { get; set; }
    }
    #endregion

    #region Util
    public static class DTOMapper
    {
        public static TransactionDTO MapTransactionDTO(Transaction transaction)
        {
            var transactionDTO = new TransactionDTO
            {
                TransactionID = transaction.TransactionID,
                AccountId_From = transaction.AccountId_From,
                AccountBalance_From = transaction.AccountBalance_From,
                AccountId_To = transaction.AccountId_To,
                AccountBalance_To = transaction.AccountBalance_To,
                TransactionTime = transaction.TransactionTime,
                Amount = transaction.Amount
            };

            return transactionDTO;
        }

        public static Transaction ReverseMapTransactionDTO(TransactionDTO transactionDTO)
        {
            var transaction = new Transaction
            {
                TransactionID = transactionDTO.TransactionID,
                AccountId_From = transactionDTO.AccountId_From,
                AccountBalance_From = transactionDTO.AccountBalance_From,
                AccountId_To = transactionDTO.AccountId_To,
                AccountBalance_To = transactionDTO.AccountBalance_To,
                TransactionTime = transactionDTO.TransactionTime,
                Amount = transactionDTO.Amount
            };

            return transaction;
        }
    }
    #endregion
}
