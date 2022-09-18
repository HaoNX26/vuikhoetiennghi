using PAN.Warranty.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.Master
{
    [Table("M_CALL_CUSTOMER_DATA_IMPORT")]
    public class MCallCustomerDataImport
    {
        public long ID { get; set; }
        [Import(DisplayName = "Lucky Code")]
        public string LUCKY_CODE { get; set; }
        [Import(DisplayName = "Họ Và Tên")]
        public string CUSTOMER_NAME { get; set; }
        [Import(DisplayName = "Kết quả lần 1")]
        public string CALL_RESULT_1ST { get; set; }
        [Import(DisplayName = "Ghi chú lần 1")]
        public string NOTE_1ST { get; set; }
        [Import(DisplayName = "Kết quả lần 2")]
        public string CALL_RESULT_2ND { get; set; }
        [Import(DisplayName = "Ghi chú lần 2")]
        public string NOTE_2ND { get; set; }
        [Import(DisplayName = "Kết quả lần 3")]
        public string CALL_RESULT_3RD { get; set; }
        [Import(DisplayName = "Ghi chú lần 3")]
        public string NOTE_3RD { get; set; }
        [Import(DisplayName = "Kết quả lần 4")]
        public string CALL_RESULT_4TH { get; set; }
        [Import(DisplayName = "Ghi chú lần 4")]
        public string NOTE_4TH { get; set; }
        [Import(DisplayName = "Kết quả lần 5")]
        public string CALL_RESULT_5TH { get; set; }
        [Import(DisplayName = "Ghi chú lần 5")]
        public string NOTE_5TH { get; set; }
        public string REMARK { get; set; }
    }
}
