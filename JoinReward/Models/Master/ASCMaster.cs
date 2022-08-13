using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.Master
{
    [Table("M_ASC")]
    public class ASCMaster
    {
        [Key]
        
        public long ID { get; set; }
        
        public string ASC_CODE { get; set; }
       
        public string ASC_NAME { get; set; }
        public string ASC_EMAIL { get; set; }
        public string ASC_TYPE { get; set; }
        public long? CREATED_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public long? MODIFIED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
    }
}
