using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.Master
{
    [Table("CALL_CUSTOMER_DATA_V")]
    public class VCallCustomerData
    {
        [Key]
        public int SEQ { get; set; }
        public string LUCKY_CODE { get; set; }
        public string STATUS { get; set; }
        public string NOTE { get; set; }
    }
}
