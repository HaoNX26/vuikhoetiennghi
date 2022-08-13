using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.DTO
{
    public class ExportCustomerRegisterDTO
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("FULL_NAME")]
        public string FullName { get; set; }
        [Column("PHONE_NUMBER")]
        public string PhoneNumber { get; set; }
        [Column("IDENTIFICATION_NUMBER")]
        public string IdentifycationNumber { get; set; }
        [Column("PRODUCT_NAME")]
        public string ProductName { get; set; }
        [Column("MODEL_NAME")]
        public string ModelName { get; set; }
        [Column("ADDRESS")]
        public string Addrress { get; set; }
        [Column("DISTRICT_NAME")]
        public string DistrictName { get; set; }
        [Column("PROVINCE_NAME")]
        public string ProviceName { get; set; }
        [Column("PURCHASE_DATE")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }
        [Column("FILE_ID_FRONT_PATH")]
        public string FileFrontId { get; set; }
        [Column("FILE_ID_BACKSIDE_PATH")]
        public string FileBacksideId { get; set; }
        [Column("FILE_WARRANTY_PATH")]
        public string FileWarranty { get; set; }
        [Column("FILE_PRODUCT_PATH")]
        public string FileProduct { get; set; }

        public int NUM_OF_RECORD { get; set; }
    }
}
