using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.Master
{
    [Table("M_STATUS")]
    public class MStatus
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("STATUS")]
        public string Status { get; set; }
        [Column("TYPE_STATUS")]
        public long TypeStatus { get; set; }
    }
}
