using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.UI.EntityFramework.Models
{
    [Table("t_account_transactions_transfer")]
    public class Transaction
    {
        [Column("TransactionID")]
        public long TransactionID { get; set; }
        [Column("AccountId_From")]
        public long AccountId_From { get; set; }
        [Column("AccountBalance_From")]
        public decimal AccountBalance_From { get; set; }
        [Column("AccountId_To")]
        public long AccountId_To { get; set; }
        [Column("AccountBalance_To")]
        public decimal AccountBalance_To { get; set; }
        [Column("TransactionTime")]
        public DateTime TransactionTime { get; set; }
        [Column("Amount")]
        public decimal Amount { get; set; }
    }
}
