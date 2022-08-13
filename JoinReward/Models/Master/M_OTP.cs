using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.Master
{
    [Table("M_OTP")]
    public class M_OTP
    {
        [Key]
        
        public long ID { get; set; }
        
        public string OTP { get; set; }
       
        public string CUSTOMER_TEL { get; set; }
       
        public long? CREATED_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public long? MODIFIED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
    }
}
