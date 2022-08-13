using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JoinReward.Models.DTO
{
    public class M_OTP_DTO
    {
        [Key]

        public long ID { get; set; }
       
        public string OTP { get; set; }
        public string CUSTOMER_TEL { get; set; }
        [NotMapped]
        public long PAGE_SIZE { get; set; }
        [NotMapped]
        public long page { get; set; }
        [NotMapped]
        public String KEYWORD { get; set; }
        public int NUM_OF_RECORD { get; set; }
    }
}
