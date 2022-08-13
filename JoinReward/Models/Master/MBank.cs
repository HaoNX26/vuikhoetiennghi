using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.Master
{
    [Table("M_BANK")]
    public class MBank
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("BANK_NAME")]
        public string BankName { get; set; }
        [Column("ABBREVIATED_NAME")]
        public string AbbreviatedName { get; set; }
    }
}
