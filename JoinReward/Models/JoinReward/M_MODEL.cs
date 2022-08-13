using System.ComponentModel.DataAnnotations;

namespace JoinReward.Models.JoinReward
{
    public class M_MODEL
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public string MODEL_NAME { get; set; }

        public long PRODUCT_ID { get; set; }

    }
}
