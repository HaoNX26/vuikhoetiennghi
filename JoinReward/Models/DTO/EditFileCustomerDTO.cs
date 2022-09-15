using System.Collections.Generic;

namespace JoinReward.Models.DTO
{
    public class EditFileCustomerDTO
    {
        public long FILE_CUSTOMER_ID { get; set; }
        public string fid { get; set; }
        public List<BProcessingFileDTO> bProcessingFileDTOs { get; set; }
    }
}
