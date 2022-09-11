using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.DTO
{
    public class MCallCustomerDataDTO
    {
        public long ID { get; set; }
        public string LUCKY_CODE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CALL_RESULT_1ST { get; set; }
        public string NOTE_1ST { get; set; }
        public string CALL_RESULT_2ND { get; set; }
        public string NOTE_2ND { get; set; }
        public string CALL_RESULT_3RD { get; set; }
        public string NOTE_3RD { get; set; }
        public string CALL_RESULT_4TH { get; set; }
        public string NOTE_4TH { get; set; }
        public string CALL_RESULT_5TH { get; set; }
        public string NOTE_5TH { get; set; }

        [NotMapped]
        public long PAGE_SIZE { get; set; }
        [NotMapped]
        public long page { get; set; }
        [NotMapped]
        public String KEYWORD { get; set; }
        public int NUM_OF_RECORD { get; set; }
    }
}
