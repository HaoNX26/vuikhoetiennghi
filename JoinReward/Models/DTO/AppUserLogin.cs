using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JoinReward.Models.DTO
{
    public class AppUserLogin
    {
        public long ID { get; set; }

        [Required]
        public string USERNAME { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string PASSWORD { get; set; }
        public long DLR_ID { get; set; }
        public string FULLNAME { get; set; }
        public DateTime LASTEST_LOGIN_DATE { get; set; }
        public string RESPONSE_MESSAGE { get; set; }
        public bool? HAS_TO_CHANGE_PASSS { get; set; }
    }
}
