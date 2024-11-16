using System.ComponentModel.DataAnnotations.Schema;
namespace BankingApplication.UI.EntityFramework.Models
{
    [Table("t_accounts")]
    public class Account
    {
        [Column("AccountId")]
        public long AccountId { get; set; }
        [Column("AccountName")]
        public string AccountName { get; set; }
        [Column("AccountBalance")]
        public decimal AccountBalance { get; set; }
    }
}
