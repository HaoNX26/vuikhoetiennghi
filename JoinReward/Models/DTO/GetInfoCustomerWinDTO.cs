using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.DTO
{
    public class GetInfoCustomerWinDTO
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("FULL_NAME")]
        public string CustomerName { get; set; }
        [Column("LUCKY_CODE")]
        public string LuckyCode { get; set; }
        [Column("ASC_NAME")]
        public string AscName { get; set; }
        [Column("PHONE_NUMBER")]
        public string PhoneNumber { get; set; }
        public int NUM_OF_RECORD { get; set; }
    }
}
