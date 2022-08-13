using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.JoinReward
{
    [Table("B_CUSTOMER_REGISTER")]
    public class AppRegisterModel
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("PROVINCE_ID")]
        public long ProvinceId { get; set; }
        [Column("DISTRICT_ID")]
        public long DistrictsId { get; set; }
        [Column("FULL_NAME")]
        public string FullName { get; set; }
        [Column("PHONE_NUMBER")]
        public string PhoneNumber { get; set; }
        [Column("IDENTIFICATION_NUMBER")]
        public string IdentificationNumber { get; set; }
        [Column("FILE_ID_FRONT_PATH")]
        public string FileIDFrontPath { get; set; }
        [Column("FILE_ID_BACKSIDE_PATH")]
        public string FileIDBacksidePath { get; set; }
        [Column("FILE_PRODUCT_PATH")]
        public string FileProductPath { get; set; }
        [Column("PRODUCT_NAME_ID")]
        public long ProductNameId { get; set; }
        [Column("MODEL_ID")]
        public long ModelId { get; set; }
        [Column("ADDRESS")]
        public string Address { get; set; }
        [Column("PURCHASE_DATE")]
        public DateTime PurchaseDate { get; set; }
        [Column("FILE_WARRANTY_PATH")]
        public string FileWarrantyPath { get; set; }

        [Column("CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }
    }
}
