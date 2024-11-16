using Banking.UI.EntityFramework.Dao;
using BankingApplication.UI.EntityFramework.DBContext;
using BankingApplication.UI.EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Text.Json;

namespace BankingApplication.UI.Controllers
{
    [ApiController]
    [Route("accounts-api/accounts")]
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> logger;
        private readonly IAccountDao accountDao;

        public AccountsController(ILogger<AccountsController> logger, BankingDBContext dbContext, IAccountDao accountDao)
        {
            this.logger = logger;
            this.accountDao = accountDao;
        }

        [HttpGet]
        public IActionResult GetAccounts()
        {
            var accounts = new List<AccountDTO>();
            var accountsFromDB = accountDao.GetAccounts();

            //Alternate - accounts = accountsFromDB.Select(x => DTOMapper.MapAccountDTO(x)).ToList();
            foreach (var accountfromDB in accountsFromDB)
            {
                var account =  DTOMapper.MapAccountDTO(accountfromDB);
                accounts.Add(account);
            }
            return Ok(accounts);
        }

        [HttpPost]
        public IActionResult CreateAccount()
        {
            CreateAccountRequestDTO requestBody;

            using (var reader = new StreamReader(Request.Body))
            {
                var content = reader.ReadToEnd();
                requestBody = JsonSerializer.Deserialize<CreateAccountRequestDTO>(content);
            }

            if(requestBody == null)
            {
                throw new Exception("Invalid body");
            }

            var account = DTOMapper.ReverseMapAccountDTO(requestBody.Account);
            accountDao.CreateAccount(account);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateAccount()
        {
            UpdateAccountRequestDTO requestBody;

            using (var reader = new StreamReader(Request.Body))
            {
                var content = reader.ReadToEnd();
                requestBody = JsonSerializer.Deserialize<UpdateAccountRequestDTO>(content);
            }

            if (requestBody == null)
            {
                throw new Exception("Invalid body");
            }

            var account = DTOMapper.ReverseMapAccountDTO(requestBody.Account);
            accountDao.CreateAccount(account);

            return NoContent();
        }
    }

    #region DTO
    public class GetAccountsResponseDTO
    {
        public List<AccountDTO> Accounts { get; set; }
    }

    public class CreateAccountRequestDTO
    {
        public AccountDTO Account { get; set; }
    }

    public class UpdateAccountRequestDTO
    {
        public AccountDTO Account { get; set; }
    }

    public class AccountDTO
    {
        public long AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal AccountBalance { get; set; }
    }
    #endregion

    #region Util
    public static class DTOMapper
    {
        public static AccountDTO MapAccountDTO(Account account)
        {
            var accountDTO = new AccountDTO
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                AccountBalance = account.AccountBalance
            };

            return accountDTO;
        }

        public static Account ReverseMapAccountDTO(AccountDTO accountDTO)
        {
            var account = new Account
            {
                AccountId = accountDTO.AccountId,
                AccountName = accountDTO.AccountName,
                AccountBalance = accountDTO.AccountBalance
            };

            return account;
        }
    }
    #endregion
}
