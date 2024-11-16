using BankingApplication.UI.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.UI.EntityFramework.DBContext
{
    public class BankingDBContext : DbContext
    {
        public BankingDBContext(DbContextOptions<BankingDBContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildAccountsModel(modelBuilder);
            BuildTransactionsModel(modelBuilder);
        }

        private void BuildAccountsModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(x => x.AccountId);
        }

        private void BuildTransactionsModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().HasKey(x => x.TransactionID);
        }
    }
}
