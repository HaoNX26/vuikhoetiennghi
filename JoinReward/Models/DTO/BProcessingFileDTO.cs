using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace JoinReward.Models.DTO
{
    public class BProcessingFileDTO
    {
        public long ID { get; set; }
        public long STATUS_AGENCY { get; set; }
        public long STATUS_ASC { get; set; }
        public long REASON_AGENCY { get; set; }
        public long REASON_ASC { get; set; }
        public List<IFormFile> FILE_OF_ASC { get; set; }
        public List<IFormFile> FILE_OF_AGENCY { get; set; }
        public long ORDERING { get; set; }
    }
}
