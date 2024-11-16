using Banking.UI.EntityFramework.Dao;
using BankingApplication.UI.Controllers;
using BankingApplication.UI.EntityFramework.DBContext;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Banking.UI.Controllers
{
    public class InitiateTransactionController : Controller
    {
        private readonly ILogger<InitiateTransactionController> logger;
        private readonly IInitiateTransactionDao initiateTransactionDao;

        public InitiateTransactionController(ILogger<InitiateTransactionController> logger, BankingDBContext dbContext, IInitiateTransactionDao initiateTransactionDao)
        {
            this.logger = logger;
            this.initiateTransactionDao = initiateTransactionDao;
        }

        /// <summary>
        /// Show all transactions in Transaction View
        /// </summary>
        [HttpPost]
        public IActionResult InitiateTransaction()
        {
            InitiateTransactionDTO requestBody;

            using (var reader = new StreamReader(Request.Body))
            {
                var content = reader.ReadToEnd();
                requestBody = JsonSerializer.Deserialize<InitiateTransactionDTO>(content);
            }

            if (requestBody == null)
            {
                throw new Exception("Invalid body");
            }

            initiateTransactionDao.InitiateTransaction(requestBody.AccountId_From, requestBody.AccountId_To, requestBody.Amount);

            return NoContent();
        }
    }

    #region DTO
    public class InitiateTransactionDTO
    {
        public long AccountId_From { get; set; }
        public long AccountId_To { get; set; }
        public decimal Amount { get; set; }
    }
    #endregion
}
