using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.DTO
{
    public class FileCustomerWinSearchDTO
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("FULL_NAME")]
        public string CustomerName { get; set; }
        [Column("LUCKY_CODE")]
        public string LuckyCode { get; set; }
        [Column("BANK_NUMBER")]
        public string BankNumber { get; set; }
        [Column("BANK_NAME")]
        public string BankName { get; set; }
        [Column("BANK_ACCOUNT_NAME")]
        public string BankAccountName { get; set; }
        [Column("BANK_BRANCH")]
        public string BankBranch { get; set; }
        [Column("MODEL_NAME")]
        public string ModelName { get; set; }
        [Column("ASC_NAME")]
        public string AscName { get; set; }
        [Column("PHONE_NUMBER")]
        public string PhoneNumber { get; set; }
        [Column("IDENTIFICATION_NUMBER")]
        public string IdentificationNumber { get; set; }
        [Column("ADDRESS")]
        public string Address { get; set; }
        [Column("ENGINE_NO")]
        public string EngineNo { get; set; }
        [Column("CUSTOMER_STATUS")]
        public string CustomerStatus { get; set; }
        [Column("PAN_STATUS")]
        public string PanStatus { get; set; }
        [Column("DISTRICT_NAME")]
        public string DistrictName { get; set; }
        [Column("PROVINCE_NAME")]
        public string ProvinceName { get; set; }
        public string ROUND_NAME { get; set; }
        public string WARD_NAME { get; set; }
        public string NOTE_OF_AGENCY { get; set; }
        public string NOTE_CUS_REJECT { get; set; }
        public string NOTE_OF_ASC { get; set; }
        public string NOTE_OF_CALL_CENTER { get; set; }
        public string ASC_STATUS { get; set; }
        public string PROVINCE_BRANCH { get; set; }
        public string NOTE_OF_CUSTOMER { get; set; }
        public string AGENCY_NAME { get; set; }
        public string IDENTIFICATION_AUTHORIZATION { get; set; }
        public long COUNT_SUBMIT { get; set; }
        public string PRIZE_WIN_TEXT { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CREATE_DATE { get; set; }
        [Column("NOTE")]
        public string Note { get; set; }
        public int NUM_OF_RECORD { get; set; }
    }
}
