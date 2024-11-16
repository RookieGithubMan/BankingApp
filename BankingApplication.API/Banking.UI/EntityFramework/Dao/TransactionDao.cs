using BankingApplication.UI.EntityFramework.DBContext;
using BankingApplication.UI.EntityFramework.Models;

namespace Banking.UI.EntityFramework.Dao
{
    public interface ITransactionDao
    {
        public List<Transaction> GetTransactions();
        public void CreateTransaction(Transaction transaction);
    }

    public class TransactionDao : ITransactionDao
    {
        private readonly BankingDBContext bankingDBContext;

        public TransactionDao(BankingDBContext bankingDBContext)
        {
            this.bankingDBContext = bankingDBContext;
        }

        public List<Transaction> GetTransactions()
        {
            var transactions = bankingDBContext.Transactions.ToList();
            return transactions;
        }

        public void CreateTransaction(Transaction transaction)
        {
            bankingDBContext.Transactions.Add(transaction);
            bankingDBContext.SaveChanges();
        }
    }
}
