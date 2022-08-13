using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.DTO
{
    public class SearchCustomerRegisterDTO
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        [NotMapped]
        public long PAGE_SIZE { get; set; }
        [NotMapped]
        public long page { get; set; }
        [NotMapped]
        public String KEYWORD { get; set; }
        public int NUM_OF_RECORD { get; set; }
    }
}
