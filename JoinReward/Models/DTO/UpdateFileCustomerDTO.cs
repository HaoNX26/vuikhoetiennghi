using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JoinReward.Models.DTO
{
    public class UpdateFileCustomerDTO
    {
        public long Id { get; set; }
        public long AssignAscId { get; set; }
        public string Note { get; set; }

        [DataType(DataType.Upload)]
        public List<IFormFile> FileASCUpdate { get; set; }
    }
}
