using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.Master
{
    [Table("M_WARD")]
    public class MWard
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("DISTRICT_ID")]
        public long DistrictId { get; set; }
        [Column("WARD_NAME")]
        public string WardName { get; set; }
    }
}
