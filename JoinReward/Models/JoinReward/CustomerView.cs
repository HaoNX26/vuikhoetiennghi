using System;
using System.ComponentModel.DataAnnotations;

namespace JoinReward.Models.JoinReward
{
    public class CustomerView
    {
        public long Id { get; set; }
        public long ProvinceId { get; set; }
        public long DistrictsId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public string FileIDFront { get; set; }
        public string FileIDBackside { get; set; }
        public long ProductNameId { get; set; }
        public long ModelId { get; set; }
        public string Address { get; set; }
        public string Capchar { get; set; }
        public string PurchaseDate { get; set; }
        public string FileWarranty { get; set; }
    }
}
