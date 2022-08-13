using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.DTO
{
    public class ASCMasterDTO
    {
        [Key]
         
        public long ID { get; set; }
        [Column("ASC_CODE")]
        public string ASC_CODE { get; set; }
        [Column("ASC_NAME")]
        public string ASC_NAME { get; set; }

        [Column("ASC_EMAIL")]
        public string ASC_EMAIL { get; set; }
        [Column("USERNAME")]
        public string USERNAME { get; set; }

        [NotMapped]
        public long PAGE_SIZE { get; set; }
        [NotMapped]
        public long page { get; set; }
        [NotMapped]
        public String KEYWORD { get; set; }
        public int NUM_OF_RECORD { get; set; }
    }
}
