using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Views.Master
{
    [Table("M_STEP")]
    public class M_STEP
    {
        [Key]

        public long ID { get; set; }

        public int STEP { get; set; }

        public string STEP_NAME { get; set; }
    }
}
