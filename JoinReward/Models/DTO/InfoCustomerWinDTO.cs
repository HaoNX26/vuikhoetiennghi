using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JoinReward.Models.DTO
{
    public class InfoCustomerWinDTO
    {
        public long Id { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictsId { get; set; }
        public long? WardId { get; set; }
        public string FullName { get; set; }
        public string LuckyCode { get; set; }
        public string FullNameInfo { get; set; }
        public string AddressInfo { get; set; }
        public string PrizeWin { get; set; }
        public string BankNumber { get; set; }
        public long? BankNameId { get; set; }
        public string BankAccountName { get; set; }
        public string BankBranch { get; set; }
        public long ProvinceBranchId { get; set; }

        public string BankNumberAuth { get; set; }
        public long? BankNameIdAuth { get; set; }
        public string BankAccountNameAuth { get; set; }
        public string BankBranchAuth { get; set; }
        public long ProvinceBranchIdAuth { get; set; }

        public string IdentificationAuthorization { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationNumber { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile FileIDFront { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile FileIDBackside { get; set; }
        public string ProductName { get; set; }
        public string EngineNo { get; set; }
        public string ModelName { get; set; }
        public string TypeWin { get; set; }
        public string NoteOfCus { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Upload)]
        public List<IFormFile> FileSerialNumber { get; set; }
        [DataType(DataType.Upload)]
        public List<IFormFile> FileSMSCode { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile FileProduct { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile FileInvoice { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile FILE_ID_FRONT_AUTHORIZATION { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile FILE_ID_BACKSIDE_AUTHORIZATION { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile FileVerification { get; set; }
        [DataType(DataType.Upload)]
        public List<IFormFile> FileSMSWin { get; set; }
        [DataType(DataType.Upload)]
        public List<IFormFile> FileAuthozizationLetter { get; set; }

        public string Note { get; set; }
        public long? Status { get; set; }
        public long TypeBank { get; set; }
    }
}
