using System;
using System.ComponentModel.DataAnnotations;

namespace JoinReward.Models.Log
{
    public class B_INPUT_PHONE_LOG
    {
        [Key]
        public long ID { get; set; }
        public string PHONE_NUMBER {get;set;} 
        public string NOTE { get;set;} 
        public DateTime? CREATED_DATE { get; set; }
    }
}
