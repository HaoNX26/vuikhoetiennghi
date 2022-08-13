using System.ComponentModel.DataAnnotations;

namespace JoinReward.Models.JoinReward
{
    public class M_PRODUCT_NAME
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public string PRODUCT_NAME { get; set; }
    }
}
