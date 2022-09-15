using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.SubmitCustomerWin
{
    [Table("B_PROCESSING_FILE")]
    public class BProcessingFile
    {
        [Key]
        public long ID { get; set; }
        public long FILE_CUSTOMER_ID { get; set; }
        public long STATUS_AGENCY { get; set; }
        public long STATUS_ASC { get; set; }
        public long REASON_AGENCY { get; set; }
        public long REASON_ASC { get; set; }
        public string FILE_OF_ASC { get; set; }
        public string FILE_OF_AGENCY { get; set; }
        public long ORDERING { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public long? CREATED_BY { get; set; }
        public long? MODIFIED_BY { get; set; }

    }
}
