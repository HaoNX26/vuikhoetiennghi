using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace JoinReward.Models
{
    public class AppRegisterDTO
    {
        public long Id { get; set; }
        public long ProvinceId { get; set; }
        public long DistrictsId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationNumber { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile FileIDFront { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile FileIDBackside { get; set; }
        public long ProductNameId { get; set; }
        public long ModelId { get; set; }
        public string Address { get; set; }
        public string Capchar { get; set; }
        public string PurchaseDate { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile FileWarranty { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile FileProduct { get; set; }
    }
}
