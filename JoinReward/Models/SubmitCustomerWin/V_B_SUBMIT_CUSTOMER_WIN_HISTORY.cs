﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.SubmitCustomerWin
{
     [Table("V_B_SUBMIT_CUSTOMER_WIN_HISTORY")]
    public class V_B_SUBMIT_CUSTOMER_WIN_HISTORY
    {
        public long ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string ADDRESS { get; set; }

        public string ENGINE_NO { get; set; }
        public string MODEL_NAME { get; set; }
        public string LUCKY_CODE { get; set; }
        public string PRIZE_WIN_TEXT { get; set; }
        public long? ASC_ID { get; set; }
        public string NOTE { get; set; }
        public string NOTE_OF_ASC { get; set; }
        public long? PAN_STATUS { get; set; }

        public long? ASC_STATUS { get; set; }
        public long? CUSTOMER_STATUS { get; set; }
        public string NOTE_OF_AGENCY { get; set; }
        public string NOTE_OF_CALL_CENTER { get; set; }
        public string NOTE_OF_ADMIN { get; set; }
        public string ADDRESS_STORAGE { get; set; }
        public string BANK_ACCOUNT_NAME { get; set; }
        public string BANK_NAME { get; set; }
        public string BANK_NUMBER { get; set; }
        public string BANK_BRANCH { get; set; }
        public string IDENTIFICATION_AUTHORIZATION { get; set; }
        public string FULL_NAME { get; set; }
        public string IDENTIFICATION_NUMBER { get; set; }
        public string DISTRICT_NAME { get; set; }
        public string PROVINCE_NAME { get; set; }
        public string WARD_NAME { get; set; }
        public string FILE_AUTHORIZATION_LETTER { get; set; }
        public string FILE_ID_BACKSIDE_PATH { get; set; }
        public string FILE_ID_FRONT_PATH { get; set; }
        public string FILE_INVOICE_PATH { get; set; }
        public string FILE_PRODUCT_PATH { get; set; }
        public string FILE_SERIAL_NUMBER { get; set; }
        public string FILE_SMS_CODE { get; set; }
        public string FILE_SMS_WIN { get; set; }
        public string FILE_UPLOAD_OF_ASC_VIEW { get; set; }
        public string ROUND_NAME { get; set; }
        public string NOTE_OF_CUSTOMER { get; set; }
        public string NOTE_CUS_REJECT { get; set; }
        public string PROVINCE_BRANCH { get; set; }
        public long? NOTE_CUS_REJECT_ID { get; set; }
        public DateTime? ASC_PROCESS_DEADLINE { get; set; }
        public string FILE_OF_CALL_CENTER_VIEW { get; set; }
        public string FILE_OF_AGENCY_VIEW { get; set; }
        public string FILE_ID_FRONT_AUTHORIZATION { get; set; }
        public string FILE_ID_BACKSIDE_AUTHORIZATION { get; set; }
    }
}
