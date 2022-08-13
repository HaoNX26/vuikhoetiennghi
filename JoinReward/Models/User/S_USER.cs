using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JoinReward.Models.User
{
    public class S_USER
    {
        [Key]
        public long ID { get; set; }
        public long ASC_ID { get; set; }
        public DateTime? LASTEST_LOGIN_DATE { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
       
        public long? NUM_OF_LOGIN_FAILT { get; set; }

        [Required]
         
        public long? CREATED_BY { get; set; }
        public long? MODIFIED_BY { get; set; }
        [Required]
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        
        public string ROLES { get; set; }
        [Required]
        public string FULLNAME { get; set; }
        public bool IS_EXPIRED { get; set; }
    }
}
