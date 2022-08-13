using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PAN.Warranty.Models.Master.JoinReward
{
    public class M_PROVINCE
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public string PROVINCE_NAME { get; set; }
        [Required]
        public long REGION_ID { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        
    }
}
