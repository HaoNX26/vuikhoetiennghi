using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.SubmitCustomerWin
{
    [Table("B_SUBMIT_CUSTOMER_WIN")]
    public class SubmitInfoCustomerWin
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("PROVINCE_ID")]
        public long? ProvinceId { get; set; }
        [Column("DISTRICT_ID")]
        public long? DistrictsId { get; set; }
        [Column("WARD_ID")]
        public long? WardId { get; set; }
        [Column("ASC_ID")]
        public long? AscId { get; set; }
        [Column("FULL_NAME")]
        public string FullName { get; set; }
        [Column("BANK_NUMBER")]
        public string BankNumber { get; set; }
        [Column("BANK_NAME_ID")]
        public long? BankNameId { get; set; }
        [Column("BANK_ACCOUNT_NAME")]
        public string BankAccountName { get; set; }
        [Column("PHONE_NUMBER")]
        public string PhoneNumber { get; set; }
        [Column("BANK_BRANCH")]
        public string BankBranch { get; set; }
        [Column("PROVINCE_BRANCH_ID")]
        public long ProvinceBranchId { get; set; }
        [Column("IDENTIFICATION_NUMBER")]
        public string IdentificationNumber { get; set; }
        [Column("FILE_ID_FRONT_PATH")]
        public string FileIDFront { get; set; }
        [Column("FILE_ID_BACKSIDE_PATH")]
        public string FileIDBackside { get; set; }
        [Column("PRODUCT_NAME")]
        public string ProductName { get; set; }
        [Column("ENGINE_NO")]
        public string EngineNo { get; set; }
        [Column("MODEL_NAME")]
        public string ModelName { get; set; }
        [Column("NOTE")]
        public string Note { get; set; }
        [Column("NOTE_OF_ASC")]
        public string NoteOfAsc { get; set; }
        [Column("ADDRESS")]
        public string Address { get; set; }
        [Column("CUSTOMER_STATUS")]
        public long? CustomerStatus { get; set; }
        [Column("PAN_STATUS")]
        public long? PanStatus { get; set; }
        [Column("FILE_SERIAL_NUMBER")]
        public string FileSerialNumber { get; set; }
        [Column("FILE_SMS_CODE")]
        public string FileSMSCode { get; set; }
        [Column("FILE_SMS_WIN")]
        public string FileSMSWin { get; set; }
        [Column("IDENTIFICATION_AUTHORIZATION")]
        public string IdentificationAuthorization { get; set; }
        [Column("FILE_PRODUCT_PATH")]
        public string FileProduct { get; set; }
        [Column("FILE_INVOICE_PATH")]
        public string FileInvoice { get; set; }
        [Column("FILE_UPLOAD_OF_ASC")]
        public string FileUploadOfAsc { get; set; }
        [Column("FILE_AUTHORIZATION_LETTER")]
        public string FileAuthozizationLetter { get; set; }
        [Column("COUNT_SUBMIT")]
        public long? CountSubmit { get; set; }
        [Column("CREATED_BY")]
        public int? CreatedBy { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("MODIFIED_BY")]
        public int? ModifiedBy { get; set; }
        [Column("MODIFY_DATE")]
        public DateTime? ModifyDate { get; set; }

        public string NOTE_OF_AGENCY { get; set; }
        public string NOTE_OF_CALL_CENTER { get; set; }
        public string NOTE_OF_ADMIN { get; set; }

        public string NOTE_CUS_REJECT { get; set; }
        public string NOTE_OF_CUSTOMER { get; set; }
        public string FILE_OF_CALL_CENTER { get; set; }
        public string FILE_OF_AGENCY { get; set; }
        public long? NOTE_CUS_REJECT_ID { get; set; }
        public string FILE_ID_FRONT_AUTHORIZATION { get; set; }
        public string FILE_ID_BACKSIDE_AUTHORIZATION { get; set; }

        public long ? ASC_STATUS { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ASC_PROCESS_DEADLINE { get; set; }
        public DateTime? REJECT_DATE { get; set; }
    }
}
