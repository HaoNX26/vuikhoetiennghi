using System;

namespace JoinReward.Models.JoinReward
{
    public class ExportExcelCustomer
    {
        public long Id { get; set; }
        public string Province { get; set; }
        public string Districts { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public string FileIDFront { get; set; }
        public string FileIDBackside { get; set; }
        public string ProductName { get; set; }
        public string Model { get; set; }
        public string Address { get; set; }
        public string Capchar { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string FileWarranty { get; set; }
        public string FileProduct { get; set; }
    }
}
