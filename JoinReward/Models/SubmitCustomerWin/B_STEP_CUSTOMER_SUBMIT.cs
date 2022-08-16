using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.SubmitCustomerWin
{
    [Table("B_STEP_CUSTOMER_SUBMIT")]
    public class B_STEP_CUSTOMER_SUBMIT
    {
        [Key]
        public long ID { get; set; }
        public long CUSTOMER_SUBMIT_ID { get; set; }
        public long STEP_ID { get; set; }

        public string NOTE_ERROR { get; set; }
    }
}
