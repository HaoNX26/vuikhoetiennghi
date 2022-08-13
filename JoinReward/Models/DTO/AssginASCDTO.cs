using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.DTO
{
    public class AssginASCDTO
    {
        public string CustmerName { get; set; }
        public long AscId { get; set; }
        public string PhoneNumber { get; set; }
        public string LuckyCode { get; set; }
        public long StatusId { get; set; }

        [NotMapped]
        public long PAGE_SIZE { get; set; }
        [NotMapped]
        public long page { get; set; }
        [NotMapped]
        public String KEYWORD { get; set; }
        public int NUM_OF_RECORD { get; set; }
    }
}
