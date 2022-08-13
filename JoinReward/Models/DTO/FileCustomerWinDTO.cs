using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.DTO
{
    public class FileCustomerWinDTO
    {
        public string CustmerName { get; set; }
        public long AscId { get; set; }
        public string PhoneNumber { get; set; }
        public string LuckyCode { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public long StatusId { get; set; }
        public long ASC_STATUS { get; set; }
        public long PAN_STATUS { get; set; }
        public long PROVINCE_ID { get; set; }
        public long ROUND_ID { get; set; }
        public long PRIZE_WIN { get; set; }
        public long COUNT_SUBMIT { get; set; }
        public long AGENCY_ID { get; set; }
        public long CUSTOMER_STATUS { get; set; }
        [NotMapped]
        public long PAGE_SIZE { get; set; }
        [NotMapped]
        public long page { get; set; }
        [NotMapped]
        public String KEYWORD { get; set; }
        public String fid { get; set; }
        public int NUM_OF_RECORD { get; set; }
    }
}
