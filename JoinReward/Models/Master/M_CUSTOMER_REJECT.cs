using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JoinReward.Models.Master
{
    public class M_CUSTOMER_REJECT
    {
        [Key]
        public long ID { get; set; }

        public string REASON { get; set; }
    }
}
