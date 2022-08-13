using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.SubmitCustomerWin
{
    [Table("B_CUSTOMER_WIN")]
    public class BCustomerWin
    {
        [Key]
        [Column("ID")]
        public long ID { get; set; }

        [Column("PHONE_NUMBER")]
        public string PhoneNumber { get; set; }

        [Column("LUCKY_CODE")]
        public string LuckyCode { get; set; }
        [Column("CUSTOMER_NAME")]
        public string CustomerName { get; set; }

        [Column("ADDRESS")]
        public string Address { get; set; }

        [Column("MODEL_NAME")]
        public string ModelName { get; set; }

        [Column("ENGINE_NO")]
        public string EngineNo { get; set; }

        [Column("PRIZE_WIN")]
        public long PrizeWin { get; set; }
        [Column("ROUND_ID")]
        public long RoundId { get; set; } 
        [Column("TYPE_PRODUCT")]
        public string TypeProduct { get; set; }

        [Column("CREATED_BY")]
        public int? CreatedBy { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("MODIFIED_BY")]
        public int? ModifiedBy { get; set; }
        [Column("MODIFY_DATE")]
        public DateTime? ModifyDate { get; set; }
    }
}
