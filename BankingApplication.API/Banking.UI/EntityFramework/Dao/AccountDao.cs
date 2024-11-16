using BankingApplication.UI.EntityFramework.DBContext;
using BankingApplication.UI.EntityFramework.Models;

namespace Banking.UI.EntityFramework.Dao
{
    public interface IAccountDao
    {
        public List<Account> GetAccounts();
        public Account GetAccountByAccountId(long accountId);
        public void CreateAccount(Account account);
        public void UpdateAccount(Account account);
    }

    public class AccountDao : IAccountDao
    {
        private readonly BankingDBContext bankingDBContext;

        public AccountDao(BankingDBContext bankingDBContext)
        {
            this.bankingDBContext = bankingDBContext;
        }

        public List<Account> GetAccounts()
        {
            var accounts = bankingDBContext.Accounts.ToList();
            return accounts;
        }

        public Account GetAccountByAccountId(long accountId)
        {
            var account = bankingDBContext.Accounts.Where(x => x.AccountId == accountId).First();
            return account;
        }

        public void CreateAccount(Account account)
        {
            bankingDBContext.Accounts.Add(account);
            bankingDBContext.SaveChanges();
        }

        public void UpdateAccount(Account account)
        {
            bankingDBContext.Accounts.Update(account);
            bankingDBContext.SaveChanges();
        }
    }
}
